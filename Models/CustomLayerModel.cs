using DekelApp.Utils;
using System.Text.Json.Serialization;

namespace DekelApp.Models
{
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
}
