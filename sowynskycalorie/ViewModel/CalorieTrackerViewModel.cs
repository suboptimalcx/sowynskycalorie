using sowynskycalorie.Model;
using sowynskycalorie.Stores;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace sowynskycalorie.ViewModel
{
    public class CalorieTrackerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly User _user;

        public ObservableCollection<Product> TrackedProducts { get; } = new ObservableCollection<Product>();
        public ObservableCollection<Meal> TrackedMeals { get; } = new ObservableCollection<Meal>();

        public int KcalGoal { get; set; }
        public int ProteinGoal { get; set; }
        public int FatGoal { get; set; }
        public int CarbsGoal { get; set; }

        private int _kcalProgress;
        public int KcalProgress
        {
            get => _kcalProgress;
            set { _kcalProgress = value; OnPropertyChanged(nameof(KcalProgress)); }
        }
        private int _proteinProgress;
        public int ProteinProgress
        {
            get => _proteinProgress;
            set { _proteinProgress = value; OnPropertyChanged(nameof(ProteinProgress)); }
        }
        private int _fatProgress;
        public int FatProgress
        {
            get => _fatProgress;
            set { _fatProgress = value; OnPropertyChanged(nameof(FatProgress)); }
        }
        private int _carbsProgress;
        public int CarbsProgress
        {
            get => _carbsProgress;
            set { _carbsProgress = value; OnPropertyChanged(nameof(CarbsProgress)); }
        }
        public ICommand AddFoodCommand { get; }

        private void ExecuteAddFoodCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddProductViewModel(_navigationStore, this, _user.Id);
        }
        private bool CanExecuteAddFoodCommand(object parameter)
        {
            return true;
        }
        public ICommand DeleteFoodCommand { get; }
        private void ExecuteDeleteFoodCommand(object parameter)
        {
            if (parameter is Product product && TrackedProducts.Contains(product))
            {
                TrackedProducts.Remove(product);
            }
        }
        private bool CanExecuteDeleteCommand(object parameter)
        {
            return true;
        }
        public ICommand DeleteMealCommand { get; }
        private void ExecuteDeleteMealCommand(object parameter)
        {
            if (parameter is Meal meal && TrackedMeals.Contains(meal))
            {
                TrackedMeals.Remove(meal);
            }
        }
        private void UpdateProgress()
        {
            var totalKcal = TrackedProducts.Sum(p => p.Calories) + TrackedMeals.Sum(m => m.TotalCalories);
            var totalProtein = TrackedProducts.Sum(p => p.Protein) + TrackedMeals.Sum(m => m.TotalProtein);
            var totalFat = TrackedProducts.Sum(p => p.Fat) + TrackedMeals.Sum(m => m.TotalFat); 
            var totalCarbs = TrackedProducts.Sum(p => p.Carbohydrates) + TrackedMeals.Sum(m => m.TotalCarbohydrates);

            KcalProgress = (int)Math.Round(totalKcal);
            ProteinProgress = (int)Math.Round(totalProtein);
            FatProgress = (int)Math.Round(totalFat);
            CarbsProgress = (int)Math.Round(totalCarbs);
        }
        public CalorieTrackerViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            AddFoodCommand = new RelayCommand(ExecuteAddFoodCommand, CanExecuteAddFoodCommand);
            DeleteFoodCommand = new RelayCommand(ExecuteDeleteFoodCommand, CanExecuteDeleteCommand);
            DeleteMealCommand = new RelayCommand(ExecuteDeleteMealCommand, CanExecuteDeleteCommand);
            KcalGoal = _user.KcalPerDay;

            ProteinGoal = (int)Math.Round((0.3 * KcalGoal) / 4); // 4 kcal/g
            FatGoal = (int)Math.Round((0.25 * KcalGoal) / 9);    // 9 kcal/g
            CarbsGoal = (int)Math.Round((0.45 * KcalGoal) / 4);  // 4 kcal/g

            TrackedProducts.CollectionChanged += (s, e) => UpdateProgress();
            TrackedMeals.CollectionChanged += (s, e) => UpdateProgress();
        }
    }
}
