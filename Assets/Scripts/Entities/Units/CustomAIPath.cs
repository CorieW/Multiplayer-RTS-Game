using Pathfinding;
using UnityEngine;

public class CustomAIPath : AIPath {

    // This package doesn't contain a way to copy the y-axis to the z-axis.
    // When updating the value regularly in Update to transform.position, it causes buggy behaviour.
    // This is because they use a cached transform, because transform.position is apparently slow.
    // Below I access the cached transform and copy the y-axis to the z-axis.
    protected override void Update() 
    {
        base.Update();

        tr.position = new Vector3(tr.position.x, tr.position.y, tr.position.y);
    }

}