using System.Collections;
using System.Collections.Generic;
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
}

