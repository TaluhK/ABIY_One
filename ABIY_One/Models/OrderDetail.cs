using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderItemsID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Comment { get; set; }


        public virtual StockOrder Order { get; set; }

    }
}