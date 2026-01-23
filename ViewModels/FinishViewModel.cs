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
            var result = MessageBox.Show("Are you sure you want to finish?", "Confirm Finish", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (_validationService.ValidateAppData(_appData, out string errorMessage))
                {
                    try
                    {
                        var rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Dekel");
                        var exportPath = Path.Combine(rootPath, "Send_This_Folder");
                        var tichumExportPath = Path.Combine(exportPath, "Tichum");
                        var mikudExportPath = Path.Combine(exportPath, "Mikud");
                        
                        var jsonFilePath = Path.Combine(exportPath, "data.json");

                        // Save JSON
                        await _fileService.SaveAsync(_appData, jsonFilePath);

                        // Copy uploaded files to respective subfolders
                        if (_appData.IsTichumFileUploaded && !string.IsNullOrEmpty(_appData.TichumFilePath))
                        {
                            await _fileService.CopyShapefileAsync(_appData.TichumFilePath, tichumExportPath);
                        }
                        if (_appData.IsMikudFileUploaded && !string.IsNullOrEmpty(_appData.MikudFilePath))
                        {
                            await _fileService.CopyShapefileAsync(_appData.MikudFilePath, mikudExportPath);
                        }

                        MessageBox.Show($"Data saved successfully to {exportPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Open the folder in File Explorer
                        Process.Start("explorer.exe", rootPath);

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
}
