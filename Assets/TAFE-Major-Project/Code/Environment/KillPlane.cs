using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] private float damage = 100;


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<Health>().UpdateHealth(-damage);
    }
}
