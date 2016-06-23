namespace Fq.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.g_inventory", "TransitonId", c => c.Long());
            CreateIndex("dbo.g_inventory", "TransitonId");
            AddForeignKey("dbo.g_inventory", "TransitonId", "dbo.g_transition", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.g_inventory", "TransitonId", "dbo.g_transition");
            DropIndex("dbo.g_inventory", new[] { "TransitonId" });
            DropColumn("dbo.g_inventory", "TransitonId");
        }
    }
}
