using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerEntity : Entity {

    protected RTSPlayer _owner;

    [Header("Attributes")]
    [SyncVar] [SerializeField] protected float _maxHealth;
    [SyncVar] [SerializeField] protected float _currentHealth;

    [Space]

    [SerializeField] private UnityEvent _onSelected = null;
    [SerializeField] private UnityEvent _onDeselected = null;

    public static event Action<PlayerEntity> ServerOnPlayerEntitySpawned;
    public static event Action<PlayerEntity> ServerOnPlayerEntityDespawned;

    public static event Action<PlayerEntity> AuthorityOnPlayerEntitySpawned;
    public static event Action<PlayerEntity> AuthorityOnPlayerEntityDespawned;

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    public RTSPlayer GetOwner()
    {
        return _owner;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnPlayerEntitySpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnPlayerEntityDespawned?.Invoke(this);
    }

    [Command]
    public void CmdDamage(float damage)
    { // Deal damage to the player object
        if (_currentHealth - damage > 0)
        {
            _currentHealth -= damage;
        }
        else {
            NetworkServer.Destroy(gameObject);
        }
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        ColorRepresentMinimapIcon();
    }

    public override void OnStartAuthority()
    {
        AuthorityOnPlayerEntitySpawned?.Invoke(this);
    }

    public override void OnStopAuthority()
    {
         AuthorityOnPlayerEntityDespawned?.Invoke(this);
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

    [Client]
    private void ColorRepresentMinimapIcon()
    { // Changing color of minimap icon to represent whether the object is friendly or not.
        if (hasAuthority) _minimapIcon.color = GlobalVariables.ownedColor;
        else _minimapIcon.color = GlobalVariables.enemyColor;
    }

    #endregion
}