using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPurchase : Purchase {

    [SerializeField] private Unit _unitPrefab;

    public Unit GetUnit()
    {
        return _unitPrefab;
    }

}