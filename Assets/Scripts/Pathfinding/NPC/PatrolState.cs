using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private NPCController npc;
    private Pathfinding pathfinding;
    private List<Node> patrolNodes;
    private int currentNodeIndex;
    private List<Node> currentPath;

    public PatrolState(NPCController npc, Pathfinding pathfinding, List<Node> patrolNodes)
    {
        this.npc = npc;
        this.pathfinding = pathfinding;
        this.patrolNodes = patrolNodes;
        this.currentNodeIndex = 0;
        this.currentPath = new List<Node>();
    }

    public void Enter()
    {
        Debug.Log("Entering Patrol State");
        CalculatePathToNextNode();
    }

    public void Update()
    {
        // Verificar si el jugador está en el campo de visión
        if (IsPlayerInView(out Vector3 playerPosition))
        {
            // Si el jugador está visible, enviar una alerta
            AlertSystem.Instance.SendAlert(playerPosition);
            return;
        }

        // Si no hay camino, calcular el camino al siguiente nodo
        if (currentPath.Count == 0)
        {
            CalculatePathToNextNode();
        }
        else
        {
            // Moverse a lo largo del camino calculado
            MoveAlongPath();
        }
    }

    private bool IsPlayerInView(out Vector3 playerPosition)
    {
        float detectionRadius = 5f;
        LayerMask playerLayer = LayerMask.GetMask("Player");
        Collider[] hitColliders = Physics.OverlapSphere(npc.transform.position, detectionRadius, playerLayer);

        foreach (var collider in hitColliders)
        {
            if (collider != null && Pathfinding.FieldOfView(npc.transform, collider.transform, 90f))
            {
                playerPosition = collider.transform.position;
                return true;
            }
        }

        playerPosition = Vector3.zero;
        return false;
    }

    private void CalculatePathToNextNode()
    {
        // Verificar que haya nodos de patrullaje disponibles
        if (patrolNodes.Count == 0) return;

        // Asegurarse de que el índice no se pase del tamaño de la lista
        if (currentNodeIndex >= patrolNodes.Count)
        {
            currentNodeIndex = 0; // Si el índice está fuera de rango, reiniciar al primer nodo
        }

        // Obtener el siguiente nodo de patrullaje y solicitar el camino hacia él
        Node nextNode = patrolNodes[currentNodeIndex];
        pathfinding.RequestPath(npc.transform.position, nextNode.transform.position, OnPathFound);
    }

    private void OnPathFound(List<Node> path)
    {
        currentPath = path;

        if (currentPath.Count > 0)
        {
            Debug.Log($"Path found with {currentPath.Count} nodes.");
        }
        else
        {
            Debug.Log("No path found.");
        }
    }

    private void MoveAlongPath()
    {
        if (currentPath.Count > 0)
        {
            Vector3 targetPosition = currentPath[0].transform.position;

            // Mover al NPC hacia el siguiente nodo en el camino
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPosition, Time.deltaTime * npc.speed);

            // Verificar si el NPC ha llegado al objetivo
            if (Vector3.Distance(npc.transform.position, targetPosition) < 0.1f)
            {
                // Eliminar el primer nodo de la lista de camino
                currentPath.RemoveAt(0);

                // Si se ha llegado al final del camino, avanzar al siguiente nodo
                if (currentPath.Count == 0)
                {
                    currentNodeIndex++;

                    // Asegurarse de que el índice no se pase del tamaño de la lista
                    if (currentNodeIndex >= patrolNodes.Count)
                    {
                        currentNodeIndex = 0; // Reiniciar al primer nodo si se ha alcanzado el final
                    }

                    // Calcular el camino hacia el siguiente nodo
                    CalculatePathToNextNode();
                }
            }
        }
        else
        {
            // Si el camino está vacío, volver a calcular el camino al siguiente nodo
            currentNodeIndex++;
            if (currentNodeIndex >= patrolNodes.Count)
            {
                currentNodeIndex = 0;
            }
            CalculatePathToNextNode();
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
