using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace sait.domein.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Укажите Ваше имя и фамилию")]
       public string NamePers { get; set; }

        [Required(ErrorMessage = "Укажите ваш номер телефона")]
        public string Telephone { get; set; }


        [Required(ErrorMessage = "Укажите адрес доставки")]

        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Улица")]
        public string Line1 { get; set; }

        [Display(Name = "Дом")]
        public string Line2 { get; set; }

        [Display(Name = "Квартира/офис")]
        public string Line3 { get; set; }
        public bool GiftWrap { get; set; }
    }
}
