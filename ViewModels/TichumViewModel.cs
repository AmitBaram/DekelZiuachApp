using DekelApp.Models;
using DekelApp.Utils;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DekelApp.ViewModels
{
    public class TichumViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        public ObservableCollection<CoordinateModel> Coordinates => _appData.Tichum.Coordinates;

        public CoordinateSystemType CoordinateSystem
        {
            get => _appData.Tichum.CoordinateSystem;
            set
            {
                if (_appData.Tichum.CoordinateSystem != value)
                {
                    _appData.Tichum.CoordinateSystem = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsUTM));
                    OnPropertyChanged(nameof(IsGeographic));
                }
            }
        }

        public bool IsUTM => CoordinateSystem == CoordinateSystemType.UTM;
        public bool IsGeographic => CoordinateSystem == CoordinateSystemType.Geographic;

        public string? UploadedFileName => _appData.Tichum.IsFileUploaded && !string.IsNullOrEmpty(_appData.Tichum.FilePath) 
            ? System.IO.Path.GetFileName(_appData.Tichum.FilePath) 
            : null;

        public bool HasUploadedFile => _appData.Tichum.IsFileUploaded;

        public ICommand AddCoordinateCommand { get; }
        public ICommand DeleteCoordinateCommand { get; }
        public ICommand UploadFileCommand { get; }
        public ICommand ToggleToUTMCommand { get; }
        public ICommand ToggleToGeographicCommand { get; }

        public TichumViewModel(AppData appData)
        {
            _appData = appData;
            AddCoordinateCommand = new RelayCommand(_ => AddCoordinate());
            DeleteCoordinateCommand = new RelayCommand(coord => DeleteCoordinate(coord));
            UploadFileCommand = new RelayCommand(_ => UploadFile());
            ToggleToUTMCommand = new RelayCommand(_ => CoordinateSystem = CoordinateSystemType.UTM);
            ToggleToGeographicCommand = new RelayCommand(_ => CoordinateSystem = CoordinateSystemType.Geographic);
        }

        private void AddCoordinate()
        {
            if (CoordinateSystem == CoordinateSystemType.UTM)
            {
                Coordinates.Add(new CoordinateModel() { Easting = "0", Northing = "0", Zone = "36N" });
            }
            else
            {
                Coordinates.Add(new CoordinateModel() { Latitude = "0", Longitude = "0" });
            }
        }

        private void DeleteCoordinate(object? coord)
        {
            if (coord is CoordinateModel model)
            {
                Coordinates.Remove(model);
            }
        }

        private void UploadFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Shapefiles (*.shp)|*.shp",
                Title = "Upload Tichum File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _appData.Tichum.IsFileUploaded = true;
                _appData.Tichum.FilePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(UploadedFileName));
                OnPropertyChanged(nameof(HasUploadedFile));
                System.Windows.MessageBox.Show($"File '{openFileDialog.SafeFileName}' uploaded successfully.", "File Uploaded", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
    }
}
