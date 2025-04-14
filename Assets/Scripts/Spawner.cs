using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Exploder), typeof(Colorizer))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    [SerializeField] private int _initialCubeCount = 3;
    [SerializeField] private float _initialSpawnRadius = 1f;
    [SerializeField] private int _minCountCubes = 2;
    [SerializeField] private int _maxCountCubes = 6;
    [SerializeField] private float _childSpawnRadius = 1f;

    private Colorizer _colorizer;

    private void Awake()
    {
        _colorizer = GetComponent<Colorizer>();
    }

    private void Start()
    {
        SpawnInitialCubes();
    }

    public void SpawnChildCubes(Cube parentCube)
    {
        int randomNumber = Random.Range(_minCountCubes, _maxCountCubes + 1);
        List<Rigidbody> newCubeRigidbodies = new List<Rigidbody>(); ;

        for (int i = 0; i < randomNumber; i++)
        {
            Vector3 spawnPosition = parentCube.transform.position + Random.insideUnitSphere * _childSpawnRadius;
            Vector3 newScale = parentCube.transform.localScale / parentCube.GetScaleReduction();
            float newSplitChance = parentCube.GetSplitChance() * parentCube.GetReductionChance();

            Cube newCube = SpawnCube(spawnPosition, newScale, newSplitChance);

            Rigidbody rigidbody = newCube.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                newCubeRigidbodies.Add(rigidbody);
            }
        }
    }

    private void SpawnInitialCubes()
    {
        for (int i = 0; i < _initialCubeCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * _initialSpawnRadius;
            SpawnCube(spawnPosition, Vector3.one, 1f);
        }
    }

    private Cube SpawnCube(Vector3 position, Vector3 scale, float splitChance)
    {
        Cube newCube = Instantiate(_cubePrefab, position, Quaternion.identity);
        newCube.transform.localScale = scale;

        newCube.Initialize(this, splitChance);
        newCube.SetSpawner(this);

        _colorizer.SetRandomColor(newCube.GetComponent<Renderer>());
        return newCube;
    }
}