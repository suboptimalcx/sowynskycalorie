using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sowynskycalorie.Model
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public float Weight { get; set; }
        public float Height{ get; set; }
        public int DoB{ get; set; } //date of birth
    }
}
