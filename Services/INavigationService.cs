using DekelApp.ViewModels;

namespace DekelApp.Services
{
    public interface INavigationService
    {
        BaseViewModel? CurrentViewModel { get; }
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    }
}
