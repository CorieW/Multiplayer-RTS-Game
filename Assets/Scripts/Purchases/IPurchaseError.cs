public interface IPurchaseError
{
    PurchaseErrorType GetPurchaseErrorType();
    string GetMessage();
}