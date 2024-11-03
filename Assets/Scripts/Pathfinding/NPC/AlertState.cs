using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IState
{
    private NPCController npc;
    private Vector3 alertPosition;  // Posici�n recibida donde se vio al jugador por �ltima vez

    public AlertState(NPCController npc, Vector3 alertPosition)
    {
        this.npc = npc;
        this.alertPosition = alertPosition;
    }

    public void Enter()
    {
        Debug.Log("Entering Alert State");
        // Moverse hacia el jugador donde fue visto
    }

    public void Update()
    {
       

        //Agregar l�gica para moverse a alertposition y cambiar a persecci�n en caso de que el jugar este en el FOV 
        //en caso de no hallarlo, volver a patrullaje
    }

    public void Exit()
    {
        Debug.Log("Exiting Alert State");
    }
}
