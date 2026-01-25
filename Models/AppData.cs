using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Text.Json.Serialization;
using DekelApp.Utils;

namespace DekelApp.Models
{
    public enum CoordinateSystemType
    {
        UTM,
        Geographic
    }

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
        private string? _latitude;
        private string? _longitude;

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
    }

    public class TichumModel : ObservableObject
    {
        private string? _filePath;
        private bool _isFileUploaded;
        private CoordinateSystemType _coordinateSystem = CoordinateSystemType.UTM;

        public LayerModel Layers { get; set; } = new();
        public ObservableCollection<CoordinateModel> Coordinates { get; set; } = new();

        public CoordinateSystemType CoordinateSystem
        {
            get => _coordinateSystem;
            set => SetProperty(ref _coordinateSystem, value);
        }

        public string? FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        [JsonIgnore]
        public bool IsFileUploaded
        {
            get => _isFileUploaded;
            set => SetProperty(ref _isFileUploaded, value);
        }
    }

    public class MikudAreaModel : ObservableObject
    {
        private bool _isFileUploaded;
        private string? _filePath;
        private string _name = string.Empty;
        private CoordinateSystemType _coordinateSystem = CoordinateSystemType.UTM;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<CoordinateModel> Coordinates { get; set; } = new();
        public LayerModel Layers { get; set; } = new();

        public CoordinateSystemType CoordinateSystem
        {
            get => _coordinateSystem;
            set => SetProperty(ref _coordinateSystem, value);
        }

        public string? FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        [JsonIgnore]
        public bool IsFileUploaded
        {
            get => _isFileUploaded;
            set => SetProperty(ref _isFileUploaded, value);
        }
    }

    public class YeadimTargetModel : ObservableObject
    {
        private string? _name;
        private string? _easting;
        private string? _northing;
        private string? _zone;
        private string? _latitude;
        private string? _longitude;

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
    }

    public class GeneralInfoModel
    {
        public string? UnitName { get; set; }
        public string? OperationName { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string? GeneralNotes { get; set; }
    }

    public class AppData : ObservableObject
    {
        private CoordinateSystemType _yeadimCoordinateSystem = CoordinateSystemType.UTM;

        public GeneralInfoModel GeneralInfo { get; set; } = new();
        public FormatModel Formats { get; set; } = new();
        public TichumModel Tichum { get; set; } = new();
        [JsonPropertyName("Mikud")]
        public ObservableCollection<MikudAreaModel> MikudAreas { get; set; } = new();
        [JsonPropertyName("Yeadim")]
        public ObservableCollection<YeadimTargetModel> YeadimTargets { get; set; } = new();

        public CoordinateSystemType YeadimCoordinateSystem
        {
            get => _yeadimCoordinateSystem;
            set => SetProperty(ref _yeadimCoordinateSystem, value);
        }
    }
}
