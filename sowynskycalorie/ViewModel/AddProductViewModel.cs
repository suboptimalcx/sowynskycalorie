using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using sowynskycalorie.Model; 
using System.Windows;
using sowynskycalorie.Stores;
using sowynskycalorie.DataAccess;

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
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set{ _selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); }
        }

        private int selectedGrams;
        public int SelectedGrams
        {
            get => selectedGrams;
            set { selectedGrams = value; OnPropertyChanged(nameof(SelectedGrams)); }
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
        private void ExecuteConfirmCommand(object parameter)
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
        private bool CanExecuteConfirmCommand(object parameter)
        {
            if(SelectedProduct == null || SelectedGrams <= 0) return false;
            return true;
        }
        public ICommand ExitViewCommand { get; }
        private void ExecuteExitViewCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = _calorieTrackerViewModel;
        }
        private bool CanExecuteExitViewCommand(object parameter)
        {
            return true;
        }
        public ICommand GoToAddMealCommand { get; }
        private void ExecuteGoToMealCommand(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddMealViewModel(_navigationStore, _calorieTrackerViewModel, _userId); 
        }
        private bool CanExecuteGoToMealCommand(object parameter)
        {
            return true;
        }
        public ICommand LikeProductCommand { get; }
        private void ExecuteLikeProductCommand(object parameter)
        {
            ProductDataHandler.likeProduct(SelectedProduct.Name, _userId, SelectedProduct.Id);
            if (!LikedProductIds.Contains(SelectedProduct.Id)) //locally add the product so it updates realtime
                LikedProductIds.Add(SelectedProduct.Id);
        }
        private bool CanExecuteLikeProductCommand(object parameter)
        {
            if (SelectedProduct == null) return false;
            return true;
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
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand, CanExecuteConfirmCommand);
            ExitViewCommand = new RelayCommand(ExecuteExitViewCommand, CanExecuteExitViewCommand);
            GoToAddMealCommand = new RelayCommand(ExecuteGoToMealCommand, CanExecuteGoToMealCommand);
            LikeProductCommand = new RelayCommand(ExecuteLikeProductCommand, CanExecuteLikeProductCommand);
            _userId = userId;
            ProductDataHandler.LoadData(_userId, LikedProductIds, AllProducts, Categories);
            FilterProducts();
        }
    }
}
