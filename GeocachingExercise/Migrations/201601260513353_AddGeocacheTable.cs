namespace GeocachingExercise.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGeocacheTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Geocaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Coordinate_Latitude = c.Double(nullable: false),
                        Coordinate_Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Geocaches");
        }
    }
}
