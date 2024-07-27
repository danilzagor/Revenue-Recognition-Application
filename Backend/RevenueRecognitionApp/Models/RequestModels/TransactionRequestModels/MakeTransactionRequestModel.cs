namespace RevenueRecognition.RequestModels.TransactionRequestModels;

public class MakeTransactionRequestModel
{
    public decimal Amount { get; set; }
    
    public DateOnly TransactionDate { get; set; }
}