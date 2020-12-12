using Mirror;
using UnityEngine;

[RequireComponent(typeof(Map))]
public class BiomeZoneGenerator : NetworkBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Map _map;

    [Header("Generation Settings")]
    [Tooltip("These are the biome zones that will be used in the creation of the map.")]
    [SerializeField] private BiomeZone[] _biomeZones;

    [Space]

    [SyncVar] [SerializeField] private int _seed = 0;
    [Tooltip("A higher scale will result in larger zones being generated.")]
    [SerializeField] private float _scale;
    [Tooltip("More octaves results in more random/sharper shaped zones.")]
    [Range(1, 8)]
    [SerializeField] private int _octaves;

    private void Start()
    {
        if (!_map) _map = GetComponent<Map>();
    }

    public BiomeZone[,] GenerateBiomeZoneMap()
    {
        BiomeZone[,] biomeMap = new BiomeZone[_map.mapSize.x, _map.mapSize.y];
        float[,] nutrientMap = PerlinNoise.GeneratePerlinNoise(_map.mapSize.x, _map.mapSize.y, _seed, _scale, _octaves, Vector2.zero);
        gameObject.SetActive(true);
        for (int x = 0; x < _map.mapSize.x; x++)
        {
            for (int y = 0; y < _map.mapSize.y; y++)
            {
                foreach (BiomeZone biomeZone in _biomeZones)
                {
                    // Can't spawn this biome zone, continue to next biome zone.
                    if (!biomeZone.CanSpawn(nutrientMap[x, y])) continue;

                    biomeMap[x, y] = biomeZone;
                    break;
                }
            }
        }

        return biomeMap;
    }

    #region Server

    public override void OnStartServer()
    {
        GenerateRandomSeed();
    }

    [Server]
    private void GenerateRandomSeed()
    {
        _seed = Random.Range(0, int.MaxValue);
    }

    #endregion
}