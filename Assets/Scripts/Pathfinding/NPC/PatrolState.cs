using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private NPCController npc;


    public PatrolState(NPCController npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        // Agregar inicio de patrullaje con primer nodo
        Debug.Log("Entering Patrol State");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
