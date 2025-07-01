using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;

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

            int age = DateTime.Today.Year - dob.Year;
            if (DateTime.Today < dob.AddYears(age))
                age--;

            double bmr;
            if (sex) bmr = 66.47 + (13.75 * weight) + (5.003 * height) - (6.755 * age);
            else bmr = 655.1 + (9.563 * weight) + (1.85 * height) - (4.676 * age);

            double multiplier = activity switch
            {
                activityLevel.sedentary => 1.2,
                activityLevel.light => 1.375,
                activityLevel.moderate => 1.55,
                activityLevel.very => 1.725,
                activityLevel.super => 1.9,
                _ => 1.0
            };

            KcalPerDay = (int)(bmr * multiplier);
        }
        public int Id { get; internal set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public float Weight { get; private set; }
        public float Height{ get; private set; }
        public bool Sex {  get; private set; } //0w 1m
        public activityLevel Activity { get; private set; }
        public DateTime DoB { get; private set; } //date of birth
        public int KcalPerDay { get; private set; }
    }
}
