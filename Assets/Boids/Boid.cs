using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private BoidSettings settings;

    private Vector3 velocity;
    private Transform target;

    [HideInInspector] public Vector3 forward;
    [HideInInspector] public Vector3 position;

    public void Initialise(Transform _target, BoidSettings _settings)
    {
        target = _target;
        settings = _settings;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    private void Update()
    {
        Vector3 acceleration = Vector3.zero;
        List<Boid> nearbyBoids = GetNearbyBoids();

        if(target != null)
        {
            Vector3 offsetToTarget = target.position - position;
            acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
        }

        if(nearbyBoids.Count > 0)
        {
            acceleration += SteerTowards(GetAverageAlignment(nearbyBoids)) * settings.alignWeight;
            acceleration += SteerTowards(GetDirectionToCentre(nearbyBoids)) * settings.cohesionWeight;
            acceleration += SteerTowards(GetAvoidanceDirection(nearbyBoids)) * settings.separateWeight;
        }

        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            acceleration += SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
        }

        velocity += acceleration * Time.deltaTime;

        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        transform.position += velocity * Time.deltaTime;
        position = transform.position;
        transform.forward = velocity.normalized;
        forward = transform.forward;
    }

    private Vector3 SteerTowards(Vector3 _vector)
    {
        Vector3 v = _vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }

    private List<Boid> GetNearbyBoids()
    {
        List<Boid> boids = new List<Boid>();
        Collider[] boidColliders = Physics.OverlapSphere(transform.position, settings.perceptionRadius, settings.boidMask, QueryTriggerInteraction.Collide);
        foreach(Collider boidCollider in boidColliders)
        {
            if(boidCollider.TryGetComponent(out Boid boid))
            {
                boids.Add(boid);
            }
        }
        return boids;
    }

    private Vector3 GetAverageAlignment(List<Boid> _boids)
    {
        Vector3 vector = Vector3.zero;
        foreach(Boid boid in _boids)
        {
            vector += boid.forward;
        }
        return vector;
    }

    private Vector3 GetDirectionToCentre(List<Boid> _boids)
    {
        Vector3 vector = Vector3.zero;
        foreach(Boid boid in _boids)
        {
            vector += boid.position;
        }
        vector /= _boids.Count;

        return vector - position;
    }

    private Vector3 GetAvoidanceDirection(List<Boid> _boids)
    {
        Vector3 vector = Vector3.zero;
        foreach(Boid boid in _boids)
        {
            Vector3 offset = boid.position - position;
            float distance = offset.magnitude;
            if (distance < settings.avoidanceRadius)
            {
                vector -= offset.normalized * (settings.avoidanceRadius - distance);
            }
        }
        return vector;
    }

    private bool IsHeadingForCollision()
    {
        return Physics.SphereCast(position, settings.boundsRadius, forward, out _, settings.avoidCollisionDistance, settings.collisionMask);
    }

    private Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; ++i)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if(!Physics.SphereCast(ray, settings.boundsRadius, settings.avoidCollisionDistance, settings.collisionMask))
            {
                return dir;
            }
        }

        return forward;
    }
}
