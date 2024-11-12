using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    public static AlertSystem Instance { get; private set; }

    private void Awake()
    {
        // Un �nico sistema de alertas en escena
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // M�todo que se llama cuando un NPC ve al jugador
    public void SendAlert(Vector3 playerPosition)
    {
        // Alerta a todos los NPCs en la escena
        NPCController[] npcs = FindObjectsOfType<NPCController>();
        foreach (var npc in npcs)
        {
            npc.ReceiveAlert(playerPosition);
        }
    }
}
