using Mirror;
using UnityEngine;

public abstract class Entity : NetworkBehaviour {
    [Header("References")]
    [SerializeField] protected SpriteRenderer _minimapIcon;

    protected EntityType _type;

    protected virtual void Awake()
    {

    }

    protected virtual void Start() 
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y);
    }

    public EntityType GetRTSObjectType() 
    {
        return _type;
    }
}
public enum EntityType {
    Unit, Building, ResourceDeposit, ResourceDrop
}