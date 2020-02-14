namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedCustomer : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Customers(Name, IsSubcribedToNewsletter, MembershipTypeId) VALUES ('John Smith', 0, 1)");
            Sql("INSERT INTO Customers(Name, IsSubcribedToNewsletter, MembershipTypeId) VALUES ('Mary William', 1, 2)");
        }
        
        public override void Down()
        {
        }
    }
}
