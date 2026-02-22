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
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IGeoCellService, GeoCellService>();

            // Models
            services.AddSingleton<AppData>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<TichumLayersViewModel>(s => new TichumLayersViewModel(s.GetRequiredService<AppData>(), s.GetRequiredService<IMessageService>()));
            services.AddTransient<FormatsViewModel>(s => new FormatsViewModel(s.GetRequiredService<AppData>().Formats));
            services.AddTransient<TichumViewModel>(s => new TichumViewModel(s.GetRequiredService<AppData>(), s.GetRequiredService<IMessageService>()));
            services.AddTransient<YeadimViewModel>(s => new YeadimViewModel(s.GetRequiredService<AppData>().YeadimTargets, s.GetRequiredService<AppData>()));
            services.AddTransient<GeneralInfoViewModel>(s => new GeneralInfoViewModel(s.GetRequiredService<AppData>().GeneralInfo));
            services.AddTransient<FinishViewModel>(s => new FinishViewModel(
                s.GetRequiredService<AppData>(),
                s.GetRequiredService<IValidationService>(),
                s.GetRequiredService<IFileService>(),
                s.GetRequiredService<IMessageService>(),
                s.GetRequiredService<IGeoCellService>()));

            // Navigation Factory
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => 
                viewModelType => {
                    var vm = (BaseViewModel)serviceProvider.GetRequiredService(viewModelType);
                    if (vm is GeneralInfoViewModel) { /* just testing */ }
                    return vm;
                });

            _serviceProvider = services.BuildServiceProvider();

            this.DispatcherUnhandledException += (s, e) =>
            {
                System.IO.File.WriteAllText("crash.log", e.Exception.ToString());
                System.Windows.MessageBox.Show(e.Exception.Message, "Crash", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                e.Handled = true;
            };
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
