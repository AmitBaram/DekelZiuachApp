using DekelApp.Models;
using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DekelApp.ViewModels
{
    public class YeadimViewModel : BaseViewModel
    {
        private readonly AppData _appData;
        public ObservableCollection<YeadimTargetModel> Targets { get; }

        public CoordinateSystemType CoordinateSystem
        {
            get => _appData.YeadimCoordinateSystem;
            set
            {
                if (_appData.YeadimCoordinateSystem != value)
                {
                    _appData.YeadimCoordinateSystem = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsUTM));
                    OnPropertyChanged(nameof(IsGeographic));
                }
            }
        }

        public bool IsUTM => CoordinateSystem == CoordinateSystemType.UTM;
        public bool IsGeographic => CoordinateSystem == CoordinateSystemType.Geographic;

        public ICommand AddTargetCommand { get; }
        public ICommand DeleteTargetCommand { get; }
        public ICommand ToggleToUTMCommand { get; }
        public ICommand ToggleToGeographicCommand { get; }

        public YeadimViewModel(ObservableCollection<YeadimTargetModel> targets, AppData appData)
        {
            Targets = targets;
            _appData = appData;
            AddTargetCommand = new RelayCommand(_ => AddTarget());
            DeleteTargetCommand = new RelayCommand(target => DeleteTarget(target));
            ToggleToUTMCommand = new RelayCommand(_ => CoordinateSystem = CoordinateSystemType.UTM);
            ToggleToGeographicCommand = new RelayCommand(_ => CoordinateSystem = CoordinateSystemType.Geographic);
        }

        private void AddTarget()
        {
            if (CoordinateSystem == CoordinateSystemType.UTM)
            {
                Targets.Add(new YeadimTargetModel() { Name = "New Target", Easting = "0", Northing = "0", Zone = "36N" });
            }
            else
            {
                Targets.Add(new YeadimTargetModel() { Name = "New Target", Latitude = "0", Longitude = "0" });
            }
        }

        private void DeleteTarget(object? target)
        {
            if (target is YeadimTargetModel model)
            {
                Targets.Remove(model);
            }
        }
    }
}
