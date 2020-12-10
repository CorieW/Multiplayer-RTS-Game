using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ResourceDrop : NetworkBehaviour {

    // ! [SerializeField] [SyncVar] private Unit _hauler; // ! Doesn't work for whatever reason (Unsupported type error).
    [SerializeField] [SyncVar] private bool _hauled = false;

    [Space]

    [SerializeField] private ResourceType _type;
    [SerializeField] private int _amount;

    public bool isBeingHauled { get { return _hauled; } }

    public static event Action<int, Dictionary<ResourceType, int>> ServerOnAddResourceToCollection;
    public static event Action<int, Dictionary<ResourceType, int>> ServerOnTakeResourceFromCollection;

    public static event Action<Dictionary<ResourceType, int>> AuthorityOnAddResourceToCollection;
    public static event Action<Dictionary<ResourceType, int>> AuthorityOnTakeResourceFromCollection;

    /*public Unit GetHauler()
    {
        return _hauler;
    }*/

    public ResourceType GetResourceType()
    {
        return _type;
    }

    #region Server

    [Command]
    private void CmdAddResourceToCollection(int playerConnectionID)
    { // Called from the client version of this method
        ServerOnAddResourceToCollection?.Invoke(playerConnectionID, new Dictionary<ResourceType, int>() { { _type, _amount } });
    }

    [Command]
    private void CmdTakeResourceFromCollection(int playerConnectionID)
    { // Called from the client version of this method
        ServerOnTakeResourceFromCollection?.Invoke(playerConnectionID, new Dictionary<ResourceType, int>() { { _type, _amount } });
    }

    [Server]
    public void Haul(bool hauling)
    { // Begins the hauling of the resource drop.
        // Todo: Change hauling to delete the ResourceDrop and add a ResourceStack to the unit.
        _hauled = hauling;
    }

    #endregion

    #region Client

    [Client]
    public void AddResourceToCollection()
    { // Add resources for the client
        AuthorityOnAddResourceToCollection?.Invoke(new Dictionary<ResourceType, int>() { { _type, _amount } });

        // Add resources for the server as well, so the server can keep track of the player
        // This is so the server can verify that the resources of the player are correct and no cheating is involved.
        CmdAddResourceToCollection(connectionToClient.connectionId);
    }

    [Client]
    public void TakeResourceFromCollection()
    { // Add resources for the client
        AuthorityOnTakeResourceFromCollection?.Invoke(new Dictionary<ResourceType, int>() { { _type, _amount } });

        // Add resources for the server as well, so the server can keep track of the player
        // This is so the server can verify that the resources of the player are correct and no cheating is involved.
        CmdTakeResourceFromCollection(connectionToClient.connectionId);
    }

    #endregion
}