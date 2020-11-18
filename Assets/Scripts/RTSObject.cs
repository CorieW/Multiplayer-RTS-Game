using Mirror;
using UnityEngine;

public abstract class RTSObject : NetworkBehaviour {
    protected RTSObjectType _type;

    protected void Start() 
    {
        transform.localPosition = PositionCorrector.CorrectPosition(transform.position);
    }

    public RTSObjectType GetRTSObjectType() 
    {
        return _type;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
public enum RTSObjectType {
    Unit, Building, ResourceDeposit
}