using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.BC;
using sowynskycalorie.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace sowynskycalorie.DataAccess
{
    public class MealDataHandler
    {
        public static void LoadAllMeals(MySqlConnection conn, Dictionary<int, Meal> mealsdict) // meals table
        {
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
                    mealsdict.Add(meal.Id, meal);
                }
            }
        }
        public static void LoadMealProducts(MySqlConnection conn, Dictionary<int, Meal> mealsdict) //meals_products table 
        {
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
                    if (!mealsdict.TryGetValue(mealID, out var meal))
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
        }
        public static void LoadMealRatings(MySqlConnection conn, Dictionary<int, Meal> mealsdict) //meals_ratings table
        {
            string ratingsQuery = "SELECT * FROM meals_ratings";
            using (var ratingsCmd = new MySqlCommand(ratingsQuery, conn))
            using (var ratingsReader = ratingsCmd.ExecuteReader())
            {
                while (ratingsReader.Read())
                {
                    int mealId = ratingsReader.GetInt32("mealID");
                    if (!mealsdict.TryGetValue(mealId, out var meal))
                        continue;

                    var rating = new MealRating
                    {
                        Id = ratingsReader.GetInt32("id"),
                        UserId = ratingsReader.GetInt32("userID"),
                        MealId = mealId,
                        Rating = ratingsReader.IsDBNull("meal_rating") ? null : ratingsReader.GetInt32("meal_rating")
                    };

                    meal.Ratings.Add(rating);
                }
            }
        }
        public static void LoadMealsFromDatabase(ObservableCollection<Meal> allmeals)
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();
                var mealsDict = new Dictionary<int, Meal>();

                LoadAllMeals(conn, mealsDict);
                LoadMealProducts(conn, mealsDict);
                LoadMealRatings(conn, mealsDict);

                foreach (var meal in mealsDict.Values)
                    allmeals.Add(meal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR LOADING MEALS: {ex.Message}");
            }
        }
        public static void ExecuteRateMeal(int userid, Meal meal, int rating)
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM meals_ratings WHERE userID = @userID AND mealID = @mealID";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@userID", userid);
                    checkCmd.Parameters.AddWithValue("@mealID", meal.Id);

                    var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                    if (exists)
                    {
                        MessageBox.Show("You've already rated this meal.");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO meals_ratings (userID, mealID, meal_rating) VALUES (@userID, @mealID, @rating)";
                using var cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@userID", userid);
                cmd.Parameters.AddWithValue("@mealID", meal.Id);
                cmd.Parameters.AddWithValue("@rating", rating.ToString());

                cmd.ExecuteNonQuery();

                meal.Ratings.Add(new MealRating
                {
                    UserId = userid,
                    MealId = meal.Id,
                    Rating = rating
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rating: {ex.Message}");
            }
        }
    }
}
