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

        public bool TXP
        {
            get => _formats.TXP;
            set { if (_formats.TXP != value) { _formats.TXP = value; OnPropertyChanged(); } }
        }

        
        public FormatsViewModel(FormatModel formats)
        {
            _formats = formats;
        }
    }
}
