public class InsufficientResourcesPurchaseError : IPurchaseError
{
    public PurchaseErrorType GetPurchaseErrorType()
    {
        return PurchaseErrorType.Insufficient_Resources;
    }

    public string GetMessage()
    {
        return "You don't have enough resources to purchase this!";
    }
}