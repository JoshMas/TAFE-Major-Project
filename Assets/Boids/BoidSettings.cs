using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    public float minSpeed, maxSpeed;
    public float perceptionRadius, avoidanceRadius;
    public LayerMask boidMask;
    public float maxSteerForce;
    [Space]
    public float alignWeight;
    public float cohesionWeight;
    public float separateWeight; 
    public float targetWeight;
    [Space]
    public LayerMask collisionMask;
    public float boundsRadius;
    public float avoidCollisionWeight;
    public float avoidCollisionDistance;
}