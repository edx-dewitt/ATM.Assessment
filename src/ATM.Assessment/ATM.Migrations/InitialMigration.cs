using FluentMigrator;

namespace ATM.Migrations
{
    [Migration(202308091001)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Accounts")
                .WithColumn("AccountId").AsInt32().PrimaryKey().Identity()
                .WithColumn("AccountType").AsString(50).NotNullable()
                .WithColumn("Balance").AsDecimal(18, 2).NotNullable();
            
            
                Create.Table("Transactions")
                    .WithColumn("TransactionId").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AccountId").AsInt32().NotNullable()
                    .WithColumn("TransactionType").AsString(50).NotNullable()
                    .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Timestamp").AsDateTime().NotNullable();
                
                Insert.IntoTable("Accounts").Row(new
                {
                    AccountType = "Checking",
                    Balance = 0.00m
                });
            
                Insert.IntoTable("Accounts").Row(new
                {
                    AccountType = "Savings",
                    Balance = 0.00m
                });
        }

        public override void Down()
        {
            Delete.Table("Accounts");
            Delete.Table("Transactions");
        }
    }
}