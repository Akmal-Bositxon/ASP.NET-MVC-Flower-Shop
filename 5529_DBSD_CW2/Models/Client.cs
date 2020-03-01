using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace _00005529_DBSD_CW2.Models
{
    public class Client
    {
        public int? Customer_ID { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy\\-MM\\-dd}")]
        [DisplayName("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public string Country { get; set; }
        //public string District { get; set; }
        //public string Street { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public byte[] Photo { get; set; }
        public string Title { get; set; }
    }
   public enum Titles
    {
        Mr,
        Mrs,
        Ms,
        Dr,

    }
    public enum Countries
    {
        Afghanistan, Albania, Algeria, Andorra, Angola, Antigua_Deps, Argentina, Armenia, Australia, Austria, Azerbaijan, Bahamas, Bahrain, Bangladesh, Barbados, Belarus, Belgium, Belize, Benin, Bhutan, Bolivia, Bosnia_Herzegovina, Botswana, Brazil, Brunei, Bulgaria, Burkina, Burma, Burundi, Cambodia, Cameroon, Canada, Cape_Verde, Central_African_Rep, Chad, Chile, Peoples_Republic_of_China, Republic_of_China, Colombia, Comoros, Democratic_Republic_of_the_Congo, Republic_of_the_Congo, Costa_Rica, Croatia, Cuba, Cyprus, Czech_Republic, Danzig, Denmark, Djibouti, Dominica, Dominican_Republic, East_Timor, Ecuador, Egypt, El_Salvador, Equatorial_Guinea, Eritrea, Estonia, Ethiopia, Fiji, Finland, France, Gabon, Gaza_Strip, The_Gambia, Georgia, Germany, Ghana, Greece, Grenada, Guatemala, GuineaBissau, Guyana, Haiti, Holy_Roman_Empire, Honduras, Hungary, Iceland, India, Indonesia, Iran, Iraq, Republic_of_Ireland, Israel, Italy, Ivory_Coast, Jamaica, Japan, Jonathanland, Jordan, Kazakhstan, Kenya, Kiribati, North_Korea, South_Korea, Kosovo, Kuwait, Kyrgyzstan, Laos, Latvia, Lebanon, Lesotho, Liberia, Libya, Liechtenstein, Lithuania, Luxembourg, Macedonia, Madagascar, Malawi, Malaysia, Maldives, Mali, Malta, Marshall_Islands, Mauritania, Mauritius, Mexico, Micronesia, Moldova, Monaco, Mongolia, Montenegro, Morocco, Mount_Athos, Mozambique, Namibia, Nauru, Nepal, Newfoundland, Netherlands, New_Zealand, Nicaragua, Niger, Nigeria, Norway, Oman, Ottoman_Empire, Pakistan, Palau, Panama, Papua_New_Guinea, Paraguay, Peru, Philippines, Poland, Portugal, Prussia, Qatar, Romania, Rome, Russian_Federation, Rwanda, St_Kitts_And_Nevis, St_Lucia, Saint_Vincent_And_the, Grenadines, Samoa, San_Marino, Sao_Tome_And_Principe, Saudi_Arabia, Senegal, Serbia, Seychelles, Sierra_Leone, Singapore, Slovakia, Slovenia, Solomon_Islands, Somalia, South_Africa, Spain, Sri_Lanka, Sudan, Suriname, Swaziland, Sweden, Switzerland, Syria, Tajikistan, Tanzania, Thailand, Togo, Tonga, Trinidad_And_Tobago, Tunisia, Turkey, Turkmenistan, Tuvalu, Uganda, Ukraine, UAE, UK, Uruguay, Uzbekistan, Vanuatu, Vatican_City, Venezuela, Vietnam, Yemen, Zambia, Zimbabwe
    }
}