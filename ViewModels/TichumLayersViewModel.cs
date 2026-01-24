using DekelApp.Models;

namespace DekelApp.ViewModels
{
    public class TichumLayersViewModel : BaseViewModel
    {
        private readonly LayerModel _layers;

        public bool Orthophoto
        {
            get => _layers.Orthophoto;
            set { if (_layers.Orthophoto != value) { _layers.Orthophoto = value; OnPropertyChanged(); } }
        }

        public bool DTM
        {
            get => _layers.DTM;
            set { if (_layers.DTM != value) { _layers.DTM = value; OnPropertyChanged(); } }
        }

        public bool Buildings
        {
            get => _layers.Buildings;
            set { if (_layers.Buildings != value) { _layers.Buildings = value; OnPropertyChanged(); } }
        }

        public bool Fences
        {
            get => _layers.Fences;
            set { if (_layers.Fences != value) { _layers.Fences = value; OnPropertyChanged(); } }
        }

        public bool Roads
        {
            get => _layers.Roads;
            set { if (_layers.Roads != value) { _layers.Roads = value; OnPropertyChanged(); } }
        }

        public bool Vegetation
        {
            get => _layers.Vegetation;
            set { if (_layers.Vegetation != value) { _layers.Vegetation = value; OnPropertyChanged(); } }
        }

        public bool PowerLines
        {
            get => _layers.PowerLines;
            set { if (_layers.PowerLines != value) { _layers.PowerLines = value; OnPropertyChanged(); } }
        }

        public TichumLayersViewModel(LayerModel layers)
        {
            _layers = layers;
        }
    }
}
