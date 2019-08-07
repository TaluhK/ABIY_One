using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public class Size
    {
        [Key]

        public int SizeId { get; set; }
        public string SizeName { get; set; }
    }
}