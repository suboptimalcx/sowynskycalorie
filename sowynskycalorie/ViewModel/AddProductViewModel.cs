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
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Product> AllProducts { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<Product> FilteredProducts { get; set; } = new ObservableCollection<Product>();

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

        public ICommand ConfirmCommand { get; }

        private void ExecuteConfirm(object parameter)
        {
            _calorieTrackerViewModel.TrackedProducts.Add(SelectedProduct);
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

        private void LoadData()
        {
            try
            {
                using (var conn = new MySqlConnection(App.ConnectionStr))
                {
                    conn.Open();
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
        private void FilterProducts()
        {
            FilteredProducts.Clear();

            if (string.IsNullOrEmpty(SelectedCategory)) //TODO : FIX, AFTER SELECTING A FILTER YOU CANT UNSELECT IT WHATEVER MAN I JUST WANT TO FINISH THIS
            {
                foreach (var product in AllProducts)
                    FilteredProducts.Add(product);
            }
            else
            {
                foreach (var product in AllProducts.Where(p => p.Category == SelectedCategory))
                    FilteredProducts.Add(product);
            }
        }
        public AddProductViewModel(NavigationStore navigationStore, CalorieTrackerViewModel calorieTrackerViewModel)
        {
            _navigationStore = navigationStore;
            LoadData();
            _calorieTrackerViewModel = calorieTrackerViewModel;
            ConfirmCommand = new RelayCommand(ExecuteConfirm, CanExecuteConfirm);
            ExitViewCommand = new RelayCommand(ExecuteExitView, CanExecuteExitView);
        }
    }
}
