using DekelApp.Models;
using DekelApp.Utils;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace DekelApp.ViewModels
{
    public class TichumViewModel : BaseViewModel
    {
        private readonly AppData _appData;

        public ObservableCollection<TichumAreaModel> TichumAreas => _appData.TichumAreas;

        public TichumAreaModel? CurrentArea
        {
            get => _appData.SelectedTichumArea;
            set
            {
                var oldArea = _appData.SelectedTichumArea;
                if (oldArea != null)
                {
                    oldArea.Coordinates.CollectionChanged -= Coordinates_CollectionChanged;
                    foreach (var c in oldArea.Coordinates) c.PropertyChanged -= Coordinate_PropertyChanged;
                }

                if (_appData.SelectedTichumArea != value)
                {
                    _appData.SelectedTichumArea = value;
                    OnPropertyChanged(nameof(CurrentArea));
                    
                    if (_appData.SelectedTichumArea != null)
                    {
                        _appData.SelectedTichumArea.Coordinates.CollectionChanged += Coordinates_CollectionChanged;
                        foreach (var c in _appData.SelectedTichumArea.Coordinates) c.PropertyChanged += Coordinate_PropertyChanged;
                    }
                    OnPropertyChanged(nameof(IsEditingArea));
                    OnPropertyChanged(nameof(IsListView));
                    UpdateDuplicates();
                }
            }
        }

        public bool IsEditingArea => CurrentArea != null;
        public bool IsListView => CurrentArea == null;

        public ICommand AddNewAreaCommand { get; }
        public ICommand EditAreaCommand { get; }
        public ICommand DeleteAreaCommand { get; }
        public ICommand FinishAreaCommand { get; }
        
        public ICommand AddCoordinateCommand { get; }
        public ICommand DeleteCoordinateCommand { get; }
        public ICommand UploadFileCommand { get; }
        public ICommand ToggleToUTMCommand { get; }
        public ICommand ToggleToGeographicCommand { get; }

        public CoordinateSystemType CoordinateSystem
        {
            get => CurrentArea?.CoordinateSystem ?? CoordinateSystemType.UTM;
            set
            {
                if (CurrentArea != null)
                {
                    CurrentArea.CoordinateSystem = value;
                    OnPropertyChanged(nameof(CoordinateSystem));
                    OnPropertyChanged(nameof(IsUTM));
                    OnPropertyChanged(nameof(IsGeographic));
                    UpdateDuplicates();
                }
            }
        }

        public bool IsUTM => CoordinateSystem == CoordinateSystemType.UTM;
        public bool IsGeographic => CoordinateSystem == CoordinateSystemType.Geographic;

        public TichumViewModel(AppData appData)
        {
            _appData = appData;

            RefreshAreaNames();
            if (CurrentArea == null)
            {
                CurrentArea = TichumAreas.FirstOrDefault();
            }
            AddNewAreaCommand = new RelayCommand(_ => AddNewArea());
            EditAreaCommand = new RelayCommand(area => EditArea(area));
            DeleteAreaCommand = new RelayCommand(area => DeleteArea(area));
            FinishAreaCommand = new RelayCommand(_ => FinishArea());

            AddCoordinateCommand = new RelayCommand(_ => AddCoordinate());
            DeleteCoordinateCommand = new RelayCommand(coord => DeleteCoordinate(coord));
            UploadFileCommand = new RelayCommand(_ => UploadFile());
            ToggleToUTMCommand = new RelayCommand(_ => CoordinateSystem = CoordinateSystemType.UTM);
            ToggleToGeographicCommand = new RelayCommand(_ => CoordinateSystem = CoordinateSystemType.Geographic);

            TichumAreas.CollectionChanged += TichumAreas_CollectionChanged;
        }

        private void TichumAreas_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshAreaNames();
        }


        private void AddNewArea()
        {
            var newArea = new TichumAreaModel();
            TichumAreas.Add(newArea);
            CurrentArea = newArea;
        }

        private void RefreshAreaNames()
        {
            for (int i = 0; i < TichumAreas.Count; i++)
            {
                TichumAreas[i].Name = GetPriorityName(i);
            }
        }

        private string GetPriorityName(int index)
        {
            return index switch
            {
                0 => "First",
                1 => "Second",
                2 => "Third",
                3 => "Fourth",
                4 => "Fifth",
                5 => "Sixth",
                6 => "Seventh",
                7 => "Eighth",
                8 => "Ninth",
                9 => "Tenth",
                _ => $"Area {index + 1}"
            };
        }

        private void EditArea(object? area)
        {
            if (area is TichumAreaModel model)
            {
                CurrentArea = model;
            }
        }

        private void DeleteArea(object? area)
        {
            if (area is TichumAreaModel model)
            {
                var result = System.Windows.MessageBox.Show("Are you sure you want to delete this Tichum area?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    TichumAreas.Remove(model);
                }
            }
        }

        private void FinishArea()
        {
            if (CurrentArea == null) return;

            bool isFileUploaded = CurrentArea.IsFileUploaded;
            int validCount;
            
            if (CurrentArea.CoordinateSystem == CoordinateSystemType.UTM)
            {
                validCount = CurrentArea.Coordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);
            }
            else
            {
                validCount = CurrentArea.Coordinates.Count(c => c.IsLatitudeValid && c.IsLongitudeValid);
            }

            if (!isFileUploaded && validCount < 3)
            {
                System.Windows.MessageBox.Show("Please upload a file or add at least 3 valid coordinates before finishing this area.", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (CurrentArea.Coordinates.Any(c => c.IsDuplicate))
            {
                System.Windows.MessageBox.Show("Please correct or remove any identical coordinates before finishing.", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // Unsubscribe before clearing CurrentArea
            CurrentArea.Coordinates.CollectionChanged -= Coordinates_CollectionChanged;
            foreach (var c in CurrentArea.Coordinates) c.PropertyChanged -= Coordinate_PropertyChanged;

            CurrentArea = null;
        }

        private void AddCoordinate()
        {
            if (CurrentArea != null)
            {
                if (CoordinateSystem == CoordinateSystemType.UTM)
                {
                    CurrentArea.Coordinates.Add(new CoordinateModel() { Easting = "0", Northing = "0", Zone = "36N" });
                }
                else
                {
                    CurrentArea.Coordinates.Add(new CoordinateModel() { Latitude = "0", Longitude = "0" });
                }
            }
        }

        private void DeleteCoordinate(object? coord)
        {
            if (coord is CoordinateModel model)
            {
                CurrentArea?.Coordinates.Remove(model);
            }
        }

        private void UploadFile()
        {
            if (CurrentArea == null) return;

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Shapefiles (*.shp)|*.shp",
                Title = "Upload Tichum File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CurrentArea.IsFileUploaded = true;
                CurrentArea.FilePath = openFileDialog.FileName;
                
                // Extract coordinates using NetTopologySuite
                try
                {
                    CurrentArea.Coordinates.Clear();
                    CoordinateSystem = CoordinateSystemType.Geographic;
                    
                    using (var reader = new NetTopologySuite.IO.ShapefileDataReader(openFileDialog.FileName, new NetTopologySuite.Geometries.GeometryFactory()))
                    {
                        if (reader.Read())
                        {
                            var geom = reader.Geometry;
                            if (geom != null && geom.Coordinates.Length > 0)
                            {
                                var coords = geom.Coordinates;
                                int count = coords.Length;
                                if (count > 1 && coords[0].Equals2D(coords[count - 1]))
                                {
                                    count--; // Ignore the last point if it's identical to the first (Polygon closure)
                                }
                                for (int i = 0; i < count; i++)
                                {
                                    CurrentArea.Coordinates.Add(new CoordinateModel()
                                    {
                                        Longitude = coords[i].X.ToString(),
                                        Latitude = coords[i].Y.ToString()
                                    });
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show($"Failed to parse Shapefile: {ex.Message}", "Parsing Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }

                System.Windows.MessageBox.Show($"File '{openFileDialog.SafeFileName}' uploaded and coordinates extracted successfully.", "File Uploaded", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                
                // Final validation check
                UpdateDuplicates();

                // Auto-finish area as requested
                FinishArea();
            }
        }

        private void Coordinates_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (CoordinateModel c in e.OldItems) c.PropertyChanged -= Coordinate_PropertyChanged;
            if (e.NewItems != null)
                foreach (CoordinateModel c in e.NewItems) c.PropertyChanged += Coordinate_PropertyChanged;
            
            UpdateDuplicates();
        }

        private void Coordinate_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CoordinateModel.Easting) || e.PropertyName == nameof(CoordinateModel.Northing) || e.PropertyName == nameof(CoordinateModel.Zone) ||
                e.PropertyName == nameof(CoordinateModel.Latitude) || e.PropertyName == nameof(CoordinateModel.Longitude))
            {
                UpdateDuplicates();
            }
        }


        private void UpdateDuplicates()
        {
            if (CurrentArea == null) return;
            
            // reset all
            foreach (var c in CurrentArea.Coordinates) c.IsDuplicate = false;

            if (CoordinateSystem == CoordinateSystemType.UTM)
            {
                var groups = CurrentArea.Coordinates
                    .Where(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid)
                    .GroupBy(c => $"{c.Easting?.Trim()}_{c.Northing?.Trim()}_{c.Zone?.Trim()}")
                    .Where(g => g.Count() > 1);
                foreach (var g in groups)
                    foreach (var c in g) c.IsDuplicate = true;
            }
            else
            {
                var groups = CurrentArea.Coordinates
                    .Where(c => c.IsLatitudeValid && c.IsLongitudeValid)
                    .GroupBy(c => $"{c.Latitude?.Trim()}_{c.Longitude?.Trim()}")
                    .Where(g => g.Count() > 1);
                foreach (var g in groups)
                    foreach (var c in g) c.IsDuplicate = true;
            }
        }
    }
}
