using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ResourceDrop : Entity {

    // ! [SerializeField] [SyncVar] private Unit _hauler; // ! Doesn't work for whatever reason (Unsupported type error).

    [SyncVar] [SerializeField] private ResourceStack _resourceStack;

    public ResourceStack resourceStack { get { return _resourceStack; } }

    protected override void Awake()
    {
        base.Awake();

        _type = EntityType.ResourceDrop;
    }

    #region Server

    [Server]
    public void CmdCreateDrop(ResourceStack resourceStack, Vector2 pos)
    {
        ResourceDrop drop = Instantiate(this, new Vector3(pos.x, pos.y), Quaternion.identity);
        drop.SetResourceStack(resourceStack);
        NetworkServer.Spawn(drop.gameObject);
    }

    [Server]
    public void SetResourceStack(ResourceStack resourceStack)
    {
        _resourceStack = resourceStack;
    }

    [Command(ignoreAuthority = true)]
    public void CmdHaulResource()
    { // Gets rid of the resource, because it's being hauled.
        NetworkServer.Destroy(gameObject);
    }

    #endregion
}