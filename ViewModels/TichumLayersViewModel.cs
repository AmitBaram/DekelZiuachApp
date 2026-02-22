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
        private readonly Services.IMessageService _messageService;
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
                    OnPropertyChanged(nameof(CustomLayers));
                }
            }
        }

        public bool HasSelection => SelectedArea != null;
        public ObservableCollection<CustomLayerModel>? CustomLayers => SelectedArea?.CustomLayers;

        public string NewLayerName
        {
            get => _newLayerName;
            set => SetProperty(ref _newLayerName, value);
        }

        public ICommand AddCustomLayerCommand { get; }
        public ICommand RemoveCustomLayerCommand { get; }


        public TichumLayersViewModel(AppData appData, Services.IMessageService messageService)
        {
            _appData = appData;
            _messageService = messageService;
            AddCustomLayerCommand = new RelayCommand(_ => AddCustomLayer());
            RemoveCustomLayerCommand = new RelayCommand(layer => RemoveCustomLayer(layer));
            
            // Removed auto-select first area logic to preserve navigation state (Bug Fix)
            // if (TichumAreas.Any()) { SelectedArea = TichumAreas.First(); }
        }


        private void AddCustomLayer()
        {
            if (SelectedArea == null || string.IsNullOrWhiteSpace(NewLayerName)) return;

            // Check if it already exists
            if (SelectedArea.CustomLayers.Any(l => l.Name.Equals(NewLayerName.Trim(), System.StringComparison.OrdinalIgnoreCase)))
            {
                _messageService.ShowWarning("A custom layer with this name already exists.", "Duplicate Layer");
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
