using ABIY_One.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABIY_One.Startup))]
namespace ABIY_One
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        //Default User Roles and Admin user for login
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Admin Role and default role
            if (!roleManager.RoleExists("Admin"))
            {
                //admin pool
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            //Admin super user
            var user = new ApplicationUser();
            user.UserName = "Digital";
            user.Email = "mk9digital13@gmail.com";

            string userPWD = "Digital@19";

            var chkUser = userManager.Create(user, userPWD);

            //Add default user to Role Admin
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "Admin");
            }

            //Manager Role
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }
        }

    }
}
