namespace PassionProjectN01661067.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foriegnKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SupplierId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropColumn("dbo.Products", "SupplierId");
        }
    }
}
