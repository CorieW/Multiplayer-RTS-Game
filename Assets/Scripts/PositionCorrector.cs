using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionCorrector {
    public static Vector3 CorrectPosition(Vector3 position) 
    {
        return new Vector3(position.x, position.y, position.y);
    }

    public static Vector3 CorrectPosition(Vector2 position) 
    {
        return new Vector3(position.x, position.y, position.y);
    }
}