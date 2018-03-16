using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public abstract class SuperUser
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public abstract string getType();
    }
}