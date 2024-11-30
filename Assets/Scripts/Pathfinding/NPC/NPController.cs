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

    public void ChangeState(IState newState)
    {
        StateMachine.ChangeState(newState);
    }

    public void ReceiveAlert(Vector3 playerPosition)
    {
        StateMachine.ChangeState(new AlertState(this, playerPosition, pathfinding));
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public bool IsPlayerInView(out Vector3 playerPosition)
    {
        float detectionRadius = 5f;
        LayerMask playerLayer = LayerMask.GetMask("Player");

        // Verifica si hay colisiones en el radio de detección
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        foreach (var collider in hitColliders)
        {
            if (collider != null)
            {
                // Verifica si el jugador está en el campo de visión
                bool inFieldOfView = Pathfinding.FieldOfView(transform, collider.transform, 45f);
                //Debug.Log($"Collider detectado: {collider.name}, en campo de visión: {inFieldOfView}");

                if (inFieldOfView)
                {
                    playerPosition = collider.transform.position;
                    return true;
                }
            }
        }


        playerPosition = Vector3.zero;
        return false;
    }

}
