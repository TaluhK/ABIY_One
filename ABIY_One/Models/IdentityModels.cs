using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ABIY_One.Models.Data_Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ABIY_One.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Fist Name must be atleast 3 characters long", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Fist Name must be atleast 3 characters long", MinimumLength = 3)]
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ABIY_DB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        //Temporal Shopping
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Items> Cart_Items { get; set; }
        //Shopping
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Order_Address> Order_Addresses { get; set; }
        //
        public DbSet<Payment> Payments { get; set; }
        //
        public DbSet<Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<ABIY_One.Models.Data_Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<ABIY_One.Models.OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<ABIY_One.Models.StockOrder> StockOrders { get; set; }
        //Designs
        public System.Data.Entity.DbSet<ABIY_One.Models.DesignArea> DesignAreas { get; set; }

        public System.Data.Entity.DbSet<ABIY_One.Models.DesignType> DesignTypes { get; set; }

        public System.Data.Entity.DbSet<ABIY_One.Models.Size> Sizes { get; set; }
        public System.Data.Entity.DbSet<ABIY_One.Models.Design> Designs { get; set; }

    }
}