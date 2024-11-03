using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CazadorIA : MonoBehaviour, IAgent
{
    public List<Transform> waypoints;
    public LayerMask birdMask;
    public LayerMask obstacleMask;
    public Transform target;
   


    public FSM<IAStates, CazadorIA> fsm;

    public enum IAStates
    {
        Rest,
        Patrol,
        CHASE,
    }


    public float speed;
    private Vector3 velocity; 

    public Vector3 Position => transform.position; 
    public Vector3 Velocity => velocity; 

    void Awake()
    {
        fsm = new FSM<IAStates, CazadorIA>()

            .AddState(IAStates.Rest, new Rest())
            .AddState(IAStates.Patrol, new Patrol())
            .AddState(IAStates.CHASE, new Chase());

        fsm.Done(this);

        fsm.ChangeState(IAStates.Patrol);
    }

    void Update()
    {
        fsm.OnUpdate();
    }
    public void UpdateAgent()
    {
        // Aquí puedes implementar la lógica que necesites para el cazador
        // Por ejemplo, moverse hacia el waypoint o detectar Boids
    }

    public void ApplyFlocking(IEnumerable<IAgent> neighbors)
    {
        // Si el cazador no necesita flocking, puedes dejarlo vacío o lanzar una excepción
    }

    public void ApplyArrive(Vector3 target)
    {
        // Implementa la lógica para llegar a un objetivo si es necesario
    }

    public void ApplyEvade(IAgent predator)
    {
        // Implementa la lógica de evasión si es necesario
    }

    public void MoveTo(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        velocity = direction * speed;
        transform.position += velocity * Time.deltaTime; // Mover el cazador
    }
}
