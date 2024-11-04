using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IState
{
    private NPCController npc;
    private Vector3 alertPosition; // Posición donde se vio al jugador por última vez
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
        if (currentPath.Count == 0)
        {
            CalculatePathToAlertPosition();
        }
        else
        {
            MoveAlongPath();
        }

        // Verificar si el jugador está en el campo de visión
        if (IsPlayerInView(out Vector3 playerPosition))
        {
            // Cambiar a un estado de persecución o realizar alguna acción
            npc.ReceiveAlert(playerPosition);
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
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPosition, Time.deltaTime * npc.speed);

            if (Vector3.Distance(npc.transform.position, targetPosition) < 0.1f)
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
                playerPosition = collider.transform.position; // Devuelve la posición del jugador
                return true;
            }
        }

        playerPosition = Vector3.zero;
        return false;
    }

    public void Exit()
    {
        Debug.Log("Exiting Alert State");
    }
}
