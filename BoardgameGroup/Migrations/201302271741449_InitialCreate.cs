namespace BoardgameGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Klubbs",
                c => new
                    {
                        klubbID = c.Int(nullable: false, identity: true),
                        klubbNavn = c.String(),
                        klubbURL = c.String(),
                        Spiller_spillerID = c.Int(),
                    })
                .PrimaryKey(t => t.klubbID)
                .ForeignKey("dbo.Spillers", t => t.Spiller_spillerID)
                .Index(t => t.Spiller_spillerID);
            
            CreateTable(
                "dbo.Spillers",
                c => new
                    {
                        spillerID = c.Int(nullable: false, identity: true),
                        fornavn = c.String(),
                        etternavn = c.String(),
                        epost = c.String(),
                        phone = c.String(),
                        mobile = c.String(),
                        facebookID = c.String(),
                    })
                .PrimaryKey(t => t.spillerID);
            
            CreateTable(
                "dbo.Spillsesjons",
                c => new
                    {
                        spillsesjonID = c.Int(nullable: false, identity: true),
                        spillID = c.Int(nullable: false),
                        klubbID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.spillsesjonID)
                .ForeignKey("dbo.Spills", t => t.spillID, cascadeDelete: true)
                .ForeignKey("dbo.Klubbs", t => t.klubbID, cascadeDelete: true)
                .Index(t => t.spillID)
                .Index(t => t.klubbID);
            
            CreateTable(
                "dbo.Spills",
                c => new
                    {
                        spillID = c.Int(nullable: false, identity: true),
                        navn = c.String(),
                        boardgamegeekID = c.String(),
                        spillerantall = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.spillID);
            
            CreateTable(
                "dbo.Spilldeltakelses",
                c => new
                    {
                        spilldeltakelseID = c.Int(nullable: false, identity: true),
                        posisjon = c.Int(nullable: false),
                        deltakerID = c.Int(nullable: false),
                        Spillsesjon_spillsesjonID = c.Int(),
                    })
                .PrimaryKey(t => t.spilldeltakelseID)
                .ForeignKey("dbo.Spillers", t => t.deltakerID, cascadeDelete: true)
                .ForeignKey("dbo.Spillsesjons", t => t.Spillsesjon_spillsesjonID)
                .Index(t => t.deltakerID)
                .Index(t => t.Spillsesjon_spillsesjonID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Spilldeltakelses", new[] { "Spillsesjon_spillsesjonID" });
            DropIndex("dbo.Spilldeltakelses", new[] { "deltakerID" });
            DropIndex("dbo.Spillsesjons", new[] { "klubbID" });
            DropIndex("dbo.Spillsesjons", new[] { "spillID" });
            DropIndex("dbo.Klubbs", new[] { "Spiller_spillerID" });
            DropForeignKey("dbo.Spilldeltakelses", "Spillsesjon_spillsesjonID", "dbo.Spillsesjons");
            DropForeignKey("dbo.Spilldeltakelses", "deltakerID", "dbo.Spillers");
            DropForeignKey("dbo.Spillsesjons", "klubbID", "dbo.Klubbs");
            DropForeignKey("dbo.Spillsesjons", "spillID", "dbo.Spills");
            DropForeignKey("dbo.Klubbs", "Spiller_spillerID", "dbo.Spillers");
            DropTable("dbo.Spilldeltakelses");
            DropTable("dbo.Spills");
            DropTable("dbo.Spillsesjons");
            DropTable("dbo.Spillers");
            DropTable("dbo.Klubbs");
        }
    }
}
