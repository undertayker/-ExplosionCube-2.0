using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _splitChance = 1f;
    [SerializeField] private float _reductionChance = 0.5f;
    [SerializeField] private float _scaleReduction = 2f;

    private float _baseExplosionForce = 10f;
    private float _baseExplosionRadius = 5f;
    private float _currentSplitChance;

    private Spawner _spawner;

    private void Awake()
    {
        _currentSplitChance = _splitChance;
    }

    private void OnMouseDown()
    {
        if (ShouldSplit())
        {
            _spawner.SpawnChildCubes(this);
        }
        else
        {
            ExplodeCurrentCube();
        }

        Destroy(gameObject);
    }

    public void Initialize(Spawner spawner, float splitChance)
    {
        _spawner = spawner;
        _splitChance = splitChance;
    }

    public float GetSplitChance()
    {
        return _splitChance;
    }

    public float GetReductionChance()
    {
        return _reductionChance;
    }

    public float GetScaleReduction()
    {
        return _scaleReduction;
    }

    public void SetSpawner(Spawner spawner)
    {
        _spawner = spawner;
    }

    private bool ShouldSplit()
    {
        return Random.value <= _splitChance;
    }

    private void ExplodeCurrentCube()
    {
        float sizeFactor = 1f / transform.localScale.x;
        float explosionForce = _baseExplosionForce * sizeFactor;
        float explosionRadius = _baseExplosionRadius * sizeFactor;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        List<Rigidbody> affectedRigidbodies = new List<Rigidbody>();

        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody != null && hit.gameObject != gameObject)
            {
                affectedRigidbodies.Add(hit.attachedRigidbody);
            }
        }

        foreach (Rigidbody rigidbody in affectedRigidbodies)
        {
            float distance = Vector3.Distance(transform.position, rigidbody.position);
            float relativeForce = 1f - Mathf.Clamp01(distance / explosionRadius);

            rigidbody.AddExplosionForce(
                explosionForce * relativeForce,
                transform.position,
                explosionRadius,
                0f,
                ForceMode.Impulse
            );
        }
    }
}