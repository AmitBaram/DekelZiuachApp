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

        public ICommand NavigateLayersCommand { get; }
        public ICommand NavigateFormatsCommand { get; }
        public ICommand NavigateTichumCommand { get; }
        public ICommand NavigateMikudCommand { get; }
        public ICommand NavigateYeadimCommand { get; }
        public ICommand NavigateGeneralInfoCommand { get; }
        public ICommand NavigateFinishCommand { get; }

        public MainViewModel(INavigationService navigationService, AppData appData)
        {
            _navigationService = navigationService;
            _appData = appData;

            NavigateLayersCommand = new RelayCommand(_ => _navigationService.NavigateTo<LayersViewModel>());
            NavigateFormatsCommand = new RelayCommand(_ => _navigationService.NavigateTo<FormatsViewModel>());
            NavigateTichumCommand = new RelayCommand(_ => _navigationService.NavigateTo<TichumViewModel>());
            NavigateMikudCommand = new RelayCommand(_ => _navigationService.NavigateTo<MikudViewModel>());
            NavigateYeadimCommand = new RelayCommand(_ => _navigationService.NavigateTo<YeadimViewModel>());
            NavigateGeneralInfoCommand = new RelayCommand(_ => _navigationService.NavigateTo<GeneralInfoViewModel>());
            NavigateFinishCommand = new RelayCommand(_ => _navigationService.NavigateTo<FinishViewModel>());

            // Set initial page
            _navigationService.NavigateTo<LayersViewModel>();
        }
    }
}
