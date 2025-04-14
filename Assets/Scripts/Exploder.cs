using UnityEngine;
using System.Collections.Generic;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _explosionRadius = 5f;

    public void Explode(Vector3 explosionCenter, List<Rigidbody> rigidbodies)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            float distance = Vector3.Distance(explosionCenter, rigidbody.transform.position);

            if (distance <= _explosionRadius)
            {
                rigidbody.AddExplosionForce(_explosionForce, explosionCenter, _explosionRadius, 0, ForceMode.Impulse);
            }
        }
    }
}