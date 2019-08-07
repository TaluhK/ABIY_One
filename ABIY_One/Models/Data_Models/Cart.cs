using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models.Data_Models
{
    public class Cart
    {
        [Key]
        public string cart_id { get; set; }
        public DateTime date_created { get; set; }

        public virtual ICollection<Cart_Items> Cart_Items { get; set; }

    }
}