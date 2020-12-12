using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FogHandler))]
public class FogTextureHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private FogHandler _fogHandler;

    [Header("Attributes")]
    [SerializeField] private Color _fogColor;
    [Tooltip("Time it takes for the fog to fade out.")]
    [SerializeField] private float _fadeTime = 0.5f;

    void Start()
    {
        if (!_sr) _sr = GetComponent<SpriteRenderer>();
        if (!_fogHandler) _fogHandler = GetComponent<FogHandler>();

        SetupFog();
    }

    void Update()
    {
        UpdateFog();
    }

    public void SetupFog()
    { // Creates a fog sprite.
        Texture2D texture = new Texture2D(_fogHandler.fogSize.x, _fogHandler.fogSize.y);
        texture.filterMode = FilterMode.Point;

        Color[] colors = new Color[texture.width * texture.height];
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++) 
            {
                colors[(x * texture.height) + y] = _fogColor;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);
        _sr.sprite = sprite;
    }

    public void UpdateFog()
    { // Updates the fog sprite.
        Texture2D texture = _sr.sprite.texture;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++) 
            {
                // Tile is now visible and wasn't before.
                if (_fogHandler.fogMap[x, y] == 1 && texture.GetPixel(x, y) != new Color(_fogColor.r, _fogColor.g, _fogColor.b, 0))
                { // Set tile to visible
                    Color currentColor = texture.GetPixel(x, y);
                    // Fades the fog out over the course of 1 second.
                    texture.SetPixel(x, y, new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - (_fogColor.a * (Time.deltaTime * (1 / _fadeTime)))));
                }
            }
        }
        texture.Apply();
    }
}
