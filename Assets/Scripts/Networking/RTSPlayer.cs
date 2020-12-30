using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerResourcesHandler))]
public class RTSPlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerResourcesHandler _playerResourcesHandler;

    [Header("Attributes")]
    [SerializeField] private List<PlayerEntity> _entities = new List<PlayerEntity>();

    private void Awake()
    {
        if (_playerResourcesHandler == null) _playerResourcesHandler = GetComponent<PlayerResourcesHandler>();
    }

    public List<PlayerEntity> GetPlayerEntities()
    {
        return _entities;
    }

    public Dictionary<ResourceType, int> GetTotalResources()
    { // Todo: Complete
        return null;
    }

    #region Server

    public override void OnStartServer()
    {
        // Subscribing to events
        PlayerEntity.ServerOnPlayerEntitySpawned += ServerHandlePlayerEntitySpawned;
        PlayerEntity.ServerOnPlayerEntityDespawned += ServerHandlePlayerEntityDespawned;
    }

    public override void OnStopServer()
    {
        // Unsubscribing to events
        PlayerEntity.ServerOnPlayerEntitySpawned -= ServerHandlePlayerEntitySpawned;
        PlayerEntity.ServerOnPlayerEntityDespawned -= ServerHandlePlayerEntityDespawned;
    }

    private void ServerHandlePlayerEntitySpawned(PlayerEntity playerObj)
    {
        if (playerObj.connectionToClient.connectionId != connectionToClient.connectionId) return;

        _entities.Add(playerObj);
    }

    private void ServerHandlePlayerEntityDespawned(PlayerEntity playerObj)
    {
        if (playerObj.connectionToClient.connectionId != connectionToClient.connectionId) return;

        _entities.Remove(playerObj);
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

        PlayerEntity.AuthorityOnPlayerEntitySpawned += AuthorityHandlePlayerEntitySpawned;
        PlayerEntity.AuthorityOnPlayerEntityDespawned += AuthorityHandlePlayerEntityDespawned;
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        PlayerEntity.AuthorityOnPlayerEntitySpawned -= AuthorityHandlePlayerEntitySpawned;
        PlayerEntity.AuthorityOnPlayerEntityDespawned -= AuthorityHandlePlayerEntityDespawned;
    }

    private void AuthorityHandlePlayerEntitySpawned(PlayerEntity playerObj)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        _entities.Add(playerObj);
    }

    private void AuthorityHandlePlayerEntityDespawned(PlayerEntity playerObj)
    {
        // I don't know why the below line is required
        if (!hasAuthority) return;

        _entities.Remove(playerObj);
    }

    #endregion
}
