namespace ATM.Data.Models;

public class TransactionRecord
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}

