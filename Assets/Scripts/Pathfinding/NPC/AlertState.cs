using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IState
{
    private NPCController npc;
    private Vector3 alertPosition;
    private Pathfinding pathfinding;
    private List<Node> currentPath;

    public AlertState(NPCController npc, Vector3 alertPosition, Pathfinding pathfinding)
    {
        this.npc = npc;
        this.alertPosition = alertPosition;
        this.pathfinding = pathfinding;
        this.currentPath = new List<Node>();
    }

    public void Enter()
    {
        Debug.Log("Entering Alert State");
        CalculatePathToAlertPosition();
    }

    public void Update()
    {
        if (npc.IsPlayerInView(out Vector3 playerPosition))
        {
            npc.ChangeState(new PursuitState(npc, playerPosition, pathfinding));
            return; 
        }

        if (currentPath.Count == 0)
        {
            CalculatePathToAlertPosition();
        }
        else
        {
            MoveAlongPath();
        }
    }

    private void CalculatePathToAlertPosition()
    {
        pathfinding.RequestPath(npc.transform.position, alertPosition, OnPathFound);
    }

    private void OnPathFound(List<Node> path)
    {
        currentPath = path;
        if (currentPath.Count > 0)
        {
            currentPath.RemoveAt(0); 
        }
    }

    private void MoveAlongPath()
    {
        if (currentPath.Count > 0)
        {
            Vector3 targetPosition = currentPath[0].transform.position;
            npc.RotateTowards(targetPosition);
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPosition, Time.deltaTime * npc.speed);

            if (Vector3.Distance(npc.transform.position, targetPosition) < 0.1f)
            {
                currentPath.RemoveAt(0); // Eliminamos el nodo al que hemos llegado

                // Si hemos llegado al destino de la alerta y no hay más nodos en el path
                if (currentPath.Count == 0)
                {
                    // Cambiamos al estado de patrullaje, y volvemos a hacer pathfinding hacia el siguiente nodo de patrullaje
                    npc.ChangeState(new PatrolState(npc, npc.pathfinding, npc.patrolNodes));
                }
            }
        }
        else
        {
            // Si no hay pathfinding, recalculamos el path hacia el destino de la alerta.
            CalculatePathToAlertPosition();
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Alert State");
    }
}
