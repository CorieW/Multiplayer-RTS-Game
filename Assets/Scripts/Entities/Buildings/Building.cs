using System.Collections;
using System.Collections.Generic;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Building : PlayerEntity {

    [Header("References")]
    [SerializeField] protected Transform _unitSpawnPoint = null;

    [Header("Attributes")]
    [SerializeField] private List<Purchase> _purchases = new List<Purchase>();

    public static event Action<int, Purchase> ServerOnPurchase;
    public static event Action<Purchase> AuthorityOnPurchase;

    protected override void Awake()
    {
        base.Awake();

        _type = EntityType.Building;
    }

    #region Server

    [Command]
    private void CmdPurchase(int playerConnectionID, Purchase purchase)
    { // Called from the client-side version of this function
        // Player can not afford to purchase the unit, so return.
        if(!purchase.CanAfford(_owner.GetTotalResources())) return;

        ServerOnPurchase?.Invoke(playerConnectionID, purchase);

        if (purchase is UnitPurchase) HandleUnitPurchase(purchase as UnitPurchase);
    }

    [Server]
    private void HandleUnitPurchase(UnitPurchase unitPurchase)
    {
        Unit purchasedUnit = unitPurchase.GetUnit();
        purchasedUnit = Instantiate(purchasedUnit, _unitSpawnPoint.position, Quaternion.identity);

        NetworkServer.Spawn(purchasedUnit.gameObject, connectionToClient);
    }

    #endregion

    #region Client

    [Client]
    public void Purchase(Purchase purchase)
    {
        if(purchase.CanAfford(_owner.GetTotalResources())) //! I don't know whether this sends the type Purchase or the actual type.

        CmdPurchase(connectionToClient.connectionId, purchase);

        AuthorityOnPurchase?.Invoke(purchase);
    }

    #endregion
}
