using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABIY_One.Models.Data_Models
{
    public class Order
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_ID { get; set; }
        public DateTime date_created { get; set; }
        [ForeignKey("Customer")]
        public string Email { get; set; }
        public bool shipped { get; set; }
        public string status { get; set; }
        public bool packed { get; set; }
        public virtual ICollection<Order_Item> Order_Items { get; set; }
        public virtual ICollection<Order_Address> Order_Addresses { get; set; }
       // public virtual Customer Customer { get; set; }

      //  public virtual ICollection<Order_Tracking> Order_Tracking { get; set; }



        public virtual ApplicationUser Customer{ get; set; }
       // public string UserId { get; set; }
    }
}
