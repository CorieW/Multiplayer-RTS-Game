using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{

    [SerializeField] private Resources _resources;
    [SerializeField] private List<PlayerObject> _objects = new List<PlayerObject>();

    public Resources GetResources()
    {
        return _resources;
    }

    public List<PlayerObject> GetPlayerObjects()
    {
        return _objects;
    }

    #region Server

    public override void OnStartServer()
    {
        // Subscribing to events
        Unit.ServerOnPlayerObjectSpawned += ServerHandlePlayerObjectSpawned;
        Unit.ServerOnPlayerObjectDespawned += ServerHandlePlayerObjectDespawned;
    }

    public override void OnStopServer()
    {
        // Unsubscribing to events
        Unit.ServerOnPlayerObjectSpawned -= ServerHandlePlayerObjectSpawned;
        Unit.ServerOnPlayerObjectDespawned -= ServerHandlePlayerObjectDespawned;
    }

    private void ServerHandlePlayerObjectSpawned(PlayerObject playerObj)
    {
        if (playerObj.connectionToClient.connectionId != connectionToClient.connectionId) return;

        _objects.Add(playerObj);
    }

    private void ServerHandlePlayerObjectDespawned(PlayerObject playerObj)
    {
        if (playerObj.connectionToClient.connectionId != connectionToClient.connectionId) return;

        _objects.Remove(playerObj);
    }

    #endregion

    #region Client

    [Client]
    private void Start()
    {
        if (!hasAuthority) return;

        Camera.main.transform.position = transform.position;
    }

    public override void OnStartClient()
    {
        // Without the below check, there would be a duplicate list
        if (!isClientOnly) return;

        Unit.AuthorityOnPlayerObjectSpawned += AuthorityHandlePlayerObjectSpawned;
        Unit.AuthorityOnPlayerObjectDespawned += AuthorityHandlePlayerObjectDespawned;
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        Unit.AuthorityOnPlayerObjectSpawned -= AuthorityHandlePlayerObjectSpawned;
        Unit.AuthorityOnPlayerObjectDespawned -= AuthorityHandlePlayerObjectDespawned;
    }

    private void AuthorityHandlePlayerObjectSpawned(PlayerObject playerObj)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        _objects.Add(playerObj);
    }

    private void AuthorityHandlePlayerObjectDespawned(PlayerObject playerObj)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        _objects.Remove(playerObj);
    }

    #endregion
}
