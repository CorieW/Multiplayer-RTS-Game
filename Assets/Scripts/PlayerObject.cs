using UnityEngine;

public abstract class PlayerObject : RTSObject 
{
    protected Player owner;

    public Player GetOwner()
    {
        return owner;
    }
}