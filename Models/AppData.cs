using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DekelApp.Models
{
    public class AppData : ObservableObject
    {
        private CoordinateSystemType _yeadimCoordinateSystem = CoordinateSystemType.UTM;
        private TichumAreaModel? _selectedTichumArea;

        public GeneralInfoModel GeneralInfo { get; set; } = new();
        public FormatModel Formats { get; set; } = new();

        [JsonPropertyName("Tichum")]
        public ObservableCollection<TichumAreaModel> TichumAreas { get; set; } = new();

        [JsonPropertyName("Yeadim")]
        public ObservableCollection<YeadimTargetModel> YeadimTargets { get; set; } = new();

        [JsonPropertyName("GeoCells")]
        public List<string> GeoCells { get; set; } = new();

        public CoordinateSystemType YeadimCoordinateSystem
        {
            get => _yeadimCoordinateSystem;
            set => SetProperty(ref _yeadimCoordinateSystem, value);
        }

        [JsonIgnore]
        public TichumAreaModel? SelectedTichumArea
        {
            get => _selectedTichumArea;
            set => SetProperty(ref _selectedTichumArea, value);
        }
    }
}
