using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ResourceCollection : NetworkBehaviour
{
    [SyncVar]
    Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

    public ResourceCollection() 
    {
        _resources.Add(ResourceType.Food, 0);
        _resources.Add(ResourceType.Wood, 0);
        _resources.Add(ResourceType.Stone, 0);
        _resources.Add(ResourceType.Gold, 0);
    }

    public Dictionary<ResourceType, int> GetResources(Resource resource) 
    {
        return _resources;
    }

    public int GetResourceQuantity(Resource resource) 
    {
        return _resources[resource.GetResourceType()];
    }

    int AddResource(Resource resource, int quantity) 
    { // Returns the new resource quantity
        _resources[resource.GetResourceType()] += quantity;
        return _resources[resource.GetResourceType()];
    }

    [Command]
    public Dictionary<ResourceType, int> CmdAddResources(Dictionary<Resource, int> resources) 
    { // Returns the new resources
        foreach(Resource resource in resources.Keys) 
        {
            AddResource(resource, resources[resource]);
        }
        return _resources;
    }

    int TakeResource(Resource resource, int quantity) 
    { // Returns the new resource quantity
        _resources[resource.GetResourceType()] -= quantity;
        return _resources[resource.GetResourceType()];
    }

    [Command]
    public Dictionary<ResourceType, int> CmdTakeResource(Dictionary<Resource, int> resources) 
    { // Returns the new resources
        foreach(Resource resource in resources.Keys) 
        {
            TakeResource(resource, resources[resource]);
        }
        return _resources;
    }
}