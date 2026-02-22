using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DekelApp.Models
{
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

        [JsonInclude]
        public ObservableCollection<CoordinateModel> Coordinates { get; set; } = new();
        [JsonPropertyName("CustomLayers")]
        [JsonInclude]
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
}
