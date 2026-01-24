using DekelApp.Models;
using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace DekelApp.ViewModels
{
    public class MikudLayersViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        private MikudAreaModel? _selectedArea;

        public ObservableCollection<MikudAreaModel> MikudAreas => _appData.MikudAreas;

        public MikudAreaModel? SelectedArea
        {
            get => _selectedArea;
            set
            {
                if (SetProperty(ref _selectedArea, value))
                {
                    OnPropertyChanged(nameof(HasSelection));
                    UpdateLocalLayerProperties();
                }
            }
        }

        public bool HasSelection => SelectedArea != null;

        public ICommand ApplyToAllCommand { get; }

        public bool Orthophoto
        {
            get => SelectedArea?.Layers.Orthophoto ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.Orthophoto != value) { SelectedArea.Layers.Orthophoto = value; OnPropertyChanged(); } }
        }

        public bool DTM
        {
            get => SelectedArea?.Layers.DTM ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.DTM != value) { SelectedArea.Layers.DTM = value; OnPropertyChanged(); } }
        }

        public bool Buildings
        {
            get => SelectedArea?.Layers.Buildings ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.Buildings != value) { SelectedArea.Layers.Buildings = value; OnPropertyChanged(); } }
        }

        public bool Fences
        {
            get => SelectedArea?.Layers.Fences ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.Fences != value) { SelectedArea.Layers.Fences = value; OnPropertyChanged(); } }
        }

        public bool Roads
        {
            get => SelectedArea?.Layers.Roads ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.Roads != value) { SelectedArea.Layers.Roads = value; OnPropertyChanged(); } }
        }

        public bool Vegetation
        {
            get => SelectedArea?.Layers.Vegetation ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.Vegetation != value) { SelectedArea.Layers.Vegetation = value; OnPropertyChanged(); } }
        }

        public bool PowerLines
        {
            get => SelectedArea?.Layers.PowerLines ?? false;
            set { if (SelectedArea != null && SelectedArea.Layers.PowerLines != value) { SelectedArea.Layers.PowerLines = value; OnPropertyChanged(); } }
        }

        public MikudLayersViewModel(AppData appData)
        {
            _appData = appData;
            ApplyToAllCommand = new RelayCommand(_ => ApplyToAll());
            
            // Auto-select first area if available
            if (MikudAreas.Any())
            {
                SelectedArea = MikudAreas.First();
            }
        }

        private void UpdateLocalLayerProperties()
        {
            OnPropertyChanged(nameof(Orthophoto));
            OnPropertyChanged(nameof(DTM));
            OnPropertyChanged(nameof(Buildings));
            OnPropertyChanged(nameof(Fences));
            OnPropertyChanged(nameof(Roads));
            OnPropertyChanged(nameof(Vegetation));
            OnPropertyChanged(nameof(PowerLines));
        }

        private void ApplyToAll()
        {
            if (SelectedArea == null) return;

            var template = SelectedArea.Layers;
            foreach (var area in MikudAreas)
            {
                if (area == SelectedArea) continue;
                area.Layers.Orthophoto = template.Orthophoto;
                area.Layers.DTM = template.DTM;
                area.Layers.Buildings = template.Buildings;
                area.Layers.Fences = template.Fences;
                area.Layers.Roads = template.Roads;
                area.Layers.Vegetation = template.Vegetation;
                area.Layers.PowerLines = template.PowerLines;
            }

            System.Windows.MessageBox.Show("Settings applied to all Mikud areas.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
