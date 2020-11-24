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

    private void AddResources(Dictionary<ResourceType, int> resources) 
    { // Returns the new resource quantity
        foreach (ResourceType resourceType in resources.Keys)
        {
            _resources[resourceType] += resources[resourceType];
        }
    }

    private void TakeResources(Dictionary<ResourceType, int> resources) 
    { // Returns the new resource quantity
        foreach (ResourceType resourceType in resources.Keys)
        {
            _resources[resourceType] -= resources[resourceType];
        }
    }

    #region Server

    public override void OnStartServer()
    {
        Resource.ServerOnAddResourceToCollection += ServerHandleAddResourcesToCollection;
        Resource.ServerOnTakeResourceFromCollection += ServerHandleTakeResourcesFromCollection;
        Building.ServerOnPurchase += ServerHandlePurchase;
    }

    public override void OnStopServer()
    {
        Resource.ServerOnAddResourceToCollection -= ServerHandleAddResourcesToCollection;
        Resource.ServerOnTakeResourceFromCollection -= ServerHandleTakeResourcesFromCollection;
        Building.ServerOnPurchase -= ServerHandlePurchase;
    }

    private void ServerHandleAddResourcesToCollection(int playerConnectionID, Dictionary<ResourceType, int> resources)
    {
        if (playerConnectionID != connectionToClient.connectionId) return;

        AddResources(resources);
    }

    private void ServerHandleTakeResourcesFromCollection(int playerConnectionID, Dictionary<ResourceType, int> resources)
    {
        if (playerConnectionID != connectionToClient.connectionId) return;

        TakeResources(resources);
    }

    private void ServerHandlePurchase(int playerConnectionID, Purchase purchase)
    {
        if (playerConnectionID != connectionToClient.connectionId) return;

        TakeResources(purchase.GetCost());
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly) return;

        Resource.AuthorityOnAddResourceToCollection += AuthorityHandleAddResourcesToCollection;
        Resource.AuthorityOnTakeResourceFromCollection += AuthorityHandleTakeResourcesFromCollection;
        Building.AuthorityOnPurchase += AuthorityHandlePurchase;
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        Resource.AuthorityOnAddResourceToCollection -= AuthorityHandleAddResourcesToCollection;
        Resource.AuthorityOnTakeResourceFromCollection -= AuthorityHandleTakeResourcesFromCollection;
        Building.AuthorityOnPurchase -= AuthorityHandlePurchase;
    }

    private void AuthorityHandleAddResourcesToCollection(Dictionary<ResourceType, int> resources)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        AddResources(resources);
    }

    private void AuthorityHandleTakeResourcesFromCollection(Dictionary<ResourceType, int> resources)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        TakeResources(resources);
    }

    private void AuthorityHandlePurchase(Purchase purchase)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        TakeResources(purchase.GetCost());
    }

    #endregion

}