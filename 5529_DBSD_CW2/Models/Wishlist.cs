using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _00005529_DBSD_CW2.Models
{
    public class Wishlist
    {
        [Required]
        public int WishId { get; set; }
        [Required]
        public int Customer_id { get; set; }
        [Required]
        public int FlowerId { get; set; }
        [Required]
        [DisplayName("Created Date")]
        public DateTime? CreatedDate { get; set; }
        [Required]
        [DisplayName("Number Of Flowers")]
        public int NumberOfFlowers { get; set; }
        [Required]
        public string Comments { get; set; }
        public Flower Flow { get; set; }

    }
}