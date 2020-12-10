 using Mirror;
using UnityEngine;

public class ResourceDeposit : RTSObject {

    [SyncVar] [SerializeField] private float _maxHealth;
    [SyncVar] [SerializeField] private float _currentHealth;

    [Space]

    [SerializeField] private ResourceDrop _resourceDrop;

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    #region Server

    [Command(ignoreAuthority = true)]
    public void CmdGatherResource(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0) DestroyDeposit();
    }

    [Server]
    private void DestroyDeposit()
    {
        // Create Drop
        GameObject drop = Instantiate(_resourceDrop, transform.position, Quaternion.identity).gameObject; //? I don't know whether this will just create a gameobject with Resource on it, without a sr.
        NetworkServer.Spawn(drop);
        // Destroy Deposit
        NetworkServer.Destroy(gameObject); //? I don't know whether this will destroy on server as well.
    }

    #endregion

    #region Client



    #endregion
}