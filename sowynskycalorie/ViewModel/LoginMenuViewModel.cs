using MySql.Data.MySqlClient;
using sowynskycalorie.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (!isPasswordOrUsernameCorrect())
            {
                MessageBox.Show("Wrong login or password!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _navigationStore.CurrentViewModel = new CalorieTrackerViewModel(_navigationStore);
            }
        }
        private bool CanExecuteLogInCommand(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Username)) return false;
            if (string.IsNullOrWhiteSpace(Password)) return false;
            return true;
        }
        private bool isPasswordOrUsernameCorrect()
        {
            bool isCorrect = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = "SELECT COUNT(*) FROM sowynsky_calorie.Users WHERE username = @Username AND password = @Password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username);
                        cmd.Parameters.AddWithValue("@Password", Password); 

                        conn.Open();
                        long count = (long)cmd.ExecuteScalar();
                        isCorrect = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CHECKING CREDENTIALS IN DB: " + ex.Message);
                isCorrect = false;
            }

            return isCorrect;
        }

        public LoginMenuViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            AccountCreationCommand = new RelayCommand(ExecuteAccountCreationCommand,CanExecute);
            LogInCommand = new RelayCommand(ExecuteLogInCommand,CanExecuteLogInCommand);
        }
    }
}
