using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Stockpile : Building
{
    public readonly SyncDictionary<ResourceType, int> resources = new SyncDictionary<ResourceType, int>() 
    {
        {ResourceType.Food, 0},
        {ResourceType.Wood, 0},
        {ResourceType.Stone, 0},
        {ResourceType.Gold, 0}
    };

    public static event Action<List<ResourceStack>> OnStockPileStore;
    public static event Action<List<ResourceStack>> OnStockPileTake;

    /// <summary>
    /// <para>Returns the quantity of the resource type inputted into the ResourceType param.</para>
    /// </summary>
    public int GetResourceQuantity(ResourceType resourceType) 
    {
        return resources[resourceType];
    }

    /// <summary>
    /// <para>Adds resources to the stockpile resources collection.</para>
    /// </summary>
    [Command]
    public void CmdStoreMultiple(List<ResourceStack> resourceStacks) 
    {
        foreach (ResourceType resourceType in resources.Keys)
        {
            foreach (ResourceStack resourceStack in resourceStacks) 
            {
                resources[resourceStack.resourceType] += resourceStack.amount;
            }
        }

        OnStockPileStore?.Invoke(resourceStacks);
    }

    /// <summary>
    /// <para>Adds the resource to the stockpile resources collection.</para>
    /// </summary>
    [Command]
    public void CmdStore(ResourceStack resourceStack) 
    {
        foreach (ResourceType resourceType in resources.Keys)
        {
            resources[resourceStack.resourceType] += resourceStack.amount;
        }

        OnStockPileStore?.Invoke(new List<ResourceStack> { resourceStack });
    }

    /// <summary>
    /// <para>Takes the resources from the stockpile resources collection.</para>
    /// </summary>
    [Command]
    public void CmdTakeMultiple(List<ResourceStack> resourceStacks) 
    {
        foreach (ResourceType resourceType in resources.Keys)
        {
            foreach (ResourceStack resourceStack in resourceStacks) 
            {
                resources[resourceStack.resourceType] -= resourceStack.amount;
            }
        }

        OnStockPileStore?.Invoke(resourceStacks);
    }

    /// <summary>
    /// <para>Takes the resource from the stockpile resources collection.</para>
    /// </summary>
    [Command]
    public void CmdTake(ResourceStack resourceStack) 
    {
        foreach (ResourceType resourceType in resources.Keys)
        {
            resources[resourceStack.resourceType] -= resourceStack.amount;
        }

        OnStockPileStore?.Invoke(new List<ResourceStack> { resourceStack });
    }
}
