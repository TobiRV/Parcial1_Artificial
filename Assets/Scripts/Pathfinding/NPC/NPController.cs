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
        StateMachine.ChangeState(new PursuitState(this, playerPosition, pathfinding));
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
}
