using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public class Design
    {
        [Key]
        public int DesignId { get; set; }
        public int DesignTypeId { get; set; }
        [Display(Name = "Design Name")]
        [Required]
        public string DesignName { get; set; }

        [DisplayName("Design Image")]
        [Display(Name = "Image")]
        public byte[] DesignImage { get; set; }

        [Display(Name = "Design Price")]
        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        [Range(1, 999999, ErrorMessage = "Price must be greater than 0.00")]
        public decimal DesignPrice { get; set; }
        public virtual DesignType DesignType { get; set; }

    }
}