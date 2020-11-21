using Mirror;
using UnityEngine;

public class ResourceDeposit : RTSObject {

    [SyncVar] [SerializeField] private float _health;
    [SerializeField] private Resource _resource;

    public float GetHealth()
    {
        return _health;
    }

    #region Server

    [Command]
    public void CmdGatherResource(float damage)
    {
        _health -= damage;

        if (_health <= 0) DestroyDeposit();
    }

    [Server]
    public void DestroyDeposit()
    {
        // Create Drop
        GameObject drop = Instantiate(_resource, transform.position, Quaternion.identity).gameObject; //? I don't know whether this will just create a gameobject with Resource on it, without a sr.
        NetworkServer.Spawn(drop);
        // Destroy Deposit
        NetworkServer.Destroy(gameObject); //? I don't know whether this will destroy on server as well.
    }

    #endregion

    #region Client



    #endregion
}