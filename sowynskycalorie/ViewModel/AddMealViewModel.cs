using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using sowynskycalorie.DataAccess;
using sowynskycalorie.Model;
using sowynskycalorie.Stores;

namespace sowynskycalorie.ViewModel
{
    public class AddMealViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly CalorieTrackerViewModel _calorieTrackerViewModel;
        private readonly int _userId;

        public ObservableCollection<Meal> AllMeals { get; set; } = new ObservableCollection<Meal>();

        private Meal _selectedMeal;
        public Meal SelectedMeal
        {
            get => _selectedMeal;
            set { _selectedMeal = value; OnPropertyChanged(nameof(SelectedMeal)); }
        }
        private int _selectedRating;
        public int SelectedRating
        {
            get => _selectedRating;
            set { _selectedRating = value; OnPropertyChanged(nameof(SelectedRating)); }
        }

        public ICommand RateMealCommand { get; }
        private void ExecuteRateMeal(object parameter)
        {
            MealDataHandler.ExecuteRateMeal(_userId, SelectedMeal, SelectedRating);
            OnPropertyChanged(nameof(SelectedMeal));
            MessageBox.Show($"You rated {SelectedMeal.Name} as {SelectedRating}/10.");
        }
        private bool CanExecuteRateMeal(object parameter)
        {
            return SelectedMeal != null && SelectedRating != null;
        }
        public ICommand ConfirmCommand { get; }
        private void ExecuteConfirm(object parameter)
        {
            if (SelectedMeal == null)
                return;

            _calorieTrackerViewModel.TrackedMeals.Add(SelectedMeal);

            MessageBox.Show($"Added meal: {SelectedMeal.Name}");
        }
        private bool CanExecuteConfirm(object parameter)
        {
            return SelectedMeal != null;
        }
        public ICommand ExitViewCommand { get; }
        private bool CanExecuteExitView(object parameter)
        {
            return true;
        }
        private void ExecuteExitView(object parameter)
        {
            _navigationStore.CurrentViewModel = _calorieTrackerViewModel;
        }
        public ICommand GoToAddProductCommand { get; }
        private bool CanExecuteGoToAddProduct(object parameter)
        {
            return true;
        }
        private void ExecuteGoToAddProduct(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddProductViewModel(_navigationStore, _calorieTrackerViewModel, _userId);
        }

        public AddMealViewModel(NavigationStore navigationStore, CalorieTrackerViewModel calorieTrackerViewModel, int userId)
        {
            _navigationStore = navigationStore;
            _calorieTrackerViewModel = calorieTrackerViewModel;
            _userId = userId;

            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanExecuteConfirm);
            ExitViewCommand = new RelayCommand(ExecuteExitView, CanExecuteExitView);
            RateMealCommand = new RelayCommand(ExecuteRateMeal, CanExecuteRateMeal);
            GoToAddProductCommand = new RelayCommand(ExecuteGoToAddProduct, CanExecuteGoToAddProduct);

            MealDataHandler.LoadMealsFromDatabase(AllMeals);
        }
    }
}
