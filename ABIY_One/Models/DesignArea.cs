using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public class DesignArea
    {
        [Key]
        public int DesignAreaId { get; set; }
        [Display(Name ="Design Area")]
        public string AreaName { get; set; }
    }
}