using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerObject : RTSObject {
    protected RTSPlayer owner;

    [SerializeField] private UnityEvent _onSelected = null;
    [SerializeField] private UnityEvent _onDeselected = null;

    public static event Action<PlayerObject> ServerOnPlayerObjectSpawned;
    public static event Action<PlayerObject> ServerOnPlayerObjectDespawned;

    public static event Action<PlayerObject> AuthorityOnPlayerObjectSpawned;
    public static event Action<PlayerObject> AuthorityOnPlayerObjectDespawned;

    #region Server

    public override void OnStartServer()
    {
        ServerOnPlayerObjectSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnPlayerObjectDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        AuthorityOnPlayerObjectSpawned?.Invoke(this);
    }

    public override void OnStopAuthority()
    {
         AuthorityOnPlayerObjectDespawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;

        _onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;

        _onDeselected?.Invoke();
    }

    #endregion
}