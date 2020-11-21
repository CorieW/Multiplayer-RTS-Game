using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Resources : NetworkBehaviour
{
    Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

    private void Start() 
    {
        _resources.Add(ResourceType.Food, 0);
        _resources.Add(ResourceType.Wood, 0);
        _resources.Add(ResourceType.Stone, 0);
        _resources.Add(ResourceType.Gold, 0);
    }

    public Dictionary<ResourceType, int> GetResources() 
    {
        return _resources;
    }

    public int GetResourceQuantity(ResourceType resourceType) 
    {
        return _resources[resourceType];
    }

    int AddResource(ResourceType resourceType, int quantity) 
    { // Returns the new resource quantity
        _resources[resourceType] += quantity;
        return _resources[resourceType];
    }

    int TakeResource(ResourceType resourceType, int quantity) 
    { // Returns the new resource quantity
        _resources[resourceType] -= quantity;
        return _resources[resourceType];
    }

    #region Server

    public override void OnStartServer()
    {
        Resource.ServerOnAddResourceToCollection += ServerHandleAddResourceToCollection;
        Resource.ServerOnTakeResourceFromCollection += ServerHandleTakeResourceFromCollection;
    }

    public override void OnStopServer()
    {
        Resource.ServerOnAddResourceToCollection -= ServerHandleAddResourceToCollection;
        Resource.ServerOnTakeResourceFromCollection -= ServerHandleTakeResourceFromCollection;
    }

    private void ServerHandleAddResourceToCollection(Resource resource)
    {
        AddResource(resource.GetResourceType(), 1);
    }

    private void ServerHandleTakeResourceFromCollection(Resource resource)
    {
        TakeResource(resource.GetResourceType(), 1);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly) return;

        Resource.AuthorityOnAddResourceToCollection += AuthorityHandleAddResourceToCollection;
        Resource.AuthorityOnTakeResourceFromCollection += AuthorityHandleTakeResourceFromCollection;
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        Resource.AuthorityOnAddResourceToCollection -= AuthorityHandleAddResourceToCollection;
        Resource.AuthorityOnTakeResourceFromCollection -= AuthorityHandleTakeResourceFromCollection;
    }

    private void AuthorityHandleAddResourceToCollection(Resource resource)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        AddResource(resource.GetResourceType(), 1);
    }

    private void AuthorityHandleTakeResourceFromCollection(Resource resource)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        TakeResource(resource.GetResourceType(), 1);
    }

    #endregion

}