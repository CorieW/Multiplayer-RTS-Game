using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerObject : RTSObject {
    protected RTSPlayer _owner;

    [SyncVar] [SerializeField] protected float _health;

    [SerializeField] private UnityEvent _onSelected = null;
    [SerializeField] private UnityEvent _onDeselected = null;

    public static event Action<PlayerObject> ServerOnPlayerObjectSpawned;
    public static event Action<PlayerObject> ServerOnPlayerObjectDespawned;

    public static event Action<PlayerObject> AuthorityOnPlayerObjectSpawned;
    public static event Action<PlayerObject> AuthorityOnPlayerObjectDespawned;

    public float GetHealth()
    {
        return _health;
    }

    public RTSPlayer GetOwner()
    {
        return _owner;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnPlayerObjectSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnPlayerObjectDespawned?.Invoke(this);
    }

    [Command]
    public void CmdDamage(float damage)
    { // Deal damage to the player object
        if (_health - damage > 0)
        {
            _health -= damage;
        }
        else {
            NetworkServer.Destroy(gameObject);
        }
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