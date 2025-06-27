using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
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
        private int? _selectedRating;
        public int? SelectedRating
        {
            get => _selectedRating;
            set { _selectedRating = value; OnPropertyChanged(nameof(SelectedRating)); }
        }

        public ICommand RateMealCommand { get; }
        private void ExecuteRateMeal(object parameter)
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM meals_ratings WHERE userID = @userID AND mealID = @mealID";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@userID", _userId);
                    checkCmd.Parameters.AddWithValue("@mealID", SelectedMeal.Id);

                    var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                    if (exists)
                    {
                        MessageBox.Show("You've already rated this meal.");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO meals_ratings (userID, mealID, meal_rating) VALUES (@userID, @mealID, @rating)";
                using var cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@userID", _userId);
                cmd.Parameters.AddWithValue("@mealID", SelectedMeal.Id);
                cmd.Parameters.AddWithValue("@rating", SelectedRating.ToString());

                cmd.ExecuteNonQuery();

                SelectedMeal.Ratings.Add(new MealRating
                {
                    UserId = _userId,
                    MealId = SelectedMeal.Id,
                    Rating = SelectedRating
                });

                OnPropertyChanged(nameof(SelectedMeal));
                MessageBox.Show($"You rated {SelectedMeal.Name} as {SelectedRating}/10.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rating: {ex.Message}");
            }
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
        private void LoadMealsFromDatabase() //TODO: chop into smaller functions later
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();

                var mealsDict = new Dictionary<int, Meal>();

                //Load all meals
                string mealQuery = "SELECT * FROM meals";
                using (var mealCmd = new MySqlCommand(mealQuery, conn))
                using (var mealReader = mealCmd.ExecuteReader())
                {
                    while (mealReader.Read())
                    {
                        var meal = new Meal
                        {
                            Id = mealReader.GetInt32("id"),
                            Name = mealReader.GetString("name"),
                            Description = mealReader.GetString("description")
                        };
                        mealsDict.Add(meal.Id, meal);
                    }
                }

                // Load products
                string mealProductsQuery = @"
            SELECT mp.mealID, p.id AS productID, p.name, p.calories, p.protein, p.carbohydrates, p.fat, p.category, mp.grams
            FROM meals_products mp
            INNER JOIN products p ON mp.productID = p.id";
                using (var mpCmd = new MySqlCommand(mealProductsQuery, conn))
                using (var mpReader = mpCmd.ExecuteReader())
                {
                    while (mpReader.Read())
                    {
                        int mealID = mpReader.GetInt32("mealID");
                        if (!mealsDict.TryGetValue(mealID, out var meal))
                            continue;

                        var mealProduct = new MealProduct
                        {
                            ProductId = mpReader.GetInt32("productID"),
                            ProductName = mpReader.GetString("name"),
                            CaloriesPer100g = mpReader.GetDouble("calories"),
                            ProteinPer100g = mpReader.GetDouble("protein"),
                            CarbohydratesPer100g = mpReader.GetDouble("carbohydrates"),
                            FatPer100g = mpReader.GetDouble("fat"),
                            Category = mpReader.GetString("category"),
                            Grams = mpReader.GetInt32("grams")
                        };

                        meal.MealProducts.Add(mealProduct);
                    }
                }

                foreach (var meal in mealsDict.Values)
                    AllMeals.Add(meal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR LOADING MEALS: {ex.Message}");
            }
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

            LoadMealsFromDatabase();
        }
    }
}
