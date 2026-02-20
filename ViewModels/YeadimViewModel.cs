using DekelApp.Models;
using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
                    UpdateDuplicates();
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

            Targets.CollectionChanged += Targets_CollectionChanged;
            foreach (var t in Targets) t.PropertyChanged += Target_PropertyChanged;
        }

        private void Targets_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (YeadimTargetModel t in e.OldItems) t.PropertyChanged -= Target_PropertyChanged;
            if (e.NewItems != null)
                foreach (YeadimTargetModel t in e.NewItems) t.PropertyChanged += Target_PropertyChanged;

            UpdateDuplicates();
        }

        private void Target_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(YeadimTargetModel.Name))
            {
                UpdateNameDuplicates();
            }
            else if (e.PropertyName == nameof(YeadimTargetModel.Easting) || e.PropertyName == nameof(YeadimTargetModel.Northing) ||
                e.PropertyName == nameof(YeadimTargetModel.Zone) || e.PropertyName == nameof(YeadimTargetModel.Latitude) ||
                e.PropertyName == nameof(YeadimTargetModel.Longitude))
            {
                UpdateDuplicates();
            }
        }

        private void UpdateNameDuplicates()
        {
            foreach (var t in Targets) t.IsDuplicateName = false;

            var groups = Targets
                .Where(t => !string.IsNullOrWhiteSpace(t.Name))
                .GroupBy(t => t.Name!.Trim().ToLower())
                .Where(g => g.Count() > 1);

            foreach (var g in groups)
                foreach (var t in g) t.IsDuplicateName = true;
        }

        private void UpdateDuplicates()
        {
            foreach (var t in Targets) t.IsDuplicate = false;

            if (IsUTM)
            {
                var groups = Targets
                    .Where(t => t.IsEastingValid && t.IsNorthingValid && t.IsZoneValid)
                    .GroupBy(t => $"{t.Easting?.Trim()}_{t.Northing?.Trim()}_{t.Zone?.Trim()}")
                    .Where(g => g.Count() > 1);
                foreach (var g in groups)
                    foreach (var t in g) t.IsDuplicate = true;
            }
            else
            {
                var groups = Targets
                    .Where(t => t.IsLatitudeValid && t.IsLongitudeValid)
                    .GroupBy(t => $"{t.Latitude?.Trim()}_{t.Longitude?.Trim()}")
                    .Where(g => g.Count() > 1);
                foreach (var g in groups)
                    foreach (var t in g) t.IsDuplicate = true;
            }
        }

        private void AddTarget()
        {
            if (CoordinateSystem == CoordinateSystemType.UTM)
            {
                Targets.Add(new YeadimTargetModel() { Name = string.Empty, Easting = "0", Northing = "0", Zone = "36N" });
            }
            else
            {
                Targets.Add(new YeadimTargetModel() { Name = string.Empty, Latitude = "0", Longitude = "0" });
            }
        }

        private void DeleteTarget(object? target)
        {
            if (target is YeadimTargetModel model)
            {
                model.PropertyChanged -= Target_PropertyChanged;
                Targets.Remove(model);
            }
        }
    }
}
