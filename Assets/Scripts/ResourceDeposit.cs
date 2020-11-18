using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ResourceDeposit : RTSObject
{

    [SyncVar] [SerializeField] private float _health;

    [SerializeField] private Resource _resource;

    public float GetHealth()
    {
        return _health;
    }

    #region Server

    [Server]
    private void DropResource()
    {
        Resource drop = Instantiate(_resource, transform.position, Quaternion.identity);
        // NetworkServer.Spawn(drop.gameObject);
    }

    [Command]
    private void CmdDamageResource(float damage)
    { // Damages the resource on the server
        if (_health - damage <= 0) 
        {
            Destroy(gameObject);

            return;
        }

        _health -= damage;
    }

    #endregion

    #region Client

    [Client]
    public void DamageResource(float damage)
    { // Damages the resource
        CmdDamageResource(damage);
    }

    #endregion
}