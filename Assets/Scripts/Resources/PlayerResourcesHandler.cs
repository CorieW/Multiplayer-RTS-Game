using System.Collections.Generic;
using Mirror;

public class PlayerResourcesHandler : NetworkBehaviour
{
    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>() 
    {
        { ResourceType.Food, 0 },
        { ResourceType.Wood, 0 },
        { ResourceType.Stone, 0 },
        { ResourceType.Gold, 0 }
    };

    
}