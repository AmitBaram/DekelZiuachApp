using DekelApp.Models;
using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace DekelApp.ViewModels
{
    public class TichumLayersViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        private TichumAreaModel? _selectedArea;
        private string _newLayerName = string.Empty;

        public ObservableCollection<TichumAreaModel> TichumAreas => _appData.TichumAreas;

        public TichumAreaModel? SelectedArea
        {
            get => _selectedArea;
            set
            {
                if (SetProperty(ref _selectedArea, value))
                {
                    OnPropertyChanged(nameof(HasSelection));
                }
            }
        }

        public bool HasSelection => SelectedArea != null;

        public string NewLayerName
        {
            get => _newLayerName;
            set => SetProperty(ref _newLayerName, value);
        }

        public ICommand ApplyToAllCommand { get; }
        public ICommand AddCustomLayerCommand { get; }
        public ICommand RemoveCustomLayerCommand { get; }


        public TichumLayersViewModel(AppData appData)
        {
            _appData = appData;
            ApplyToAllCommand = new RelayCommand(_ => ApplyToAll());
            AddCustomLayerCommand = new RelayCommand(_ => AddCustomLayer());
            RemoveCustomLayerCommand = new RelayCommand(layer => RemoveCustomLayer(layer));
            
            // Auto-select first area if available
            if (TichumAreas.Any())
            {
                SelectedArea = TichumAreas.First();
            }
        }

        private void ApplyToAll()
        {
            if (SelectedArea == null) return;

            foreach (var area in TichumAreas)
            {
                if (area == SelectedArea) continue;

                // Sync custom layers
                area.CustomLayers.Clear();
                foreach (var customLayer in SelectedArea.CustomLayers)
                {
                    area.CustomLayers.Add(new CustomLayerModel 
                    { 
                        Name = customLayer.Name, 
                        IsSelected = customLayer.IsSelected 
                    });
                }
            }

            System.Windows.MessageBox.Show("Settings applied to all Tichum areas.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void AddCustomLayer()
        {
            if (SelectedArea == null || string.IsNullOrWhiteSpace(NewLayerName)) return;

            // Check if it already exists
            if (SelectedArea.CustomLayers.Any(l => l.Name.Equals(NewLayerName.Trim(), System.StringComparison.OrdinalIgnoreCase)))
            {
                System.Windows.MessageBox.Show("A custom layer with this name already exists.", "Duplicate Layer", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            SelectedArea.CustomLayers.Add(new CustomLayerModel { Name = NewLayerName.Trim(), IsSelected = true });
            NewLayerName = string.Empty;
        }

        private void RemoveCustomLayer(object? parameter)
        {
            if (SelectedArea != null && parameter is CustomLayerModel layer)
            {
                SelectedArea.CustomLayers.Remove(layer);
            }
        }
    }
}
