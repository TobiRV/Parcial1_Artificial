using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boid : MonoBehaviour, IAgent
{
    [SerializeField] float visionRange = 10f;
    [SerializeField] float separationRange = 2f;
    [SerializeField] float foodDetectionRange = 5f;
    [SerializeField] float evadeRange = 15f;
    [SerializeField] float alignmentRange = 10f;
    [SerializeField] float cohesionRange = 10f;
    [SerializeField] private float _speed = 5f;

    public Vector3 Position => transform.position;
    public Vector3 Velocity => velocity;
    private Vector3 velocity;

    public void UpdateAgent()
    {
        // Actualiza el comportamiento del Boid.
        ApplyFlocking(FindNearbyBoids());
        ApplyArrive(FindFood());
        ApplyEvade(FindNearbyPredators());
    }
    private IEnumerable<IAgent> FindNearbyBoids()
    {
        List<IAgent> nearbyBoids = new List<IAgent>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionRange);

        foreach (var collider in colliders)
        {
            var boid = collider.GetComponent<IAgent>();
            if (boid != null && !ReferenceEquals(boid, this)) 
            {
                nearbyBoids.Add(boid);
            }
        }
        return nearbyBoids;
    }
    private Vector3 FindFood()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, foodDetectionRange);
        foreach (var collider in colliders)
        {
            IFood food = collider.GetComponent<IFood>();
            if (food != null)
            {
                return food.Position;
            }
        }
        return Vector3.zero;
    }
    private IAgent FindNearbyPredators()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, evadeRange);
        foreach (var collider in colliders)
        {
            var predator = collider.GetComponent<IAgent>();
            if (predator != null && !ReferenceEquals(predator, this)) 
            {
                return predator;
            }
        }
        return null;
    }


    public void ApplyFlocking(IEnumerable<IAgent> neighbors)
    {
        // Implementa la lógica de flocking 
        ApplySeparation(neighbors);
        ApplyAlignment(neighbors);
        ApplyCohesion(neighbors);
    }

    private void ApplySeparation(IEnumerable<IAgent> neighbors)
    {
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        foreach (IAgent neighbor in neighbors)
        {
            Vector3 directionToNeighbor = transform.position - neighbor.Position;
            float distance = directionToNeighbor.magnitude;

            
            if (distance > 0 && distance < separationRange)
            {
                
                separationForce += directionToNeighbor.normalized / distance;
                count++;
            }
        }

        if (count > 0)
        {
            separationForce /= count; 
            velocity += separationForce; 
        }
    }

    private void ApplyAlignment(IEnumerable<IAgent> neighbors)
    {
        Vector3 alignmentForce = Vector3.zero;
        int count = 0;

        foreach (IAgent neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.Position);

            // Aplica alineación solo si el vecino está dentro del rango de alineación
            if (distance > 0 && distance < alignmentRange)
            {
                alignmentForce += neighbor.Velocity; 
                count++;
            }
        }

        if (count > 0)
        {
            alignmentForce /= count; // Promedia las direcciones de alineación
            alignmentForce = alignmentForce.normalized * velocity.magnitude; 
            velocity += alignmentForce; 
        }
    }

    private void ApplyCohesion(IEnumerable<IAgent> neighbors)
    {
        Vector3 cohesionForce = Vector3.zero;
        int count = 0;

        foreach (IAgent neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.Position);

            // 
            if (distance > 0 && distance < cohesionRange)
            {
                cohesionForce += neighbor.Position;
                count++;
            }
        }

        if (count > 0)
        {
            cohesionForce /= count; // Calcula el centro de masa de los vecinos
            Vector3 directionToCenter = cohesionForce - transform.position; 
            velocity += directionToCenter.normalized; 
        }
    }

    public void ApplyArrive(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        // Mueve el boid hacia la comida
        if (direction.magnitude > 0.1f) 
        {
            transform.position += direction.normalized * _speed * Time.deltaTime;
        }
    }

    public void ApplyEvade(IAgent predator)
    {
        Vector3 directionAway = transform.position - predator.Position;
        transform.position += directionAway.normalized * _speed * Time.deltaTime;
    }

  
}
