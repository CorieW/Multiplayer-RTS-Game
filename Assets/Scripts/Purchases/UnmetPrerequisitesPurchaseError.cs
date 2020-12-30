public class UnmetPrerequisitesPurchaseError : IPurchaseError
{
    public PurchaseErrorType GetPurchaseErrorType()
    {
        return PurchaseErrorType.Unmet_Prerequisites;
    }

    public string GetMessage()
    { // Todo: Add requirements
        return "You don't meet the requirements to ";
    }
}