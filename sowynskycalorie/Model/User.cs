using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace sowynskycalorie.Model
{
    public enum activityLevel
    {
        sedentary,
        light,
        moderate,
        very,
        super
    }
    public class User
    {
        public User(string username, string password, float weight, float height, bool sex, activityLevel activity, DateTime dob)
        {
            Username = username;
            Password = password;
            Weight = weight;
            Height = height;
            Sex = sex;
            Activity = activity;
            DoB = dob;
            double BMR = (sex == true) ? (66.47 + (13.75 * Weight) + (5.003 * Height) - (6.755 * DateTime.Today.Year - DoB.Year)) : (655.1 + (9.563 * Weight) + (1.85 * Height) - (4.676 * DateTime.Today.Year - DoB.Year));

            //ABSOLUTELY DIABOLICAL TEMPORARY SOLUTION, FOR THE LOVE OF GOD PLEASE FIX THIS LATER
            double multiplier;
            if (activity == activityLevel.sedentary)
                multiplier = 1.2;
            else if (activity == activityLevel.light)
                multiplier = 1.375;
            else if (activity == activityLevel.moderate)
                multiplier = 1.55;
            else if (activity == activityLevel.very)
                multiplier = 1.725;
            else if (activity == activityLevel.super)
                multiplier = 1.9;
            else
                multiplier = 1.0;

            KcalPerDay = (int)(BMR * multiplier);
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public float Weight { get; private set; }
        public float Height{ get; private set; }
        public bool Sex {  get; private set; } //0w 1m
        public activityLevel Activity { get; private set; }
        public DateTime DoB { get; private set; } //date of birth
        public int KcalPerDay { get; private set; }
        public void addUserToDB()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(App.ConnectionStr))
                {
                    string query = @"USE sowynsky_calorie; INSERT INTO Users (username, password, weight, height, sex, activitylvl, DoB) 
                         VALUES (@Username, @Password, @Weight, @Height, @Sex, @Activity, @DoB)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@Weight", Weight);
                        cmd.Parameters.AddWithValue("@Height", Height);
                        cmd.Parameters.AddWithValue("@Sex", Sex);
                        cmd.Parameters.AddWithValue("@Activity", Activity.ToString());
                        cmd.Parameters.AddWithValue("@DoB", DoB);

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
    }
}
