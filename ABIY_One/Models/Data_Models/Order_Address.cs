using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABIY_One.Models.Data_Models
{
    public class Order_Address
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int address_id { get; set; }

        [ForeignKey("Order")]
        public int Order_ID { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public virtual Order Order { get; set; }
    }
}