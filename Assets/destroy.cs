using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    
    public float speed = 8f;
    [SerializeField] private float damage = 1;
    private Rigidbody bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out Health health))
        {
            health.UpdateHealth(-damage);
            Destroy(gameObject);
        }
    }
}