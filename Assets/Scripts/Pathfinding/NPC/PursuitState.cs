using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitState : IState
{
    private NPCController npc;

    public PursuitState(NPCController npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        Debug.Log("Entering Pursuit State");
    }

    public void Update()
    {
        // Agregar lógica para moverse hacia el jugador utilizando A*
       
    }

    public void Exit()
    {
        Debug.Log("Exiting Pursuit State");
    }
}
