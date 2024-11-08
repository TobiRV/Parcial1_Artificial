using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitState : IState
{
    private NPCController npc;
    private Vector3 targetPosition;
    private Pathfinding pathfinding;
    private List<Node> currentPath;

    public PursuitState(NPCController npc, Vector3 targetPosition, Pathfinding pathfinding)
    {
        this.npc = npc;
        this.targetPosition = targetPosition;
        this.pathfinding = pathfinding;
        this.currentPath = new List<Node>();
    }

    public void Enter()
    {
        Debug.Log("Entering Pursuit State");
        CalculatePathToPlayer();
    }

    public void Update()
    {
        if (IsPlayerInView(out Vector3 playerPosition))
        {
            targetPosition = playerPosition; // Actualizar la posici�n del objetivo si el jugador est� visible
            MoveAlongPath();
        }
        else
        {
            npc.ChangeState(new PatrolState(npc, pathfinding, npc.patrolNodes)); // Cambia a su estado de patrullaje
        }
    }

    private void CalculatePathToPlayer()
    {
        pathfinding.RequestPath(npc.transform.position, targetPosition, OnPathFound);
    }

    private void OnPathFound(List<Node> path)
    {
        currentPath = path;
        if (currentPath.Count > 0)
        {
            currentPath.RemoveAt(0); // Remove the first node as we already are at that position
        }
    }

    private void MoveAlongPath()
    {
        if (currentPath.Count > 0)
        {
            Vector3 nextPosition = currentPath[0].transform.position;
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, nextPosition, Time.deltaTime * npc.speed);

            if (Vector3.Distance(npc.transform.position, nextPosition) < 0.1f)
            {
                currentPath.RemoveAt(0);
            }
        }
    }

    private bool IsPlayerInView(out Vector3 playerPosition)
    {
        float detectionRadius = 5f;
        LayerMask playerLayer = LayerMask.GetMask("PlayerLayer"); 

        Collider[] hitColliders = Physics.OverlapSphere(npc.transform.position, detectionRadius, playerLayer);
        foreach (var collider in hitColliders)
        {
            if (collider != null && Pathfinding.FieldOfView(npc.transform, collider.transform, 45f)) 
            {
                playerPosition = collider.transform.position; // Devuelve la posici�n del jugador
                return true;
            }
        }

        playerPosition = Vector3.zero; // Devuelve Vector3.zero si no hay jugador
        return false;
    }

    public void Exit()
    {
        Debug.Log("Exiting Pursuit State");
    }
}
