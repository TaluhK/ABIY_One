using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ABIY_One.Models.Data_Models
{
    public class OrderVM
    {
        public int SupplierId { get; set; }
        [DisplayName("Expected Date")]
        public System.DateTime OrderDate { get; set; }
        [DisplayName("Date Created")]
        public System.DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}