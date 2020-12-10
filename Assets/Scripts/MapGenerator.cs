using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MapGenerator : NetworkBehaviour 
{
    public const int TILE_SIZE = 32;
    public const int CHUNK_SIZE = 16;
    public const int CHUNKS = 8;

    [Header("Perlin Noise Settings")]
    [SyncVar] [SerializeField] private int _groundMapSeed = 0;
    [SerializeField] private int _featureMapSeed = 0;

    [Header("Attributes")]
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private Color _grassColor;
    [SerializeField] private GameObject[] _features;

    public static int mapSize { get { return MapGenerator.CHUNKS * MapGenerator.CHUNK_SIZE; } }

    System.Random rnd = new System.Random();

    private void Start()
    { // Generate map
        GenerateChunks();
    }

    private void GenerateChunks() 
    {
        for (int x = -CHUNKS/2; x < CHUNKS/2; x++) {
            for (int y = -CHUNKS/2; y < CHUNKS/2; y++) {
                if (isClient) GenerateChunk(new Vector2(x, y));
                // Only spawns chunk features on the server, so that the features sync with client and server.
                if (isServer) GenerateChunkFeatures(new Vector2(x, y));
            }
        }
    }

    #region Server

    [Server]
    private void GenerateChunkFeatures(Vector2 chunkPos) 
    { // Generates all of the resources and environmental features
        float[,] noise = PerlinNoise.GeneratePerlinNoise(CHUNK_SIZE, CHUNK_SIZE, _featureMapSeed, 10, 6, chunkPos * CHUNK_SIZE, 0.5f, 2, 0, PerlinNoise.NormalizeMode.Global);

        for (int x = 0; x < CHUNK_SIZE; x++) {
            for (int y = 0; y < CHUNK_SIZE; y++) {
                foreach (GameObject feature in _features)
                {
                    if (noise[x, y] > 0.6f)
                    {
                        GameObject newFeature = Instantiate(feature, Vector3.zero, Quaternion.identity);
                        newFeature.transform.position = (chunkPos * CHUNK_SIZE) + new Vector2(x + 0.5f, y + Random.Range(0.00f, 0.5f)); // +0.5f to center the object in the grid cell.
                        // Just determines how big the tree is going to be, so 0.7 - 1.2, these numbers just work well for me.
                        float size = (float)rnd.Next(700, 1200) / 1000;
                        newFeature.transform.localScale = new Vector3(size, size, 1);

                        NetworkServer.Spawn(newFeature);

                        break;
                    }
                }
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        CmdRandomizeMap();
    }


    [Server]
    private void CmdRandomizeMap()
    {
        _groundMapSeed = Random.Range(0, int.MaxValue);
        _featureMapSeed = Random.Range(0, int.MaxValue);
    }

    #endregion

    #region Client

    [Client]
    private GameObject GenerateChunk(Vector2 chunkPos) 
    { // Generates the chunk and the chunk's floor texture - Server doesn't need to run this, because it provides no function, just aesthetics.
        GameObject newChunk = Instantiate(_chunkPrefab, Vector3.zero, Quaternion.identity);
        newChunk.transform.SetParent(transform);
        newChunk.transform.localPosition = new Vector3(chunkPos.x * CHUNK_SIZE, chunkPos.y * CHUNK_SIZE, chunkPos.y * CHUNK_SIZE);

        SpriteRenderer sr = newChunk.GetComponent<SpriteRenderer>();

        int pixelChunkSize = CHUNK_SIZE * TILE_SIZE; // The number of pixels along a single chunk axis
        Texture2D groundTexture = new Texture2D(pixelChunkSize, pixelChunkSize);
        Color[] colors = new Color[pixelChunkSize * pixelChunkSize];
        for (int x = 0; x < pixelChunkSize; x++) {
            for (int y = 0; y < pixelChunkSize; y++) {
                colors[(x * pixelChunkSize) + y] = _grassColor;
            }
        }
        groundTexture.SetPixels(colors);
        groundTexture.Apply();

        Sprite groundSprite = Sprite.Create(groundTexture, new Rect(0, 0, pixelChunkSize, pixelChunkSize), Vector2.zero, TILE_SIZE);
        sr.sprite = groundSprite;

        return newChunk;
    }

    #endregion
}