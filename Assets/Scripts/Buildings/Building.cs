using System.Collections;
using System.Collections.Generic;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Building : PlayerObject {

    [Header("Attributes")]
    [SerializeField] private List<Purchase> _purchases = new List<Purchase>();
    [SerializeField] private Transform _unitSpawnPoint = null;

    public static event Action<int, Purchase> ServerOnPurchase;

    public static event Action<Purchase> AuthorityOnPurchase;

    #region Server

    [Command]
    private void CmdPurchase(int playerConnectionID, Purchase purchase)
    { // Called from the client-side version of this function
        if(purchase.CanAfford(_owner.GetResources().GetResources()))
        
        ServerOnPurchase?.Invoke(playerConnectionID, purchase);

        if (purchase is UnitPurchase)
        {
            Unit purchasedUnit = (purchase as UnitPurchase).GetUnit();
            purchasedUnit = Instantiate(purchasedUnit, _unitSpawnPoint.position, Quaternion.identity);

            NetworkServer.Spawn(purchasedUnit.gameObject, connectionToClient);
        }
    }

    #endregion

    #region Client

    public void Purchase(Purchase purchase)
    {
        if(purchase.CanAfford(_owner.GetResources().GetResources()))

        CmdPurchase(connectionToClient.connectionId, purchase);

        AuthorityOnPurchase?.Invoke(purchase);
    }

    #endregion
}
