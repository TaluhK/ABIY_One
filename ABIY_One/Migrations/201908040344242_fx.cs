namespace ABIY_One.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cart_Items",
                c => new
                    {
                        cart_item_id = c.String(nullable: false, maxLength: 128),
                        cart_id = c.String(maxLength: 128),
                        item_id = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.cart_item_id)
                .ForeignKey("dbo.Carts", t => t.cart_id)
                .ForeignKey("dbo.Products", t => t.item_id, cascadeDelete: true)
                .Index(t => t.cart_id)
                .Index(t => t.item_id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        cart_id = c.String(nullable: false, maxLength: 128),
                        date_created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.cart_id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ItemCode = c.Int(nullable: false, identity: true),
                        Category_ID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 80),
                        Description = c.String(nullable: false, maxLength: 255),
                        QuantityInStock = c.Int(nullable: false),
                        Picture = c.Binary(),
                        Price = c.Double(nullable: false),
                        MarkUpPercentage = c.Double(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        ExpectedProfit = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ItemCode)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Category_ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Category_ID = c.Int(nullable: false, identity: true),
                        Category_Name = c.String(nullable: false, maxLength: 80),
                        Description = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Category_ID)
                .Index(t => t.Category_Name, unique: true, name: "Category_Index");
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 35),
                        LastName = c.String(nullable: false, maxLength: 35),
                        phone = c.String(nullable: false, maxLength: 10),
                        address = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Email);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Order_ID = c.Int(nullable: false, identity: true),
                        date_created = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 128),
                        shipped = c.Boolean(nullable: false),
                        status = c.String(),
                        packed = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Order_ID)
                .ForeignKey("dbo.Customers", t => t.Email)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.Email)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Order_Address",
                c => new
                    {
                        address_id = c.Int(nullable: false, identity: true),
                        Order_ID = c.Int(nullable: false),
                        street = c.String(),
                        city = c.String(),
                        zipcode = c.String(),
                    })
                .PrimaryKey(t => t.address_id)
                .ForeignKey("dbo.Orders", t => t.Order_ID, cascadeDelete: true)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.Order_Item",
                c => new
                    {
                        Order_Item_id = c.Int(nullable: false, identity: true),
                        Order_id = c.Int(nullable: false),
                        item_id = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        replied = c.Boolean(nullable: false),
                        date_replied = c.DateTime(),
                        accepted = c.Boolean(nullable: false),
                        date_accepted = c.DateTime(),
                        shipped = c.Boolean(nullable: false),
                        status = c.String(),
                        date_shipped = c.DateTime(),
                    })
                .PrimaryKey(t => t.Order_Item_id)
                .ForeignKey("dbo.Products", t => t.item_id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_id, cascadeDelete: true)
                .Index(t => t.Order_id)
                .Index(t => t.item_id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        payment_ID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        AmountPaid = c.Double(nullable: false),
                        PaymentFor = c.String(nullable: false),
                        PaymentMethod = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.payment_ID)
                .ForeignKey("dbo.Customers", t => t.Email, cascadeDelete: true)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderItemsID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ItemName = c.String(),
                        Quantity = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.OrderItemsID)
                .ForeignKey("dbo.StockOrders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.StockOrders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Description = c.String(),
                        SupplierId = c.Int(nullable: false),
                        Supplier_SupplierId = c.Int(),
                        Suppliers_SupplierId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_SupplierId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.Suppliers_SupplierId)
                .Index(t => t.SupplierId)
                .Index(t => t.Supplier_SupplierId)
                .Index(t => t.Suppliers_SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 35),
                        LastName = c.String(nullable: false, maxLength: 35),
                        Email = c.String(nullable: false),
                        phone = c.String(nullable: false, maxLength: 10),
                        address = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.SupplierId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.StockOrders", "Suppliers_SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.StockOrders", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.StockOrders", "Supplier_SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.StockOrders");
            DropForeignKey("dbo.Payments", "Email", "dbo.Customers");
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Order_Item", "Order_id", "dbo.Orders");
            DropForeignKey("dbo.Order_Item", "item_id", "dbo.Products");
            DropForeignKey("dbo.Order_Address", "Order_ID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Email", "dbo.Customers");
            DropForeignKey("dbo.Cart_Items", "item_id", "dbo.Products");
            DropForeignKey("dbo.Products", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Cart_Items", "cart_id", "dbo.Carts");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.StockOrders", new[] { "Suppliers_SupplierId" });
            DropIndex("dbo.StockOrders", new[] { "Supplier_SupplierId" });
            DropIndex("dbo.StockOrders", new[] { "SupplierId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Payments", new[] { "Email" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Order_Item", new[] { "item_id" });
            DropIndex("dbo.Order_Item", new[] { "Order_id" });
            DropIndex("dbo.Order_Address", new[] { "Order_ID" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "Email" });
            DropIndex("dbo.Categories", "Category_Index");
            DropIndex("dbo.Products", new[] { "Category_ID" });
            DropIndex("dbo.Cart_Items", new[] { "item_id" });
            DropIndex("dbo.Cart_Items", new[] { "cart_id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Suppliers");
            DropTable("dbo.StockOrders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Payments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Order_Item");
            DropTable("dbo.Order_Address");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Carts");
            DropTable("dbo.Cart_Items");
        }
    }
}
