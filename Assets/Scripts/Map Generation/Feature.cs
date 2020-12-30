
using UnityEngine;

[System.Serializable]
public class Feature
{
    [SerializeField] private GameObject _gameObject;

    [Space]

    [Tooltip("The radius that other features can't spawn within from this feature.")]
    [SerializeField] private float _radius;
    [Range(0, 100)]
    [SerializeField] private float _spawnChance;

    public GameObject gameObject { get { return _gameObject; } }
    public float radius { get { return _radius; } }
    public float spawnChance { get { return _spawnChance / 100; } }

}