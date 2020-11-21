using System.Collections;
using System.Collections.Generic;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Building : PlayerObject
{
    [SerializeField] private GameObject _unitPrefab = null;
    [SerializeField] private Transform _unitSpawnPoint = null;

    #region Server

    [Command]
    private void CmdSpawnUnit() 
    {
        GameObject unitInstance = Instantiate(_unitPrefab, _unitSpawnPoint.position, _unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region Client

    #endregion
}
