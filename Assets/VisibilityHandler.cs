using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(FogHandler))]
public class VisibilityHandler : MonoBehaviour
{
    private RTSPlayer _player;

    [Header("Dependencies")]
    [SerializeField] private FogHandler _fogHandler;

    private int[,] _visibilityMap;

    public int[,] visibilityMap { get { return _visibilityMap; } }

    void Start() 
    {
        _visibilityMap = new int[_fogHandler.fogSize.x, _fogHandler.fogSize.y];
    }

    void Update()
    {
        if (_player == null)
        {
            _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }
        else 
        {
            UpdateVisibilityMap();
        }
    }

    private void UpdateVisibilityMap()
    {
        // Resets the map so all visibility values provided by the code below is up to date.
        ResetMap();

        // The offset from that's applied to the tiles to have them centered.
        Vector2Int offset = new Vector2Int(_fogHandler.fogSize.x, _fogHandler.fogSize.y) / 2;

        foreach(PlayerObject playerObj in _player.GetPlayerObjects())
        {
            // The x and y index of the tile that the object is currently within.
            // + offset is applied to account for the centering of the chunks.
            // For example, the position (0, 0) is not the very first tile in the. 
            Vector2Int tileIndex = new Vector2Int(
                Mathf.FloorToInt(playerObj.transform.position.x),
                Mathf.FloorToInt(playerObj.transform.position.y)
            ) + offset;

            // Runs through the nearby tiles of the object and figures out if it's visible.
            // Todo: Change 5 (Fixed radius) to use a radius variable from the player object.
            for (int x = -5; x < 5; x++)
            {
                for (int y = -5; y < 5; y++)
                {
                    // Pythagoras theorem used to check whether tile is within radius.
                    float distance = Mathf.Sqrt((x * x) + (y * y));

                    // Tile is not visible
                    if (distance > 5) continue;

                    _visibilityMap
                        [
                            Mathf.Clamp(tileIndex.x + x, 0, _fogHandler.fogSize.x - 1),
                            Mathf.Clamp(tileIndex.y + y, 0, _fogHandler.fogSize.y - 1)
                        ] = 1;
                }
            }

        }
    }

    private void ResetMap()
    { // Resets all of the visibility values to 0 (not visible).
        for (int x = 0; x < _fogHandler.fogSize.x; x++)
        {
            for (int y = 0; y < _fogHandler.fogSize.y; y++)
            {
                _visibilityMap[x, y] = 0;
            }
        }
    }
}
