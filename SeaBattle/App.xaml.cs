using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Domain;
using SeaBattle.Domain.Configuration;
using SeaBattle.ViewModels;
using SeaBattle.Views;

namespace SeaBattle
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _serviceProvider = RegisterServices();
            var viewModel = _serviceProvider.GetService<MainWindowViewModel>();

            var window = new MainWindow { DataContext = viewModel };
            window.Show();
        }

        private ServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<GameConfiguration>(domainConfiguration =>
            {
                domainConfiguration.MapSize = 10;
            });
            serviceCollection.AddTransient<MainWindowViewModel>();
            serviceCollection.AddTransient<PlayerViewModel>();
            serviceCollection.RegisterDomain();
            return serviceCollection.BuildServiceProvider();
        }
    }
}
