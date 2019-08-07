using ABIY_One.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public partial class StockOrder
    {
        public StockOrder()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int OrderID { get; set; }

        [DisplayName("Expected Date")]
        public System.DateTime OrderDate { get; set; }
        [DisplayName("Date Created")]
        public System.DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public virtual Supplier Suppliers { get; set; }
        [ForeignKey("Supplier")]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}