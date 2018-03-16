using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.Models
{
    public class User : SuperUser
    {
        public DateTime regTime { get; set; }
        public override string getType()
        {
            return "MORTY";
        }
    }
}