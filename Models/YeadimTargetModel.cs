using System.Text.Json.Serialization;

namespace DekelApp.Models
{
    public class YeadimTargetModel : BaseCoordinateModel
    {
        private string? _name;
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
        public string NameStatusText => string.IsNullOrWhiteSpace(Name) ? "fill target name" : (IsDuplicateName ? "identical names" : string.Empty);
        [JsonIgnore]
        public System.Windows.Visibility NameStatusVisibility => (string.IsNullOrWhiteSpace(Name) || IsDuplicateName) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    }
}
