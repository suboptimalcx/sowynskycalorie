using sowynskycalorie.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sowynskycalorie.ViewModel
{
    public class RegisterUserViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ICommand ToLoginCommand { get; }
        private void ExecuteToLoginCommand(object parameter)
        {
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
