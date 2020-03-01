using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace _00005529_DBSD_CW2.Models
{
    public class Report
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("UserName")]
        public string username { get; set; }
        [DisplayName("Number Of Wishes")]
        public int WishListCount { get; set; }
    }
}