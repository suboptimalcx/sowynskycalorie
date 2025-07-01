using MySql.Data.MySqlClient;
using sowynskycalorie.Model;

namespace sowynskycalorie.DataAccess
{
    public class UserDataHandler
    {
        public static bool UsernameExists(string username)
        {
            bool exists = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = "SELECT COUNT(*) FROM sowynsky_calorie.Users WHERE username = @Username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        conn.Open();
                        long count = (long)cmd.ExecuteScalar();
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CHECKING USERNAME: " + ex.Message);
                exists = true;
            }

            return exists;
        }
        public static void addUserToDB(User user)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = @"USE sowynsky_calorie; INSERT INTO Users (username, password, weight, height, sex, activitylvl, DoB) 
                         VALUES (@Username, @Password, @Weight, @Height, @Sex, @Activity, @DoB)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Weight", user.Weight);
                        cmd.Parameters.AddWithValue("@Height", user.Height);
                        cmd.Parameters.AddWithValue("@Sex", user.Sex);
                        cmd.Parameters.AddWithValue("@Activity", user.Activity.ToString());
                        cmd.Parameters.AddWithValue("@DoB", user.DoB);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR WHILE ADDING USER TO  DATABASE: " + ex.Message);
            }
        }
        public static bool isPasswordOrUsernameCorrect(string username, string password)
        {
            bool isCorrect = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = "SELECT COUNT(*) FROM sowynsky_calorie.Users WHERE username = @Username AND password = @Password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        long count = (long)cmd.ExecuteScalar();
                        isCorrect = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR CHECKING CREDENTIALS IN DB: " + ex.Message);
                isCorrect = false;
            }

            return isCorrect;
        }
        public static User GetUserFromDatabase(string username, string password)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = @"USE sowynsky_calorie; 
                             SELECT id, username, password, weight, height, sex, activitylvl, DoB 
                             FROM Users 
                             WHERE username = @Username AND password = @Password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                string dbUsername = reader.GetString("username");
                                string dbPassword = reader.GetString("password");
                                float weight = reader.GetFloat("weight");
                                float height = reader.GetFloat("height");
                                bool sex = reader.GetBoolean("sex");
                                string activityStr = reader.GetString("activitylvl");
                                DateTime dob = reader.GetDateTime("DoB");

                                activityLevel activity = Enum.Parse<activityLevel>(activityStr);

                                User u = new User(dbUsername, dbPassword, weight, height, sex, activity, dob);
                                u.Id = id;
                                return u;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR FETCHING USER: " + ex.Message);
            }
            return null;
        }
    }
}
