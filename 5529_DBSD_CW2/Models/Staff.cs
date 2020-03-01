using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _00005529_DBSD_CW2.Models
{
    public class Staff
    {
        public int? Emp_ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy\\-MM\\-dd}")]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public int? WorkedHours { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy\\-MM\\-dd}")]
        public DateTime? HiringDate { get; set; }
    }
}