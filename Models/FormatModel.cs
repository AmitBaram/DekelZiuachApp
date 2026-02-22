using DekelApp.Utils;

namespace DekelApp.Models
{
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
}
