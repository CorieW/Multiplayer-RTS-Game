using Mirror;
using UnityEngine;

public abstract class RTSObject : NetworkBehaviour {
    [Header("References")]
    [SerializeField] protected SpriteRenderer _minimapIcon;

    protected RTSObjectType _type;

    protected virtual void Awake()
    {

    }

    protected virtual void Start() 
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y);
    }

    public RTSObjectType GetRTSObjectType() 
    {
        return _type;
    }
}
public enum RTSObjectType {
    Unit, Building, ResourceDeposit
}