using sowynskycalorie.Model;
using sowynskycalorie.Stores;
using System;
using System.ComponentModel;

namespace sowynskycalorie.ViewModel
{
    public class CalorieTrackerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly User _user;

        public int KcalGoal { get; set; }
        public int ProteinGoal { get; set; }
        public int FatGoal { get; set; }
        public int CarbsGoal { get; set; }

        public CalorieTrackerViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            KcalGoal = _user.KcalPerDay;

            ProteinGoal = (int)Math.Round((0.3 * KcalGoal) / 4); // 4 kcal/g
            FatGoal = (int)Math.Round((0.25 * KcalGoal) / 9);    // 9 kcal/g
            CarbsGoal = (int)Math.Round((0.45 * KcalGoal) / 4);  // 4 kcal/g
        }
    }
}
