using System;
using System.Collections.Generic;

namespace sowynskycalorie.Model
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MealRating> Ratings { get; set; } = new List<MealRating>();

        public double? AverageRating => Ratings.Count > 0 ? Ratings.Average(r => r.Rating ?? 0) : (double?)null;
        public double TotalCalories => MealProducts.Sum(p => p.TotalCalories);
        public double TotalProtein => MealProducts.Sum(p => p.TotalProtein);
        public double TotalCarbohydrates => MealProducts.Sum(p => p.TotalCarbohydrates);
        public double TotalFat => MealProducts.Sum(p => p.TotalFat);
        public int TotalGrams => MealProducts.Sum(p => p.Grams);

        public List<MealProduct> MealProducts { get; set; } = new List<MealProduct>();
    }

    public class MealProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public double CaloriesPer100g { get; set; }
        public double ProteinPer100g { get; set; }
        public double CarbohydratesPer100g { get; set; }
        public double FatPer100g { get; set; }

        public string Category { get; set; }
        public int Grams { get; set; }

        public double TotalCalories => (CaloriesPer100g / 100) * Grams;
        public double TotalProtein => (ProteinPer100g / 100) * Grams;
        public double TotalCarbohydrates => (CarbohydratesPer100g / 100) * Grams;
        public double TotalFat => (FatPer100g / 100) * Grams;
    }
    public class MealRating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MealId { get; set; }
        public int? Rating { get; set; } 
    }

}
