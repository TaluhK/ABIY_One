namespace ABIY_One.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DesignAreas",
                c => new
                    {
                        DesignAreaId = c.Int(nullable: false, identity: true),
                        AreaName = c.String(),
                    })
                .PrimaryKey(t => t.DesignAreaId);
            
            CreateTable(
                "dbo.DesignTypes",
                c => new
                    {
                        DesignTypeId = c.Int(nullable: false, identity: true),
                        DesignTypeName = c.String(),
                    })
                .PrimaryKey(t => t.DesignTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DesignTypes");
            DropTable("dbo.DesignAreas");
        }
    }
}
