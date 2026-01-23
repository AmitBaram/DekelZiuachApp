using DekelApp.Models;

namespace DekelApp.ViewModels
{
    public class FormatsViewModel : BaseViewModel
    {
        private readonly FormatModel _formats;

        public bool CDB
        {
            get => _formats.CDB;
            set { if (_formats.CDB != value) { _formats.CDB = value; OnPropertyChanged(); } }
        }

        public bool MFT
        {
            get => _formats.MFT;
            set { if (_formats.MFT != value) { _formats.MFT = value; OnPropertyChanged(); } }
        }

        public bool VBS3
        {
            get => _formats.VBS3;
            set { if (_formats.VBS3 != value) { _formats.VBS3 = value; OnPropertyChanged(); } }
        }

        public bool VBS4
        {
            get => _formats.VBS4;
            set { if (_formats.VBS4 != value) { _formats.VBS4 = value; OnPropertyChanged(); } }
        }

        public FormatsViewModel(FormatModel formats)
        {
            _formats = formats;
        }
    }
}
