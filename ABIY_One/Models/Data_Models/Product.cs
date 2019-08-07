using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABIY_One.Models.Data_Models
{
    public class Product
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemCode { get; set; }
        [Required]
        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int Category_ID { get; set; }

        //[ForeignKey("Supplier")]
        //[Display(Name = "Supplier")]
        //public int SupplierId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }
        [Display(Name = "Qty in Stock")]
        public int QuantityInStock { get; set; }
        //[Required]
        [Display(Name = "Picture")]
        //[DataType(DataType.Upload)]
        public byte[] Picture { get; set; }
        [Required]
        [Display(Name = " Selling Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name = "Mark Up Percentage")]
        public double MarkUpPercentage { get; set; }

        [Display(Name = "Cost Price")]
        public double CostPrice { get; set; }
        [Display(Name = "Expected Profit")]

        public double ExpectedProfit { get; set; }

        public double CalcExpctdProft()
        {
            return ExpectedProfit = ((CalcSellingPrice() - CostPrice) * QuantityInStock);
        }

        public double DividePercent()
        {
            return MarkUpPercentage / 100;
        }
        public double CalcSellingPrice()
        {
            return Price = CostPrice + (DividePercent() * CostPrice);
        }

        public virtual Category Category { get; set; }
        //public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Cart_Items> Cart_Items { get; set; }

    }
}