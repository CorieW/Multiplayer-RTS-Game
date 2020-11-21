using System;
using Mirror;
using UnityEngine;

public class Resource : MonoBehaviour {

    [SerializeField] private ResourceType _type;

    public static event Action<Resource> ServerOnAddResourceToCollection;
    public static event Action<Resource> ServerOnTakeResourceFromCollection;

    public static event Action<Resource> AuthorityOnAddResourceToCollection;
    public static event Action<Resource> AuthorityOnTakeResourceFromCollection;

    public ResourceType GetResourceType()
    {
        return _type;
    }

    #region Server

    [Command]
    private void CmdAddResourceToCollection()
    { // Ran from the client version of this method
        ServerOnAddResourceToCollection?.Invoke(this);
    }

    [Command]
    private void CmdTakeResourceFromCollection()
    { // Ran from the client version of this method
        ServerOnTakeResourceFromCollection?.Invoke(this);
    }

    #endregion

    #region Client

    [Client]
    public void AddResourceToCollection()
    { // Add resource for the client
        AuthorityOnAddResourceToCollection?.Invoke(this);

        // Add resource for the server as well, so the server can keep track of the player
        // This is so the server can verify that the resources of the player are correct and no cheating is involved.
        CmdAddResourceToCollection();
    }

    [Client]
    public void TakeResourceFromCollection()
    { // Add resource for the client
        AuthorityOnTakeResourceFromCollection?.Invoke(this);

        // Add resource for the server as well, so the server can keep track of the player
        // This is so the server can verify that the resources of the player are correct and no cheating is involved.
        CmdTakeResourceFromCollection();
    }

    #endregion
}

public enum ResourceType {
    Food, Wood, Stone, Gold
}