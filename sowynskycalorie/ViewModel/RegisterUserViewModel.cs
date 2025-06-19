using sowynskycalorie.Model;
using sowynskycalorie.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static sowynskycalorie.Model.User;

namespace sowynskycalorie.ViewModel
{
    public class RegisterUserViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ICommand ToLoginCommand { get; }
        private void ExecuteToLoginCommand(object parameter)
        {
            User createdUser = new User(
                id: 6,
                username: "JoeShmoe",
                password: "mike",
                weight: 80.0f,         
                height: 180.0f,        
                sex: true,             
                activity: activityLevel.moderate,
                dob: new DateTime(1997, 1, 1)  
            );
            createdUser.addUserToDB();
            _navigationStore.CurrentViewModel = new LoginMenuViewModel(_navigationStore);
        }
        private bool CanExecute(object parameter)

        {
            return true;
        }
        public RegisterUserViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            ToLoginCommand = new RelayCommand(ExecuteToLoginCommand, CanExecute);
        }
    }
}
