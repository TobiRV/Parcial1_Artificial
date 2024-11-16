using System.Collections.Generic;
using UnityEngine;

public class PursuitState : IState
{
    private NPCController npc;
    private Vector3 targetPosition;
    private Pathfinding pathfinding;
    private List<Node> currentPath;
    private float timeWithoutPlayer;
    private const float maxTimeWithoutPlayer = 0.2f; // tiempo máximo antes de volver a patrullar

    public PursuitState(NPCController npc, Vector3 targetPosition, Pathfinding pathfinding)
    {
        this.npc = npc;
        this.targetPosition = targetPosition;
        this.pathfinding = pathfinding;
        this.currentPath = new List<Node>();
        this.timeWithoutPlayer = 0f;
    }

    public void Enter()
    {
        Debug.Log("Entering Pursuit State");
        CalculatePathToPlayer();
    }

    public void Update()
    {
        if (npc.IsPlayerInView(out Vector3 playerPosition))
        {
            targetPosition = playerPosition;
            CalculatePathToPlayer();
            timeWithoutPlayer = 0f; // Reinicia el temporizador si ve al jugador
        }
        else
        {
            timeWithoutPlayer += Time.deltaTime;
            if (timeWithoutPlayer > maxTimeWithoutPlayer)
            {
                npc.ChangeState(new PatrolState(npc, pathfinding, npc.patrolNodes));
                return;
            }
        }

        MoveAlongPath();
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
            currentPath.RemoveAt(0);
        }
    }

    private void MoveAlongPath()
    {
        if (currentPath.Count > 0)
        {
            Vector3 nextPosition = currentPath[0].transform.position;
            npc.RotateTowards(nextPosition);
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, nextPosition, Time.deltaTime * npc.speed);

            if (Vector3.Distance(npc.transform.position, nextPosition) < 0.1f)
            {
                currentPath.RemoveAt(0);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Pursuit State");
    }
}
