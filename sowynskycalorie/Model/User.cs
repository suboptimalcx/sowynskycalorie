using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace sowynskycalorie.Model
{
    class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public float Weight { get; private set; }
        public float Height{ get; private set; }
        public bool Sex {  get; private set; } //0w 1m
        public enum activityLevel
        {
            sedentary,
            light,
            moderate,
            very,
            super
        }
        public int DoB{ get; private set; } //date of birth
        public int BMR{ get; private set; } //date of birth

        public User()
        {
            
        }
    }
}
