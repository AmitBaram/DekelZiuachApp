using DekelApp.Models;
using DekelApp.Services;
using DekelApp.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace DekelApp.ViewModels
{
    public class FinishViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        private readonly IValidationService _validationService;
        private readonly IFileService _fileService;

        public ICommand FinishCommand { get; }

        public FinishViewModel(AppData appData, IValidationService validationService, IFileService fileService)
        {
            _appData = appData;
            _validationService = validationService;
            _fileService = fileService;
            FinishCommand = new RelayCommand(async _ => await FinishAsync());
        }

        private async Task FinishAsync()
        {
            if (_validationService.ValidateAppData(_appData, out string errorMessage))
            {
                try
                {
                    var rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Dekel");
                    var exportPath = Path.Combine(rootPath, "Send_This_Folder");
                    var tichumExportPath = Path.Combine(exportPath, "Tichum");
                    var mikudExportPath = Path.Combine(exportPath, "Mikud");
                    
                    var jsonFilePath = Path.Combine(exportPath, "send this file.json");

                    // Sanitize coordinates: clear unused fields based on coordinate system
                    foreach (var area in _appData.TichumAreas)
                    {
                        foreach (var coord in area.Coordinates)
                        {
                            if (area.CoordinateSystem == DekelApp.Models.CoordinateSystemType.UTM)
                            {
                                coord.Latitude = null;
                                coord.Longitude = null;
                            }
                            else
                            {
                                coord.Easting = null;
                                coord.Northing = null;
                                coord.Zone = null;
                            }
                        }
                    }

                    foreach (var target in _appData.YeadimTargets)
                    {
                        if (_appData.YeadimCoordinateSystem == DekelApp.Models.CoordinateSystemType.UTM)
                        {
                            target.Latitude = null;
                            target.Longitude = null;
                        }
                        else
                        {
                            target.Easting = null;
                            target.Northing = null;
                            target.Zone = null;
                        }
                    }

                    // Save JSON
                    await _fileService.SaveAsync(_appData, jsonFilePath);

                    // Copy Tichum files for each area
                    // The user requested: "I will not save the file it will extract the cordinants in GEO and write them to the json"
                    // Since we extracted them directly into the ObservableCollection when uploaded,
                    // _appData serialization in SaveAsync takes care of them automatically.
                    // We remove the old shapefile copy logic here.

                    MessageBox.Show($"Data saved successfully to {exportPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Open the folder in File Explorer
                    try
                    {
                        Process.Start(new ProcessStartInfo(exportPath)
                        {
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to open explorer: {ex.Message}");
                    }

                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
