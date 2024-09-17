using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boid : MonoBehaviour, IAgent
{
    [SerializeField] float visionRange = 10f;
    [SerializeField] float separationRange = 2f;
    [SerializeField] float foodDetectionRange = 5f;
    [SerializeField] float evadeRange = 15f;

    private Vector3 velocity;

    public void UpdateAgent()
    {
        // Actualiza el comportamiento del Boid.
        ApplyFlocking(FindNearbyBoids());
        ApplyArrive(FindFood());
        ApplyEvade(FindNearbyPredators());
    }

    public void ApplyFlocking(IEnumerable<IAgent> neighbors)
    {
        // Implementa la lógica de flocking (separación, alineación, cohesión).
        ApplySeparation(neighbors);
        ApplyAlignment(neighbors);
        ApplyCohesion(neighbors);
    }

    private void ApplySeparation(IEnumerable<IAgent> neighbors)
    {
       
    }

    private void ApplyAlignment(IEnumerable<IAgent> neighbors)
    {
        
    }

    private void ApplyCohesion(IEnumerable<IAgent> neighbors)
    {
       
    }

    public void ApplyArrive(Vector3 target)
    {
        
    }

    public void ApplyEvade(IAgent predator)
    {
        
    }

    private IEnumerable<IAgent> FindNearbyBoids()
    {
        
    }

    private IEnumerable<IAgent> FindNearbyPredators()
    {
       
    }

    private Vector3 FindFood()
    {
       
    }
}
