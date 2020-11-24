using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Purchase {
    [SerializeField] private Dictionary<ResourceType, int> _cost = new Dictionary<ResourceType, int>();

    public Dictionary<ResourceType, int> GetCost()
    {
        return _cost;
    }

    public bool CanAfford(Dictionary<ResourceType, int> resources)
    {
        foreach (ResourceType resType in resources.Keys)
        {
            if (_cost[resType] > resources[resType]) return false;
        }

        return true;
    }
}