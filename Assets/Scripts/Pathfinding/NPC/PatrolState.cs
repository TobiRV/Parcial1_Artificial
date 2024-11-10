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
        if (IsPlayerInView(out Vector3 playerPosition))
        {
            AlertSystem.Instance.SendAlert(playerPosition);
            return;
        }

        if (currentPath.Count == 0)
        {
            CalculatePathToNextNode();
        }
        else
        {
            MoveAlongPath();
        }
    }

    private bool IsPlayerInView(out Vector3 playerPosition)
    {
        float detectionRadius = 5f; 
        LayerMask playerLayer = LayerMask.GetMask("PlayerLayer");
        Collider[] hitColliders = Physics.OverlapSphere(npc.transform.position, detectionRadius, playerLayer);
        foreach (var collider in hitColliders)
        {
            if (collider != null)
            {
                if (Pathfinding.FieldOfView(npc.transform, collider.transform, 45f))
                {
                    playerPosition = collider.transform.position; 
                    return true;
                }
            }
        }

        playerPosition = Vector3.zero;
        return false;
    }

    private void CalculatePathToNextNode()
    {
        if (patrolNodes.Count == 0) return;

        Node nextNode = patrolNodes[currentNodeIndex];
        pathfinding.RequestPath(npc.transform.position, nextNode.transform.position, OnPathFound);
    }

    private void OnPathFound(List<Node> path)
    {
        currentPath = path;
        if (currentPath.Count > 0)
        {
            currentPath.RemoveAt(0);
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

            npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPosition, Time.deltaTime * npc.speed);

            if (Vector3.Distance(npc.transform.position, targetPosition) < 0.1f)
            {
                currentPath.RemoveAt(0);
            }
        }
        else
        {
            currentNodeIndex = (currentNodeIndex + 1) % patrolNodes.Count;
            CalculatePathToNextNode();
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
