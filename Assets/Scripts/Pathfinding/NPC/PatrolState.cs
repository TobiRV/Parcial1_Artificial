using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private NPCController npc;
    public static Pathfinding Instance { get; private set; }

    public LayerMask nodeMask;

    private Node fromNode;
    private Node toNode;

    private HashSet<Node> closeNodes;
    private PriorityQueue<Node> openNodes;
    private struct PathRequestData
    {
        public Vector3 fromPoint;
        public Vector3 toPoint;
        public Action<List<Node>> callbackPath;
    }

    private Queue<PathRequestData> queuePath = new Queue<PathRequestData>();
    private bool isCalculating = false;

    private Action OnResetNodes = delegate { };

    [SerializeField] private float yMargin;

    public PatrolState(NPCController npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        // Agregar inicio de patrullaje con primer nodo
        Instance = this;
        Debug.Log("Entering Patrol State");
    }

    public void Update()
    {
        if (!(queuePath.Count > 0) || isCalculating) return;

        OnResetNodes();
        OnResetNodes = delegate () { };
        isCalculating = true;
        var actualData = queuePath.Dequeue();
        StartCoroutine(Path(actualData.fromPoint, actualData.toPoint, actualData.callbackPath));
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
