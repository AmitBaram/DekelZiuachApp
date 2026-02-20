using DekelApp.Models;
using DekelApp.Services;
using DekelApp.Utils;
using System.Windows.Input;

namespace DekelApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly AppData _appData;

        public INavigationService Navigation => _navigationService;

        public ICommand NavigateTichumLayersCommand { get; }
        public ICommand NavigateFormatsCommand { get; }
        public ICommand NavigateTichumCommand { get; }
        public ICommand NavigateYeadimCommand { get; }
        public ICommand NavigateGeneralInfoCommand { get; }
        public ICommand NavigateFinishCommand { get; }

        public MainViewModel(INavigationService navigationService, AppData appData)
        {
            _navigationService = navigationService;
            _appData = appData;

            NavigateTichumLayersCommand = new RelayCommand(_ => _navigationService.NavigateTo<TichumLayersViewModel>());
            NavigateFormatsCommand = new RelayCommand(_ => _navigationService.NavigateTo<FormatsViewModel>());
            NavigateTichumCommand = new RelayCommand(_ => _navigationService.NavigateTo<TichumViewModel>());
            NavigateYeadimCommand = new RelayCommand(_ => _navigationService.NavigateTo<YeadimViewModel>());
            NavigateGeneralInfoCommand = new RelayCommand(_ => _navigationService.NavigateTo<GeneralInfoViewModel>());
            NavigateFinishCommand = new RelayCommand(_ => _navigationService.NavigateTo<FinishViewModel>());

            // Set initial page
            _navigationService.NavigateTo<GeneralInfoViewModel>();
        }
    }
}
