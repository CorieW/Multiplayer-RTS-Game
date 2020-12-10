using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Resources : NetworkBehaviour
{
    Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>() 
    {
        {ResourceType.Food, 0},
        {ResourceType.Wood, 0},
        {ResourceType.Stone, 0},
        {ResourceType.Gold, 0}
    };

    public static event Action<Resources> OnResourcesChange;

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

        OnResourcesChange?.Invoke(this);
    }

    private void TakeResources(Dictionary<ResourceType, int> resources) 
    { // Returns the new resource quantity
        foreach (ResourceType resourceType in resources.Keys)
        {
            _resources[resourceType] -= resources[resourceType];
        }

        OnResourcesChange?.Invoke(this);
    }

    #region Server

    public override void OnStartServer()
    {
        ResourceDrop.ServerOnAddResourceToCollection += ServerHandleAddResourcesToCollection;
        ResourceDrop.ServerOnTakeResourceFromCollection += ServerHandleTakeResourcesFromCollection;
        Building.ServerOnPurchase += ServerHandlePurchase;
    }

    public override void OnStopServer()
    {
        ResourceDrop.ServerOnAddResourceToCollection -= ServerHandleAddResourcesToCollection;
        ResourceDrop.ServerOnTakeResourceFromCollection -= ServerHandleTakeResourcesFromCollection;
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

        ResourceDrop.AuthorityOnAddResourceToCollection += AuthorityHandleAddResourcesToCollection;
        ResourceDrop.AuthorityOnTakeResourceFromCollection += AuthorityHandleTakeResourcesFromCollection;
        Building.AuthorityOnPurchase += AuthorityHandlePurchase;
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        ResourceDrop.AuthorityOnAddResourceToCollection -= AuthorityHandleAddResourcesToCollection;
        ResourceDrop.AuthorityOnTakeResourceFromCollection -= AuthorityHandleTakeResourcesFromCollection;
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