using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }

    private void Start()
    {
        StateMachine = new StateMachine();
        StateMachine.ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void ReceiveAlert(Vector3 playerPosition)
    {
        if (!(StateMachine.CurrentState is PursuitState))
        {
            StateMachine.ChangeState(new AlertState(this, playerPosition));
        }
    }

    private IEnumerator Path(Vector3 from, Vector3 to, Action<List<Node>> callback)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var actualRadious = 1.5f;
        var fromColliderArray = Physics.OverlapSphere(from, actualRadious, nodeMask);

        while (fromColliderArray.Length <= 0)
        {
            actualRadious *= 2;
            fromColliderArray = Physics.OverlapSphere(from, actualRadious, nodeMask);
        }

        if (fromColliderArray.Length > 0)
        {
            fromNode = GetClosestNode(fromColliderArray, from);
        }

        actualRadious = 1.5f;
        var toColliderArray = Physics.OverlapSphere(to, actualRadious, nodeMask);

        while (toColliderArray.Length <= 0)
        {
            actualRadious *= 2;
            toColliderArray = Physics.OverlapSphere(to, actualRadious, nodeMask);
        }

        if (toColliderArray.Length > 0)
        {
            toNode = GetClosestNode(toColliderArray, to);
        }

        closeNodes = new HashSet<Node>();
        openNodes = new PriorityQueue<Node>();

        var actualNode = fromNode;
        actualNode.Weight = 0;

        var watchdog = 100000;

        var counter = 0;

        while (actualNode != null && actualNode != toNode && watchdog > 0)
        {
            OnResetNodes += actualNode.OnResetWeight;
            watchdog--;

            foreach (var node in actualNode.neighbours)
            {
                if (closeNodes.Contains(node)) continue;
                OnResetNodes += node.OnResetWeight;
                var heuristic = actualNode.Weight +
                    Vector3.Distance(node.transform.position, actualNode.transform.position) +
                    Vector3.Distance(node.transform.position, toNode.transform.position);

                if (node.Weight > heuristic)
                {
                    node.Weight = heuristic;
                    node.previous = actualNode;
                }

                if (!openNodes.Contains(node)) openNodes.Enqueue(node);
            }

            closeNodes.Add(actualNode);

            actualNode = openNodes.Dequeue();

            if (counter > 5000)
            {
                yield return null;
                counter = 0;
            }

            counter++;
        }

        #region Theta en funcion aparte
        //var finalPath = ThetaStar();
        #endregion

        #region Theta en A*
        var finalPath = new List<Node>();
        actualNode = toNode;
        var seeingNode = toNode.previous;
        finalPath.Add(actualNode);

        watchdog = 10000;
        counter = 0;

        while (seeingNode != null && seeingNode.previous != null && watchdog > 0 &&
            actualNode != fromNode && actualNode.previous != null)
        {
            watchdog--;
            var dir = seeingNode.previous.transform.position -
                actualNode.transform.position;

            if (!Physics.Raycast(actualNode.transform.position, dir, dir.magnitude, LayerMask.GetMask("Wall", "Floor"))
                && dir.y == 0)
            {
                seeingNode = seeingNode.previous;
            }
            else
            {
                finalPath.Add(seeingNode);
                actualNode = seeingNode;
                seeingNode = seeingNode.previous;
            }

            counter++;
            if (counter > 1000)
            {
                yield return null;
                counter = 0;
            }
        }

        if (!finalPath.Contains(fromNode))
            finalPath.Add(fromNode);
        #endregion

        finalPath.Reverse();
        stopWatch.Stop();
        UnityEngine.Debug.Log(stopWatch.Elapsed);
        UnityEngine.Debug.Log(stopWatch.ElapsedTicks);
        callback(finalPath);
        isCalculating = false;
    }
}

