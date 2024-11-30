using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    public static AlertSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendAlert(Vector3 playerPosition)
    {
        NPCController[] npcs = FindObjectsOfType<NPCController>();
        foreach (var npc in npcs)
        {
            // Si el NPC ya está en un estado de alerta, cambia su objetivo inmediatamente
            if (npc.StateMachine.CurrentState is PatrolState)
            {
                npc.ReceiveAlert(playerPosition); // Recibe la alerta para empezar a perseguir
            }
        }
    }
}
