using MySql.Data.MySqlClient;
using sowynskycalorie.Model;
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
                _navigationStore.CurrentViewModel = new CalorieTrackerViewModel(_navigationStore, GetUserFromDatabase());
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
        private User GetUserFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = @"USE sowynsky_calorie; 
                             SELECT username, password, weight, height, sex, activitylvl, DoB 
                             FROM Users 
                             WHERE username = @Username AND password = @Password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username);
                        cmd.Parameters.AddWithValue("@Password", Password);

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbUsername = reader.GetString("username");
                                string dbPassword = reader.GetString("password");
                                float weight = reader.GetFloat("weight");
                                float height = reader.GetFloat("height");
                                bool sex = reader.GetBoolean("sex");
                                string activityStr = reader.GetString("activitylvl");
                                DateTime dob = reader.GetDateTime("DoB");

                                activityLevel activity = Enum.Parse<activityLevel>(activityStr);

                                return new User(dbUsername, dbPassword, weight, height, sex, activity, dob);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR FETCHING USER: " + ex.Message);
            }

            return null;
        }

        public LoginMenuViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            AccountCreationCommand = new RelayCommand(ExecuteAccountCreationCommand,CanExecute);
            LogInCommand = new RelayCommand(ExecuteLogInCommand,CanExecuteLogInCommand);
        }
    }
}
