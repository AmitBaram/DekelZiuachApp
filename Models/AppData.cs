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


    public class CustomLayerModel : ObservableObject
    {
        private string _name = string.Empty;
        private bool _isSelected = true;

        [JsonPropertyName("Name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        [JsonPropertyName("IsSelected")]
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }

    public class FormatModel : ObservableObject
    {
        private bool _cdb;
        private bool _mft;
        private bool _txp;

        public bool CDB 
        { 
            get => _cdb; 
            set => SetProperty(ref _cdb, value); 
        }
        public bool MFT 
        { 
            get => _mft; 
            set => SetProperty(ref _mft, value); 
        }
        public bool TXP 
        { 
            get => _txp; 
            set => SetProperty(ref _txp, value); 
        }
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

        private bool _isDuplicate;
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

    public class TichumAreaModel : ObservableObject
    {
        private bool _isFileUploaded;
        private string? _filePath;
        private string _name = string.Empty;
        private CoordinateSystemType _coordinateSystem = CoordinateSystemType.UTM;
        private bool _isDuplicateName;

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    OnPropertyChanged(nameof(NameStatusText));
                    OnPropertyChanged(nameof(NameStatusVisibility));
                }
            }
        }

        [JsonIgnore]
        public bool IsDuplicateName
        {
            get => _isDuplicateName;
            set
            {
                if (SetProperty(ref _isDuplicateName, value))
                {
                    OnPropertyChanged(nameof(NameStatusText));
                    OnPropertyChanged(nameof(NameStatusVisibility));
                }
            }
        }

        [JsonIgnore]
        public string NameStatusText => string.IsNullOrWhiteSpace(Name) ? "fill tichum name" : (IsDuplicateName ? "Tichum name can't be the same" : string.Empty);
        [JsonIgnore]
        public System.Windows.Visibility NameStatusVisibility => (string.IsNullOrWhiteSpace(Name) || IsDuplicateName) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        public ObservableCollection<CoordinateModel> Coordinates { get; set; } = new();
        [JsonPropertyName("CustomLayers")]
        public ObservableCollection<CustomLayerModel> CustomLayers { get; set; } = new();

        public CoordinateSystemType CoordinateSystem
        {
            get => _coordinateSystem;
            set => SetProperty(ref _coordinateSystem, value);
        }

        [JsonIgnore]
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
        private bool _isDuplicateName;

        public string? Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    OnPropertyChanged(nameof(NameStatusText));
                    OnPropertyChanged(nameof(NameStatusVisibility));
                }
            }
        }

        [JsonIgnore]
        public bool IsDuplicateName
        {
            get => _isDuplicateName;
            set
            {
                if (SetProperty(ref _isDuplicateName, value))
                {
                    OnPropertyChanged(nameof(NameStatusText));
                    OnPropertyChanged(nameof(NameStatusVisibility));
                }
            }
        }

        [JsonIgnore]
        public string NameStatusText => string.IsNullOrWhiteSpace(Name) ? "fill target name" : (IsDuplicateName ? "Target name can't be the same" : string.Empty);
        [JsonIgnore]
        public System.Windows.Visibility NameStatusVisibility => (string.IsNullOrWhiteSpace(Name) || IsDuplicateName) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

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

        private bool _isDuplicate;
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
        [JsonPropertyName("Tichum")]
        public ObservableCollection<TichumAreaModel> TichumAreas { get; set; } = new();
        [JsonPropertyName("Yeadim")]
        public ObservableCollection<YeadimTargetModel> YeadimTargets { get; set; } = new();

        public CoordinateSystemType YeadimCoordinateSystem
        {
            get => _yeadimCoordinateSystem;
            set => SetProperty(ref _yeadimCoordinateSystem, value);
        }
    }
}
