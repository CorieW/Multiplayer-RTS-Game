[System.Serializable]
public class ResourceStack
{
    private ResourceType _resourceType;
    private int _amount;

    public ResourceType GetResourceType()
    {
        return _resourceType;
    }

    public int GetAmount()
    {
        return _amount;
    }

    public ResourceStack()
    {
        _resourceType = ResourceType.Food;
        _amount = 0;
    }

    public ResourceStack(ResourceType resourceType, int amount)
    {
        _resourceType = resourceType;
        _amount = amount;
    }

    public void CreateDrop()
    { // Creates a drop out of the resource stack

    }
}