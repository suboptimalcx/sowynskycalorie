using sowynskycalorie.Model;
using sowynskycalorie.Stores;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace sowynskycalorie.ViewModel
{
    public class CalorieTrackerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly User _user;

        public ObservableCollection<Product> TrackedProducts { get; } = new ObservableCollection<Product>();

        public int KcalGoal { get; set; }
        public int ProteinGoal { get; set; }
        public int FatGoal { get; set; }
        public int CarbsGoal { get; set; }
        public ICommand AddFoodCommand { get; }

        private void ExecuteAddFoodCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddProductViewModel(_navigationStore, this);
        }
        private bool CanExecuteAddFoodCommand(object parameter)
        {
            return true;
        }

        public CalorieTrackerViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            AddFoodCommand = new RelayCommand(ExecuteAddFoodCommand, CanExecuteAddFoodCommand);
            KcalGoal = _user.KcalPerDay;

            ProteinGoal = (int)Math.Round((0.3 * KcalGoal) / 4); // 4 kcal/g
            FatGoal = (int)Math.Round((0.25 * KcalGoal) / 9);    // 9 kcal/g
            CarbsGoal = (int)Math.Round((0.45 * KcalGoal) / 4);  // 4 kcal/g
        }
    }
}
