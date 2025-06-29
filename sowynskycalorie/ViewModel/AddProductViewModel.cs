using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using sowynskycalorie.Model; 
using System.Windows;
using System.Windows.Media.Media3D;
using sowynskycalorie.Stores;

namespace sowynskycalorie.ViewModel
{
    public class AddProductViewModel : ViewModelBase
    {
        private CalorieTrackerViewModel _calorieTrackerViewModel;
        private readonly NavigationStore _navigationStore;
        private readonly int _userId;
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Product> AllProducts { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<Product> FilteredProducts { get; set; } = new ObservableCollection<Product>();
        private List<int> LikedProductIds { get; set; } = new List<int>();

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterProducts();
            }
        }
        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set{ selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); }
        }

        private int selectedGrams;
        public int SelectedGrams
        {
            get => selectedGrams;
            set
            { selectedGrams = value; OnPropertyChanged(nameof(SelectedGrams)); }
        }
        private bool _showLikedOnly;
        public bool ShowLikedOnly
        {
            get => _showLikedOnly;
            set
            {
                _showLikedOnly = value;
                OnPropertyChanged(nameof(ShowLikedOnly));
                FilterProducts();
            }
        }

        public ICommand ConfirmCommand { get; }

        private void ExecuteConfirm(object parameter)
        {
            double calculatedProtein = Math.Round(SelectedProduct.Protein * SelectedGrams / 100.0, 2);
            double calculatedKcal = Math.Round(SelectedProduct.Calories * SelectedGrams / 100.0, 2);
            double calculatedCarbohydrates = Math.Round(SelectedProduct.Carbohydrates * SelectedGrams / 100.0, 2);
            double calculatedFat = Math.Round(SelectedProduct.Fat * SelectedGrams / 100.0, 2);

            Product CalculatedProduct = new Product
            {
                Protein = calculatedProtein,
                Calories = calculatedKcal,
                Carbohydrates = calculatedCarbohydrates,
                Fat = calculatedFat,
                Name = SelectedProduct.Name,
                Category = SelectedProduct.Category,
                Id = SelectedProduct.Id,
                Grams = SelectedGrams
            };

            _calorieTrackerViewModel.TrackedProducts.Add(CalculatedProduct);
            MessageBox.Show($"Added {SelectedGrams}g of {SelectedProduct?.Name}.");

        }
        private bool CanExecuteConfirm(object parameter)
        {
            if(SelectedProduct == null || SelectedGrams <= 0) return false;
            return true;
        }
        public ICommand ExitViewCommand { get; }
        private void ExecuteExitView(object parameter)
        {
            _navigationStore.CurrentViewModel = _calorieTrackerViewModel;
        }
        private bool CanExecuteExitView(object parameter)
        {
            return true;
        }
        public ICommand GoToAddMealCommand { get; }
        private void ExecuteGoToMeal(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddMealViewModel(_navigationStore, _calorieTrackerViewModel, _userId); 
        }
        private bool CanExecuteGoToMeal(object parameter)
        {
            return true;
        }
        public ICommand LikeProductCommand { get; }
        private void ExecuteLikeProduct(object parameter)
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();

                string checkQuery = "SELECT preference FROM products_preferences WHERE userID = @userID AND productID = @productID";
                using var checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@userID", _userId);
                checkCmd.Parameters.AddWithValue("@productID", SelectedProduct.Id);

                var result = checkCmd.ExecuteScalar();

                if (result != null && result.ToString() == "like")
                {
                    MessageBox.Show("You already liked this product.");
                    return;
                }
                else
                {
                    // Insert new preference
                    string insertQuery = "INSERT INTO products_preferences (userID, productID, preference) VALUES (@userID, @productID, 'like')";
                    using var insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@userID", _userId);
                    insertCmd.Parameters.AddWithValue("@productID", SelectedProduct.Id);
                    insertCmd.ExecuteNonQuery();
                }

                MessageBox.Show($"You liked {SelectedProduct.Name}.");

                if (!LikedProductIds.Contains(SelectedProduct.Id)) //locally add the product so it updates realtime
                    LikedProductIds.Add(SelectedProduct.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error toggling like: {ex.Message}");
            }
        }
        private bool CanExecuteLikeProduct(object parameter)
        {
            if (SelectedProduct == null) return false;
            return true;
        }
        private void LoadData() //diabolical, refactor
        {
            try
            {
                using (var conn = new MySqlConnection(App.ConnectionStr))
                {
                    conn.Open();

                    //liked product ID
                    string likesQuery = "SELECT productID FROM products_preferences WHERE userID = @userId AND preference = 'like'";
                    using (var cmd = new MySqlCommand(likesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", _userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LikedProductIds.Add(reader.GetInt32("productID"));
                            }
                        }
                    }

                    //all products
                    string query = "SELECT * FROM products";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Calories = reader.GetDouble("calories"),
                                Protein = reader.GetDouble("protein"),
                                Carbohydrates = reader.GetDouble("carbohydrates"),
                                Fat = reader.GetDouble("fat"),
                                Category = reader.GetString("category")
                            };

                            AllProducts.Add(product);

                            if (!Categories.Contains(product.Category))
                                Categories.Add(product.Category);
                        }
                    }

                    FilterProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR WHILE LOADING PRODUCTS: " + ex.Message);
            }
        }
        private void FilterProducts() //TODO : FIX, AFTER SELECTING A FILTER YOU CANT UNSELECT IT
        {
            FilteredProducts.Clear();

            var productsToFilter = AllProducts.AsEnumerable();

            if (ShowLikedOnly)
                productsToFilter = productsToFilter.Where(p => LikedProductIds.Contains(p.Id));

            if (!string.IsNullOrEmpty(SelectedCategory))
                productsToFilter = productsToFilter.Where(p => p.Category == SelectedCategory);

            foreach (var product in productsToFilter)
                FilteredProducts.Add(product);
        }

        public AddProductViewModel(NavigationStore navigationStore, CalorieTrackerViewModel calorieTrackerViewModel, int userId)
        {
            _navigationStore = navigationStore;
            _calorieTrackerViewModel = calorieTrackerViewModel;
            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanExecuteConfirm);
            ExitViewCommand = new RelayCommand(ExecuteExitView, CanExecuteExitView);
            GoToAddMealCommand = new RelayCommand(ExecuteGoToMeal, CanExecuteGoToMeal);
            LikeProductCommand = new RelayCommand(ExecuteLikeProduct, CanExecuteLikeProduct);
            _userId = userId;
            LoadData();
        }
    }
}
