using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BiomeZoneGenerator), typeof(MapGenerator))]
public class Map : NetworkBehaviour
{
    public const int TILE_SIZE = 32;
    public const int CHUNK_SIZE = 16;

    [Header("Dependencies")]
    [SerializeField] private BiomeZoneGenerator _biomeZoneGenerator;
    [SerializeField] private MapGenerator _mapGenerator;

    // Attributes
    [Header("Map Properties")]
    [SerializeField] private Vector2Int _mapChunks;

    private Zone[,] _zoneMap;

    public Vector2Int mapChunks { get { return new Vector2Int(_mapChunks.x, _mapChunks.y); } }
    public Vector2Int mapSize { get { return new Vector2Int(_mapChunks.x * CHUNK_SIZE, _mapChunks.y * CHUNK_SIZE); } }
    public Zone[,] zoneMap { get { return _zoneMap; } }

    private void Awake()
    {
        if (!_biomeZoneGenerator) _biomeZoneGenerator = GetComponent<BiomeZoneGenerator>();
        if (!_mapGenerator) _mapGenerator = GetComponent<MapGenerator>();
    }

    private void Start()
    {
        // The below code is used for testing the generation.
        if (SceneManager.GetActiveScene().name != "MapGenerationTestScene") return;
        
        BiomeZone[,] biomeZoneMap = _biomeZoneGenerator.GenerateBiomeZoneMap();
        _zoneMap = biomeZoneMap;

        _mapGenerator.GenerateFeatures(biomeZoneMap);
        _mapGenerator.GenerateChunks(biomeZoneMap);
    }

    #region Server

    public override void OnStartServer()
    {
        base.OnStartServer();

        BiomeZone[,] biomeZoneMap = _biomeZoneGenerator.GenerateBiomeZoneMap();
        _zoneMap = biomeZoneMap;

        _mapGenerator.GenerateFeatures(biomeZoneMap);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        base.OnStartClient();

        BiomeZone[,] biomeZoneMap = _biomeZoneGenerator.GenerateBiomeZoneMap();
        _zoneMap = biomeZoneMap;

        _mapGenerator.GenerateChunks(biomeZoneMap);
    }

    #endregion
}