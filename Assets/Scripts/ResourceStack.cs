using UnityEngine;

[System.Serializable]
public class ResourceStack
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _amount;

    public ResourceType resourceType { get { return _resourceType; } }
    public int amount { get { return _amount; } }

    public ResourceStack()
    {
        _resourceType = ResourceType.Food;
        _amount = 1;
    }

    public ResourceStack(ResourceType resourceType, int amount = 1)
    {
        _resourceType = resourceType;
        _amount = amount;
    }
}