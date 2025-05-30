namespace ATM.Data.Models;

public class Account
{
    public int AccountId { get; set; }
    public string AccountType { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
