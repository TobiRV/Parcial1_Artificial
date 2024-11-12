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
        // Verificar si el jugador está en el campo de visión
        if (npc.IsPlayerInView(out Vector3 playerPosition))
        {
            // Si el jugador está en vista, cambiar al estado de persecución
            npc.ChangeState(new PursuitState(npc, playerPosition, pathfinding));
            return;  // Salir del estado de alerta
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
            currentPath.RemoveAt(0);  // Eliminar el primer nodo (el nodo inicial)
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
                currentPath.RemoveAt(0);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Alert State");
    }
}
