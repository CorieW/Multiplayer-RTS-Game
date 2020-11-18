using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MapGenerator : NetworkBehaviour 
{
    const int TILE_SIZE = 32;
    const int CHUNK_SIZE = 16;
    const int CHUNKS = 8;

    [SerializeField] private Color grassColor;
    [SerializeField] private GameObject[] _features;

    System.Random rnd = new System.Random();

    #region Server

    [Server]
    public override void OnStartServer()
    { // Generate map
        base.OnStartServer();

        GenerateChunks();
    }

    [Server]
    private void GenerateChunks() 
    {
        for (int x = -CHUNKS/2; x < CHUNKS/2; x++) {
            for (int y = -CHUNKS/2; y < CHUNKS/2; y++) {
                GameObject chunk = GenerateChunk(new Vector2(x, y));
                GenerateChunkFeatures(chunk, new Vector2(x, y));
            }
        }
    }

    [Server]
    private GameObject GenerateChunk(Vector2 chunkPos) 
    { // Generates the chunk and the chunk's floor texture
        GameObject newChunk = new GameObject("Chunk (" + chunkPos.x + ", " + chunkPos.y + ")");
        newChunk.transform.SetParent(transform);
        newChunk.transform.localPosition = new Vector3(chunkPos.x * CHUNK_SIZE, chunkPos.y * CHUNK_SIZE, chunkPos.y * CHUNK_SIZE);

        SpriteRenderer sr = newChunk.AddComponent<SpriteRenderer>();
        sr.sortingOrder = -10;

        int pixelChunkSize = CHUNK_SIZE * TILE_SIZE; // The number of pixels along a single chunk axis
        Texture2D groundTexture = new Texture2D(pixelChunkSize, pixelChunkSize);
        Color[] colors = new Color[pixelChunkSize * pixelChunkSize];
        for (int x = 0; x < pixelChunkSize; x++) {
            for (int y = 0; y < pixelChunkSize; y++) {
                colors[(x * pixelChunkSize) + y] = grassColor;
            }
        }
        groundTexture.SetPixels(colors);
        groundTexture.Apply();

        Sprite groundSprite = Sprite.Create(groundTexture, new Rect(0, 0, pixelChunkSize, pixelChunkSize), Vector2.zero, TILE_SIZE);
        sr.sprite = groundSprite;

        return newChunk;
    }

    [Server]
    private void GenerateChunkFeatures(GameObject chunk, Vector2 chunkPos) 
    { // Generates all of the resources and environmental features
        float[,] noise = PerlinNoise.GeneratePerlinNoise(CHUNK_SIZE, CHUNK_SIZE, 309742, 10, 6, chunkPos * CHUNK_SIZE, 0.5f, 2, 0, PerlinNoise.NormalizeMode.Global);

        for (int x = 0; x < CHUNK_SIZE; x++) {
            for (int y = 0; y < CHUNK_SIZE; y++) {
                foreach (GameObject feature in _features)
                {
                    if (noise[x, y] > 0.6f)
                    {
                        GameObject newFeature = Instantiate(feature, Vector3.zero, Quaternion.identity);
                        newFeature.transform.SetParent(chunk.transform);
                        newFeature.transform.localPosition = new Vector2(x + 0.5f, y); // +0.5f to center the object in the grid cell.
                        float size = (float)rnd.Next(700, 1200) / 1000;
                        newFeature.transform.localScale = new Vector3(size, size, 1);
                        break;
                    }
                }
            }
        }
    }

    #endregion
}