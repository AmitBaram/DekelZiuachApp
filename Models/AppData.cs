using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Text.Json.Serialization;
using DekelApp.Utils;

namespace DekelApp.Models
{
    public class LayerModel
    {
        public bool Orthophoto { get; set; } = true;
        public bool DTM { get; set; } = true;
        public bool Buildings { get; set; }
        public bool Fences { get; set; }
        public bool Roads { get; set; }
        public bool Vegetation { get; set; }
        public bool PowerLines { get; set; }
    }

    public class FormatModel
    {
        public bool CDB { get; set; }
        public bool MFT { get; set; }
        public bool VBS3 { get; set; }
        public bool VBS4 { get; set; }
    }

    public class CoordinateModel : ObservableObject
    {
        private string? _easting;
        private string? _northing;
        private string? _zone;

        public string? Easting
        {
            get => _easting;
            set
            {
                if (SetProperty(ref _easting, value))
                {
                    OnPropertyChanged(nameof(IsEastingValid));
                    OnPropertyChanged(nameof(EastingStatusText));
                    OnPropertyChanged(nameof(EastingStatusColor));
                }
            }
        }

        public string? Northing
        {
            get => _northing;
            set
            {
                if (SetProperty(ref _northing, value))
                {
                    OnPropertyChanged(nameof(IsNorthingValid));
                    OnPropertyChanged(nameof(NorthingStatusText));
                    OnPropertyChanged(nameof(NorthingStatusColor));
                }
            }
        }

        public string? Zone
        {
            get => _zone;
            set
            {
                if (SetProperty(ref _zone, value))
                {
                    OnPropertyChanged(nameof(IsZoneValid));
                    OnPropertyChanged(nameof(ZoneStatusText));
                    OnPropertyChanged(nameof(ZoneStatusColor));
                }
            }
        }

        // Validation Logic
        [JsonIgnore]
        public bool IsEastingValid => !string.IsNullOrWhiteSpace(Easting) && double.TryParse(Easting, out double e) && e >= 100000 && e <= 900000;
        [JsonIgnore]
        public bool IsNorthingValid => !string.IsNullOrWhiteSpace(Northing) && double.TryParse(Northing, out double n) && n >= 1000000 && n <= 10000000;
        [JsonIgnore]
        public bool IsZoneValid => !string.IsNullOrWhiteSpace(Zone) && System.Text.RegularExpressions.Regex.IsMatch(Zone, @"^\d{1,2}[A-Z]$");

        [JsonIgnore]
        public string EastingStatusText => IsEastingValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string NorthingStatusText => IsNorthingValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string ZoneStatusText => IsZoneValid ? "Coordinate is valid" : "Coordinate is not valid";

        [JsonIgnore]
        public Brush EastingStatusColor => IsEastingValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush NorthingStatusColor => IsNorthingValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush ZoneStatusColor => IsZoneValid ? Brushes.Blue : Brushes.Red;
    }

    public class YeadimTargetModel : ObservableObject
    {
        private string? _name;
        private string? _easting;
        private string? _northing;
        private string? _zone;

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Easting
        {
            get => _easting;
            set
            {
                if (SetProperty(ref _easting, value))
                {
                    OnPropertyChanged(nameof(IsEastingValid));
                    OnPropertyChanged(nameof(EastingStatusText));
                    OnPropertyChanged(nameof(EastingStatusColor));
                }
            }
        }

        public string? Northing
        {
            get => _northing;
            set
            {
                if (SetProperty(ref _northing, value))
                {
                    OnPropertyChanged(nameof(IsNorthingValid));
                    OnPropertyChanged(nameof(NorthingStatusText));
                    OnPropertyChanged(nameof(NorthingStatusColor));
                }
            }
        }

        public string? Zone
        {
            get => _zone;
            set
            {
                if (SetProperty(ref _zone, value))
                {
                    OnPropertyChanged(nameof(IsZoneValid));
                    OnPropertyChanged(nameof(ZoneStatusText));
                    OnPropertyChanged(nameof(ZoneStatusColor));
                }
            }
        }

        // Validation Logic
        [JsonIgnore]
        public bool IsEastingValid => !string.IsNullOrWhiteSpace(Easting) && double.TryParse(Easting, out double e) && e >= 100000 && e <= 900000;
        [JsonIgnore]
        public bool IsNorthingValid => !string.IsNullOrWhiteSpace(Northing) && double.TryParse(Northing, out double n) && n >= 1000000 && n <= 10000000;
        [JsonIgnore]
        public bool IsZoneValid => !string.IsNullOrWhiteSpace(Zone) && System.Text.RegularExpressions.Regex.IsMatch(Zone, @"^\d{1,2}[A-Z]$");

        [JsonIgnore]
        public string EastingStatusText => IsEastingValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string NorthingStatusText => IsNorthingValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string ZoneStatusText => IsZoneValid ? "Coordinate is valid" : "Coordinate is not valid";

        [JsonIgnore]
        public Brush EastingStatusColor => IsEastingValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush NorthingStatusColor => IsNorthingValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush ZoneStatusColor => IsZoneValid ? Brushes.Blue : Brushes.Red;
    }

    public class GeneralInfoModel
    {
        public string? UnitName { get; set; }
        public string? OperationName { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string? GeneralNotes { get; set; }
    }

    public class AppData
    {
        public LayerModel Layers { get; set; } = new();
        public FormatModel Formats { get; set; } = new();
        public ObservableCollection<CoordinateModel> TichumCoordinates { get; set; } = new();
        public ObservableCollection<CoordinateModel> MikudCoordinates { get; set; } = new();
        public bool IsTichumFileUploaded { get; set; }
        public bool IsMikudFileUploaded { get; set; }
        public string? TichumFilePath { get; set; }
        public string? MikudFilePath { get; set; }
        public ObservableCollection<YeadimTargetModel> YeadimTargets { get; set; } = new();
        public GeneralInfoModel GeneralInfo { get; set; } = new();
    }
}
