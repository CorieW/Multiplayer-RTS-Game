 using Mirror;
using UnityEngine;

public class ResourceDeposit : Entity {

    [Header("References")]
    [SerializeField] private ResourceDrop _drop;

    [Header("Attributes")]
    [SyncVar] [SerializeField] private float _maxHealth;
    [SyncVar] [SerializeField] private float _currentHealth;

    [Space]

    [SerializeField] private ResourceStack _resourceStack;

    protected override void Awake()
    {
        base.Awake();

        _type = EntityType.ResourceDeposit;
    }

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
        _drop.CmdCreateDrop(_resourceStack, transform.position);
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client



    #endregion
}