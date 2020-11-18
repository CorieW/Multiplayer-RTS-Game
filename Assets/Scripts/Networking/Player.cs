using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour 
{

    [SyncVar] [SerializeField] private string _displayName = "Missing Name";

    [SerializeField] private ResourceCollection _resourceCollection;

    public string GetDisplayName()
    {
        return _displayName;
    }

    public ResourceCollection GetResourceCollection()
    {
        return _resourceCollection;
    }

    #region Server


    #endregion

    #region Client

    #endregion
}