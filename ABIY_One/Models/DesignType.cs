using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models
{
    public class DesignType
    {
        [Key]
        public int DesignTypeId { get; set; }
        [DisplayName("Design Type")]
        public string DesignTypeName { get; set; }
    }
}