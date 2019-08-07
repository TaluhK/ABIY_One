namespace ABIY_One.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aweh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Designs",
                c => new
                    {
                        DesignId = c.Int(nullable: false, identity: true),
                        DesignTypeId = c.Int(nullable: false),
                        DesignName = c.String(nullable: false),
                        DesignImage = c.Binary(),
                        DesignPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DesignId)
                .ForeignKey("dbo.DesignTypes", t => t.DesignTypeId, cascadeDelete: true)
                .Index(t => t.DesignTypeId);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        SizeId = c.Int(nullable: false, identity: true),
                        SizeName = c.String(),
                    })
                .PrimaryKey(t => t.SizeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Designs", "DesignTypeId", "dbo.DesignTypes");
            DropIndex("dbo.Designs", new[] { "DesignTypeId" });
            DropTable("dbo.Sizes");
            DropTable("dbo.Designs");
        }
    }
}
