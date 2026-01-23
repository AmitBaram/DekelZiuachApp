using DekelApp.Models;
using DekelApp.Utils;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DekelApp.ViewModels
{
    public class MikudViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        public ObservableCollection<CoordinateModel> Coordinates => _appData.MikudCoordinates;

        public ICommand AddCoordinateCommand { get; }
        public ICommand DeleteCoordinateCommand { get; }
        public ICommand UploadFileCommand { get; }

        public MikudViewModel(AppData appData)
        {
            _appData = appData;
            AddCoordinateCommand = new RelayCommand(_ => AddCoordinate());
            DeleteCoordinateCommand = new RelayCommand(coord => DeleteCoordinate(coord));
            UploadFileCommand = new RelayCommand(_ => UploadFile());
        }

        private void AddCoordinate()
        {
            Coordinates.Add(new CoordinateModel() { Easting = "0", Northing = "0", Zone = "36N" });
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
                Title = "Upload Mikud File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _appData.IsMikudFileUploaded = true;
                _appData.MikudFilePath = openFileDialog.FileName;
                System.Windows.MessageBox.Show($"File '{openFileDialog.SafeFileName}' uploaded successfully. (Parsing logic not implemented in this demo)", "File Uploaded", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
    }
}
