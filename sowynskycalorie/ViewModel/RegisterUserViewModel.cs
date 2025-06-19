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
        public RegisterUserViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
