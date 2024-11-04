using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }
    public Pathfinding pathfinding; // Asegúrate de que este campo esté inicializado en el Inspector
    public List<Node> patrolNodes;
    public float speed = 5f;

    private void Start()
    {
        StateMachine = new StateMachine();
        StateMachine.ChangeState(new PatrolState(this, pathfinding, patrolNodes)); // Cambiar al estado inicial
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
}
