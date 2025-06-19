using sowynskycalorie.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sowynskycalorie.ViewModel
{
    public class CalorieTrackerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public CalorieTrackerViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
