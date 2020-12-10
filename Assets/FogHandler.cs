using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VisibilityHandler))]
public class FogHandler : MonoBehaviour
{
    // The extra fog that should be applied to hide features.
    public static Vector2Int fogExtra = new Vector2Int(5, 5);
    public static Vector2Int fogSize = new Vector2Int(MapGenerator.mapSize + fogExtra.x, MapGenerator.mapSize + fogExtra.y);

    [Header("Dependencies")]
    [SerializeField] private VisibilityHandler _visibilityHandler;

    private int[,] _fogMap;

    public int[,] fogMap { get { return _fogMap; } }

    void Start()
    {
        if(!_visibilityHandler) _visibilityHandler = GetComponent<VisibilityHandler>();

        _fogMap = new int[MapGenerator.mapSize + fogExtra.x, MapGenerator.mapSize + fogExtra.y];
    }

    void Update()
    {
        ApplyVisibleMap(_visibilityHandler.visibilityMap);
    }

    private void ApplyVisibleMap(int[,] visibilityMap)
    {
        for (int x = 0; x < MapGenerator.mapSize + fogExtra.x; x++)
        {
            for (int y = 0; y < MapGenerator.mapSize + fogExtra.y; y++)
            {
                // Any value of 0 on the fog map can be altered by the value of the visibility map.
                // Tiles already cleared of fog can't have fog again.
                _fogMap[x, y] = _fogMap[x, y] == 0 ? visibilityMap[x, y] : 0;
            }
        }
    }
}
