using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Map))]
public class MapGenerator : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Map _map;

    [Header("References")]
    [SerializeField] private GameObject _chunkPrefab;

    Feature[,] _featureMap;

    System.Random rnd;

    private void Awake()
    {
        if (!_map) GetComponent<Map>();
    }

    public void GenerateChunks(BiomeZone[,] biomeZoneMap) 
    {
        for (int x = -_map.mapChunks.x/2; x < _map.mapChunks.x/2; x++) {
            for (int y = -_map.mapChunks.y/2; y < _map.mapChunks.y/2; y++) {
                GenerateChunk(new Vector2Int(x, y), biomeZoneMap);
            }
        }
    }

    public void GenerateFeatures(BiomeZone[,] biomeZoneMap)
    {
        rnd = new System.Random(Random.Range(0, int.MaxValue));
        _featureMap = new Feature[_map.mapSize.x, _map.mapSize.y];

        for (int x = -_map.mapChunks.x/2; x < _map.mapChunks.x/2; x++) {
            for (int y = -_map.mapChunks.y/2; y < _map.mapChunks.y/2; y++) {
                GenerateChunkFeatures(new Vector2Int(x, y), biomeZoneMap);
            }
        }
    }

    private void GenerateChunkFeatures(Vector2Int chunkPos, BiomeZone[,] biomeZoneMap) 
    { // Generates all of the resources and environmental features
        for (int x = 0; x < Map.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Map.CHUNK_SIZE; y++)
            {
                Vector2Int tilePos = new Vector2Int(
                    (Map.CHUNK_SIZE * (chunkPos.x + _map.mapChunks.x / 2)) + Mathf.FloorToInt(x),
                    (Map.CHUNK_SIZE * (chunkPos.y + _map.mapChunks.y / 2)) + Mathf.FloorToInt(y)
                );
                Vector2Int worldPos = new Vector2Int((Map.CHUNK_SIZE * chunkPos.x) + x, (Map.CHUNK_SIZE * chunkPos.y) + y);

                BiomeZone biomeZone = biomeZoneMap[tilePos.x, tilePos.y];

                float probability = (float)rnd.Next(1000000) / 1000000;
                float previousProb = 0;
                foreach (Feature feature in biomeZone.features)
                {
                    previousProb += feature.spawnChance;
                    bool probabilityMet = probability <= previousProb;

                    // Probability not met - continue.
                    if (!probabilityMet) continue;
                    if (!IsFeatureSpawnable(tilePos, feature.radius)) continue;

                    _featureMap[tilePos.x, tilePos.y] = feature;
                    GameObject newFeature = Instantiate(feature.gameObject, Vector3.zero, Quaternion.identity);
                    newFeature.transform.position = (chunkPos * Map.CHUNK_SIZE) + new Vector2(x + 0.5f, y + Random.Range(0.00f, 0.5f)); // +0.5f to center the object in the grid cell.
                    // Just determines how big the tree is going to be, so 0.7 - 1.2, these numbers just work well for me.
                    float size = (float)rnd.Next(900, 1500) / 1000;
                    newFeature.transform.localScale = new Vector3(size, size, 1);

                    NetworkServer.Spawn(newFeature);

                    break;
                }
            }
        }
    }

    private void SpawnManagedFeatures(Vector2Int chunkPos, BiomeZone[,] biomeZoneMap)
    {
        // Todo: Managed features are features that must spawn a specific number of times.
        // Todo: Managed features are good at ensuring the map is fair for all.
    }

    private bool IsFeatureSpawnable(Vector2Int tilePos, float radius)
    { // Checks whether the feature that is spawning in that position is occupying any other feature's radius
      // Also checks whether other objects occupy the new features radius. If either are true, the feature won't spawn.
        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                Vector2Int featurePos = new Vector2Int(tilePos.x + x, tilePos.y + y);
                Feature feature = _featureMap
                    [
                        Mathf.Clamp(featurePos.x, 0, _map.mapSize.x - 1),
                        Mathf.Clamp(featurePos.y, 0, _map.mapSize.y - 1)
                    ];
                
                if (feature == null) continue;

                // Pythagoras Theorem
                float distance = Mathf.Sqrt((x * x) + (y * y));

                if (distance <= radius) return false;
                if (distance <= feature.radius) return false;
            }
        }

        return true;
    }

    private GameObject GenerateChunk(Vector2Int chunkPos, BiomeZone[,] biomeZoneMap) 
    { // Generates the chunk and the chunk's floor texture - Server doesn't need to run this, because it provides no function, just aesthetics.
        GameObject newChunk = Instantiate(_chunkPrefab, Vector3.zero, Quaternion.identity);
        newChunk.transform.SetParent(transform);
        newChunk.transform.localPosition = new Vector3(chunkPos.x * Map.CHUNK_SIZE, chunkPos.y * Map.CHUNK_SIZE, chunkPos.y * Map.CHUNK_SIZE);

        SpriteRenderer sr = newChunk.GetComponent<SpriteRenderer>();

        int pixelChunkSize = Map.CHUNK_SIZE * Map.TILE_SIZE; // The number of pixels along a single chunk axis
        Texture2D groundTexture = new Texture2D(pixelChunkSize, pixelChunkSize);
        Color[] colors = new Color[pixelChunkSize * pixelChunkSize];
        for (int x = 0; x < pixelChunkSize; x++) {
            for (int y = 0; y < pixelChunkSize; y++) {
                Vector2Int tilePos = new Vector2Int(
                    (Map.CHUNK_SIZE * (chunkPos.x + _map.mapChunks.x / 2)) + Mathf.FloorToInt(x / Map.TILE_SIZE),
                    (Map.CHUNK_SIZE * (chunkPos.y + _map.mapChunks.y / 2)) + Mathf.FloorToInt(y / Map.TILE_SIZE)
                );

                colors[(x * pixelChunkSize) + y] = biomeZoneMap[tilePos.x, tilePos.y].groundColor;
            }
        }
        groundTexture.SetPixels(colors);
        groundTexture.Apply();

        Sprite groundSprite = Sprite.Create(groundTexture, new Rect(0, 0, pixelChunkSize, pixelChunkSize), Vector2.zero, Map.TILE_SIZE);
        sr.sprite = groundSprite;

        return newChunk;
    }

}