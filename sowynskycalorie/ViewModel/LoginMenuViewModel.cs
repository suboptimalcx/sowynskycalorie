using sowynskycalorie.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sowynskycalorie.ViewModel
{
    public class LoginMenuViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ICommand AccountCreationCommand { get; }
        private void ExecuteAccountCreationCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = new RegisterUserViewModel(_navigationStore);
        }
        private bool CanExecute(object parameter)
        {
            return true;
        }

        public LoginMenuViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            AccountCreationCommand = new RelayCommand(ExecuteAccountCreationCommand, CanExecute);
        }
    }
}
