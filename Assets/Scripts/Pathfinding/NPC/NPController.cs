using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }
    public Pathfinding pathfinding;
    public List<Node> patrolNodes;
    public float speed = 5f;

    private void Start()
    {
        StateMachine = new StateMachine();
        StateMachine.ChangeState(new PatrolState(this, pathfinding, patrolNodes));
    }

    private void Update()
    {
        StateMachine.Update();
    }

    // Cambiar el estado actual del NPC
    public void ChangeState(IState newState)
    {
        StateMachine.ChangeState(newState);
    }

    // Método para recibir la alerta del sistema y cambiar al estado de alerta
    public void ReceiveAlert(Vector3 playerPosition)
    {
        // Cambiar al estado de alerta
        StateMachine.ChangeState(new AlertState(this, playerPosition, pathfinding));
    }

    // Método para rotar el NPC hacia una dirección
    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // Verificar si el jugador está dentro del campo de visión
    public bool IsPlayerInView(out Vector3 playerPosition)
    {
        float detectionRadius = 5f;
        LayerMask playerLayer = LayerMask.GetMask("Player");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        foreach (var collider in hitColliders)
        {
            if (collider != null && Pathfinding.FieldOfView(transform, collider.transform, 45f))
            {
                playerPosition = collider.transform.position;
                return true;
            }
        }

        playerPosition = Vector3.zero;
        return false;
    }
}
