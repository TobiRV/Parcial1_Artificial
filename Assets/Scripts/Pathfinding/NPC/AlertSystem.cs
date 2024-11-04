using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    public static AlertSystem Instance { get; private set; }

    private void Awake()
    {
        // Un único sistema de alertas en escena
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
        // Enviar alerta a todos los NPCs
        NPCController[] npcs = FindObjectsOfType<NPCController>();
        foreach (var npc in npcs)
        {
            npc.ReceiveAlert(playerPosition);
        }
    }
}
