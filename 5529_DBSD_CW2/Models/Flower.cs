using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _00005529_DBSD_CW2.Models
{
    public class Flower
    {
        [Required]
        public int? FlowerId { get; set; }
        [Required]
        [DisplayName("Name")]
        public string FlowerName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy\\-MM\\-dd}")]
        public DateTime? DeliveredDate { get; set; }

        public string Color { get; set; }
        public int Price { get; set; }
    }
    public enum Colors
    {
        White,
        Pink,
        Red,
        Yellow,
        Green,
        Blue,
        Violet
    }
}