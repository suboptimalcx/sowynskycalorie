using sowynskycalorie.DataAccess;
using sowynskycalorie.Model;
using sowynskycalorie.Stores;
using System.Windows;
using System.Windows.Input;

namespace sowynskycalorie.ViewModel
{
    public class RegisterUserViewModel : ViewModelBase
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

        private bool _sex;
        public bool Sex
        {
            get => _sex;
            set { _sex = value; OnPropertyChanged(nameof(Sex)); }
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }

        private float _weight;
        public float Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(nameof(Weight)); }
        }

        private float _height;
        public float Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(nameof(Height)); }
        }
        private activityLevel _selectedActivityLevel = activityLevel.sedentary;
        public activityLevel SelectedActivityLevel
        {
            get => _selectedActivityLevel;
            set { _selectedActivityLevel = value; OnPropertyChanged(nameof(SelectedActivityLevel)); }
        }
        public ICommand ToLoginCommand { get; }
        
        private void ExecuteToLoginCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = new LoginMenuViewModel(_navigationStore);
        }
        private bool CanExecuteToLoginCommand(object parameter)

        {
            return true;
        }
        public ICommand RegisterCommand { get; }
        private void ExecuteRegisterCommand(object parameter)
        {
            if (UserDataHandler.UsernameExists(Username))
            {
                MessageBox.Show("Username already taken!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User createdUser = new User(
                username: Username,
                password: Password,
                weight: Weight,
                height: Height,
                sex: Sex,
                activity: SelectedActivityLevel,
                dob: SelectedDate
            );
            UserDataHandler.addUserToDB(createdUser);
            _navigationStore.CurrentViewModel = new LoginMenuViewModel(_navigationStore);
        }
        private bool CanExecuteRegisterCommand(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Username)) return false;
            if (string.IsNullOrWhiteSpace(Password)) return false;
            if (Weight <= 0) return false;
            if (Height <= 0) return false;
            if (SelectedDate >= DateTime.Now) return false;

            return true;
        }

        public RegisterUserViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            ToLoginCommand = new RelayCommand(ExecuteToLoginCommand, CanExecuteToLoginCommand);
            RegisterCommand = new RelayCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        }
    }
}
