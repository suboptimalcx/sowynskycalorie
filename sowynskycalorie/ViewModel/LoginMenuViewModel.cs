using sowynskycalorie.DataAccess;
using sowynskycalorie.Stores;
using System.Windows;
using System.Windows.Input;

namespace sowynskycalorie.ViewModel
{
    public class LoginMenuViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public ICommand AccountCreationCommand { get; }
        private void ExecuteAccountCreationCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = new RegisterUserViewModel(_navigationStore);
        }
        private bool CanExecute(object parameter)
        {
            return true;
        }
        public ICommand LogInCommand { get; }
        private void ExecuteLogInCommand(object parameter)
        {
            if (!UserDataHandler.isPasswordOrUsernameCorrect(Username, Password))
            {
                MessageBox.Show("Wrong login or password!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _navigationStore.CurrentViewModel = new CalorieTrackerViewModel(_navigationStore, UserDataHandler.GetUserFromDatabase(Username, Password));
            }
        }
        private bool CanExecuteLogInCommand(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Username)) return false;
            if (string.IsNullOrWhiteSpace(Password)) return false;
            return true;
        }
        public LoginMenuViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            AccountCreationCommand = new RelayCommand(ExecuteAccountCreationCommand,CanExecute);
            LogInCommand = new RelayCommand(ExecuteLogInCommand,CanExecuteLogInCommand);
        }
    }
}
