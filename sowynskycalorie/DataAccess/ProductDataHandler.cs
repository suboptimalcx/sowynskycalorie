using MySql.Data.MySqlClient;
using sowynskycalorie.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace sowynskycalorie.DataAccess
{
    public class ProductDataHandler
    {
        public static void LoadData(int userId, List<int> likedproductsids, ObservableCollection<Product> allproducts, ObservableCollection<string> categories)
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();

                LoadLikedProductIds(conn, userId, likedproductsids);
                LoadAllProducts(conn, allproducts, categories);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR WHILE LOADING PRODUCTS: " + ex.Message);
            }
        }

        private static void LoadLikedProductIds(MySqlConnection conn, int userid, List<int> likedproductsids)
        {
            string likesQuery = "SELECT productID FROM products_preferences WHERE userID = @userId AND preference = 'like'";
            using var cmd = new MySqlCommand(likesQuery, conn);
            cmd.Parameters.AddWithValue("@userId", userid);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                likedproductsids.Add(reader.GetInt32("productID"));
            }
        }

        private static void LoadAllProducts(MySqlConnection conn, ObservableCollection<Product> allproducts, ObservableCollection<string> categories)
        {
            string query = "SELECT * FROM products";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
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

                allproducts.Add(product);

                if (!categories.Contains(product.Category))
                    categories.Add(product.Category);
            }
        }

        //! TEMPORARY SOLUTION, SIMILAR TO RATE MEAL, ONCE LIKE A PRODUCT YOU CANNOT "UNLIKE" IT 
        // On a side note, i dont like the "like/dislike" feature!!!! keeping a table for every users preferenc for EVERY product?????? ill delete it propably lol 
        public static void likeProduct(string productname, int userid, int productid)  
        {
            try
            {
                using var conn = new MySqlConnection(App.ConnectionStr);
                conn.Open();
                //check if product already liked
                string checkQuery = "SELECT preference FROM products_preferences WHERE userID = @userID AND productID = @productID";
                using var checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@userID", userid);
                checkCmd.Parameters.AddWithValue("@productID", productid);

                var result = checkCmd.ExecuteScalar();

                if (result != null && result.ToString() == "like")
                {
                    MessageBox.Show("You already liked this product.");
                    return;
                }
                else
                {
                    string insertQuery = "INSERT INTO products_preferences (userID, productID, preference) VALUES (@userID, @productID, 'like')";
                    using var insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@userID", userid);
                    insertCmd.Parameters.AddWithValue("@productID", productid);
                    insertCmd.ExecuteNonQuery();
                }

                MessageBox.Show($"You liked {productname}.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error toggling like: {ex.Message}");
            }
        }
    }
}
