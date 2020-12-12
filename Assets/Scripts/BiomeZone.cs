using UnityEngine;

[System.Serializable]
public class BiomeZone : Zone
{
    [SerializeField] private string _name;
    [SerializeField] private Color _groundColor;
    [SerializeField] private Feature[] _features;
    [Range(0, 100)]
    [Tooltip("Inclusive")]
    [SerializeField] private float _nutrientMax;
    [Range(0, 100)]
    [Tooltip("Inclusive")]
    [SerializeField] private float _nutrientMin;

    public Color groundColor { get { return _groundColor; } }
    public Feature[] features { get { return _features; } }
    public float nutrientMax { get { return _nutrientMax / 100; } }
    public float nutrientMin { get { return _nutrientMin / 100; } }

    public bool CanSpawn(float nutrients)
    {
        return nutrients >= nutrientMin && nutrients <= nutrientMax;
    }
}