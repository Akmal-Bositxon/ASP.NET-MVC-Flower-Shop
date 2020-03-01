using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _00005529_DBSD_CW2.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)][MaxLength(8)]
        public string Password { get; set; }
    }
}