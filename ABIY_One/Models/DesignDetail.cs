using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public class DesignDetail
    {
        [Key]
        public int DesignDetailId { get; set; }
        [Display(Name = "Design Area")]
        public string DesignAreaId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Design Instructions")]
        public string DesignDescription { get; set; }
        public virtual DesignArea DesignArea { get; set; }

    }
}