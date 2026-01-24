using DekelApp.Models;
using DekelApp.Utils;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace DekelApp.ViewModels
{
    public class MikudViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        private MikudAreaModel? _currentArea;

        public ObservableCollection<MikudAreaModel> MikudAreas => _appData.MikudAreas;

        public MikudAreaModel? CurrentArea
        {
            get => _currentArea;
            set
            {
                if (SetProperty(ref _currentArea, value))
                {
                    OnPropertyChanged(nameof(IsEditingArea));
                    OnPropertyChanged(nameof(IsListView));
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

        public MikudViewModel(AppData appData)
        {
            _appData = appData;

            // Cleanup invalid areas (less than 3 coords AND no file) that might have been left over from previous navigation
            var invalidAreas = MikudAreas.Where(area => !area.IsFileUploaded && area.Coordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid) < 3).ToList();
            foreach (var area in invalidAreas)
            {
                MikudAreas.Remove(area);
            }

            AddNewAreaCommand = new RelayCommand(_ => AddNewArea());
            EditAreaCommand = new RelayCommand(area => EditArea(area));
            DeleteAreaCommand = new RelayCommand(area => DeleteArea(area));
            FinishAreaCommand = new RelayCommand(_ => FinishArea());

            AddCoordinateCommand = new RelayCommand(_ => AddCoordinate());
            DeleteCoordinateCommand = new RelayCommand(coord => DeleteCoordinate(coord));
            UploadFileCommand = new RelayCommand(_ => UploadFile());
        }

        private void AddNewArea()
        {
            var newArea = new MikudAreaModel { Name = "tel_aviv" };
            MikudAreas.Add(newArea);
            CurrentArea = newArea;
        }

        private void EditArea(object? area)
        {
            if (area is MikudAreaModel model)
            {
                CurrentArea = model;
            }
        }

        private void DeleteArea(object? area)
        {
            if (area is MikudAreaModel model)
            {
                var result = System.Windows.MessageBox.Show("Are you sure you want to delete this Mikud area?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    MikudAreas.Remove(model);
                }
            }
        }

        private void FinishArea()
        {
            if (CurrentArea == null) return;

            bool isFileUploaded = CurrentArea.IsFileUploaded;
            int validCount = CurrentArea.Coordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);

            if (!isFileUploaded && validCount < 3)
            {
                System.Windows.MessageBox.Show("Please upload a file or add at least 3 valid coordinates before finishing this area.", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            CurrentArea = null;
        }

        private void AddCoordinate()
        {
            CurrentArea?.Coordinates.Add(new CoordinateModel() { Easting = "0", Northing = "0", Zone = "36N" });
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
                Title = "Upload Mikud File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CurrentArea.IsFileUploaded = true;
                CurrentArea.FilePath = openFileDialog.FileName;
                System.Windows.MessageBox.Show($"File '{openFileDialog.SafeFileName}' uploaded successfully.", "File Uploaded", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
    }
}
