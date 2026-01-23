using DekelApp.Models;
using System;

namespace DekelApp.ViewModels
{
    public class GeneralInfoViewModel : BaseViewModel
    {
        private readonly GeneralInfoModel _info;

        public string? UnitName
        {
            get => _info.UnitName;
            set { if (_info.UnitName != value) { _info.UnitName = value; OnPropertyChanged(); } }
        }

        public string? OperationName
        {
            get => _info.OperationName;
            set { if (_info.OperationName != value) { _info.OperationName = value; OnPropertyChanged(); } }
        }

        public DateTime? Date
        {
            get => _info.Date;
            set { if (_info.Date != value) { _info.Date = value; OnPropertyChanged(); } }
        }

        public string? GeneralNotes
        {
            get => _info.GeneralNotes;
            set { if (_info.GeneralNotes != value) { _info.GeneralNotes = value; OnPropertyChanged(); } }
        }

        public GeneralInfoViewModel(GeneralInfoModel info)
        {
            _info = info;
        }
    }
}
