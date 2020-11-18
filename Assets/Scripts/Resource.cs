using Mirror;
using UnityEngine;

public class Resource : NetworkBehaviour 
{
    [SerializeField] private ResourceType _type;

    public ResourceType GetResourceType()
    {
        return _type;
    }
}