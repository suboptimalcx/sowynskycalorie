using sowynskycalorie.Stores;
using sowynskycalorie.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace sowynskycalorie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly NavigationStore _navigationStore;
    public App()
    {
        _navigationStore = new NavigationStore();
    }
    protected override void OnStartup(StartupEventArgs e)
    {

        _navigationStore.CurrentViewModel = new LoginMenuViewModel(_navigationStore);
        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_navigationStore)
        };
        MainWindow.Show();
        base.OnStartup(e);
    }
}

