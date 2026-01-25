using System;
using System.Windows;
using DekelApp.Models;
using DekelApp.Services;
using DekelApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DekelApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IFileService, FileService>();

            // Models
            services.AddSingleton<AppData>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<TichumLayersViewModel>(s => new TichumLayersViewModel(s.GetRequiredService<AppData>().Tichum.Layers));
            services.AddTransient<MikudLayersViewModel>(s => new MikudLayersViewModel(s.GetRequiredService<AppData>()));
            services.AddTransient<FormatsViewModel>(s => new FormatsViewModel(s.GetRequiredService<AppData>().Formats));
            services.AddTransient<TichumViewModel>(s => new TichumViewModel(s.GetRequiredService<AppData>()));
            services.AddTransient<MikudViewModel>(s => new MikudViewModel(s.GetRequiredService<AppData>()));
            services.AddTransient<YeadimViewModel>(s => new YeadimViewModel(s.GetRequiredService<AppData>().YeadimTargets, s.GetRequiredService<AppData>()));
            services.AddTransient<GeneralInfoViewModel>(s => new GeneralInfoViewModel(s.GetRequiredService<AppData>().GeneralInfo));
            services.AddTransient<FinishViewModel>();

            // Navigation Factory
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => 
                viewModelType => {
                    var vm = (BaseViewModel)serviceProvider.GetRequiredService(viewModelType);
                    if (vm is GeneralInfoViewModel) { /* just testing */ }
                    return vm;
                });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
