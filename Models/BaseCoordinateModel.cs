using DekelApp.Utils;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace DekelApp.Models
{
    public abstract class BaseCoordinateModel : ObservableObject
    {
        private string? _easting;
        private string? _northing;
        private string? _zone;
        private string? _latitude;
        private string? _longitude;
        private bool _isDuplicate;

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

        public string? Latitude
        {
            get => _latitude;
            set
            {
                if (SetProperty(ref _latitude, value))
                {
                    OnPropertyChanged(nameof(IsLatitudeValid));
                    OnPropertyChanged(nameof(LatitudeStatusText));
                    OnPropertyChanged(nameof(LatitudeStatusColor));
                }
            }
        }

        public string? Longitude
        {
            get => _longitude;
            set
            {
                if (SetProperty(ref _longitude, value))
                {
                    OnPropertyChanged(nameof(IsLongitudeValid));
                    OnPropertyChanged(nameof(LongitudeStatusText));
                    OnPropertyChanged(nameof(LongitudeStatusColor));
                }
            }
        }

        [JsonIgnore]
        public bool IsEastingValid => !string.IsNullOrWhiteSpace(Easting) && double.TryParse(Easting, out double e) && e >= 100000 && e <= 900000;
        [JsonIgnore]
        public bool IsNorthingValid => !string.IsNullOrWhiteSpace(Northing) && double.TryParse(Northing, out double n) && n >= 1000000 && n <= 10000000;
        [JsonIgnore]
        public bool IsZoneValid => !string.IsNullOrWhiteSpace(Zone) && System.Text.RegularExpressions.Regex.IsMatch(Zone, @"^\d{1,2}[A-Z]$");

        [JsonIgnore]
        public bool IsLatitudeValid => !string.IsNullOrWhiteSpace(Latitude) && double.TryParse(Latitude, out double lat) && lat >= -90 && lat <= 90;
        [JsonIgnore]
        public bool IsLongitudeValid => !string.IsNullOrWhiteSpace(Longitude) && double.TryParse(Longitude, out double lon) && lon >= -180 && lon <= 180;

        [JsonIgnore]
        public string EastingStatusText => IsEastingValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string NorthingStatusText => IsNorthingValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string ZoneStatusText => IsZoneValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string LatitudeStatusText => IsLatitudeValid ? "Coordinate is valid" : "Coordinate is not valid";
        [JsonIgnore]
        public string LongitudeStatusText => IsLongitudeValid ? "Coordinate is valid" : "Coordinate is not valid";

        [JsonIgnore]
        public Brush EastingStatusColor => IsEastingValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush NorthingStatusColor => IsNorthingValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush ZoneStatusColor => IsZoneValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush LatitudeStatusColor => IsLatitudeValid ? Brushes.Blue : Brushes.Red;
        [JsonIgnore]
        public Brush LongitudeStatusColor => IsLongitudeValid ? Brushes.Blue : Brushes.Red;

        [JsonIgnore]
        public bool IsDuplicate
        {
            get => _isDuplicate;
            set
            {
                if (SetProperty(ref _isDuplicate, value))
                {
                    OnPropertyChanged(nameof(DuplicateStatusText));
                    OnPropertyChanged(nameof(DuplicateStatusVisibility));
                }
            }
        }

        [JsonIgnore]
        public string DuplicateStatusText => "Identical coordinate";
        [JsonIgnore]
        public System.Windows.Visibility DuplicateStatusVisibility => IsDuplicate ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    }
}
