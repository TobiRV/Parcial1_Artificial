using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boid : MonoBehaviour, IAgent
{
    [Header("Ranges")]
    [SerializeField] float visionRange = 10f;
    [SerializeField] float separationRange = 2f;
    [SerializeField] float foodDetectionRange = 5f;
    [SerializeField] float evadeRange = 15f;
    [SerializeField] float alignmentRange = 10f;
    [SerializeField] float cohesionRange = 10f;

    [Header("Speed")]
    [SerializeField] private float _speed = 5f;
    [SerializeField, Range(0, 1)] private float _rotationSpeed = 0.002f;

    [Header("Weights")] 
    [SerializeField] private float separationWeight = 1.5f;
    [SerializeField] private float alignmentWeight = 1.0f;
    [SerializeField] private float cohesionWeight = 1.0f;


    private IFood _currentFood; //almacenar la comida actual
    public Vector3 Position => transform.position;
    public Vector3 Velocity => velocity;
    private Vector3 velocity;


    void Start()
    {
        velocity = Vector3.zero;
    }
    void Update()
    {
        UpdateAgent();
    }
    public void UpdateAgent()
    {
        Debug.Log(_currentFood);
        Debug.Log("Updating Boid");
        //velocity = Vector3.zero;

        // Verifica si hay un depredador cercano
        IAgent predator = FindNearbyPredators();
        if (predator != null)
        {
            ApplyEvade(predator); // Evasión si hay un depredador
        }
        else
        {
            ApplyFlocking(FindNearbyBoids()); // Comportamiento de flocking
            ApplyArrive(FindFood()); // Movimiento hacia la comida
        }

        ApplyFlocking(FindNearbyBoids()); // Comportamiento de flocking
        ApplyArrive(FindFood()); // Movimiento hacia la comida

        LimitVelocity();
        velocity.y = 0;
        transform.position += velocity * Time.deltaTime;

        transform.forward = transform.forward * (1 - _rotationSpeed) + velocity.normalized * _rotationSpeed;
        GameManager.instance.ShiftPositionOnBounds(transform);
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
                _currentFood = food; // Guarda la referencia a la comida
                //Debug.Log("Comida encontrada en: " + food.Position);
                return food.Position;
            }
        }
        _currentFood = null; // Resetea la referencia si no se encuentra comida
        Debug.Log("No se encontró comida");
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
                // Verifica si el collider pertenece al cazador
                if (collider.CompareTag("Cazador")) 
                {
                    //Debug.Log("Cazador detectado: " + predator.Position);
                    return predator;
                }
            }
        }
        Debug.Log("No predators found");
        return null;
    }


    public void ApplyFlocking(IEnumerable<IAgent> neighbors)
    {
        // Implementa la lógica de flocking 
        Vector3 separationForce = ApplySeparation(neighbors) * separationWeight;
        Vector3 alignmentForce = ApplyAlignment(neighbors) * alignmentWeight;
        Vector3 cohesionForce = ApplyCohesion(neighbors) * cohesionWeight;

        // Suma las fuerzas aplicando los pesos
        velocity += separationForce + alignmentForce + cohesionForce;
    }

    private Vector3 ApplySeparation(IEnumerable<IAgent> neighbors)
    {
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        foreach (IAgent neighbor in neighbors)
        {
            Vector3 directionToNeighbor = transform.position - neighbor.Position;
            float distance = directionToNeighbor.magnitude;

            if (distance > 0 && distance < separationRange)
            {
                separationForce += directionToNeighbor.normalized / distance; // Fuerza inversamente proporcional a la distancia
                count++;
            }
        }
        return separationForce.normalized * _speed;
    }

    private Vector3 ApplyAlignment(IEnumerable<IAgent> neighbors)
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
        }

        return alignmentForce;
    }


    private Vector3 ApplyCohesion(IEnumerable<IAgent> neighbors)
    {
        Vector3 cohesionForce = Vector3.zero;
        int count = 0;

        foreach (IAgent neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.Position);

            if (distance > 0 && distance < cohesionRange)
            {
                cohesionForce += neighbor.Position;
                count++;
            }
        }

        if (count > 0)
        {
            cohesionForce /= count; // Calcula el centro de masa
            Vector3 directionToCenter = cohesionForce - transform.position;
            return directionToCenter.normalized * _speed;
        }

        return Vector3.zero;
    }

    public void ApplyArrive(Vector3 target)
    {
        if (target == Vector3.zero) return; // Si no hay comida, no hacemos nada

        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;

        // Fuerza de Llegada
        if (distance < 1f)
        {
            // Desacelerar si está cerca de la comida
            velocity += direction.normalized * (distance / 0.5f) * _speed * Time.deltaTime;

            // Consumir comida al llegar
            if (_currentFood != null)
            {
                _currentFood.Consume(); // Llama a Consume para destruir la comida
                _currentFood = null; // Resetea la referencia
            }
        }
        else
        {
            // Mueve el Boid hacia la comida
            velocity += direction.normalized * _speed * Time.deltaTime;
        }
    }

    public void ApplyEvade(IAgent predator)
    {
        if (predator != null)
        {
            Vector3 directionAway = transform.position - predator.Position;
            velocity += directionAway.normalized * (_speed * 1.5f); // Aumenta la fuerza de evasión
        }


    }
    private void LimitVelocity()
    {
        if (velocity.magnitude > _speed)
        {
            velocity = velocity.normalized * _speed; // Limita la velocidad
        }
    }
    public void MoveTo(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        velocity = direction * _speed;
        transform.position += velocity * Time.deltaTime; 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
