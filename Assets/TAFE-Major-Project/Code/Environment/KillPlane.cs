using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] private float damage = 100;


    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.root.GetComponent<Health>().UpdateHealth(-damage);
    }
}
