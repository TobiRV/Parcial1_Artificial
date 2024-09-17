using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    void UpdateAgent(); // Actualiza la l�gica del agente.
    void ApplyFlocking(IEnumerable<IAgent> neighbors); 
    void ApplyArrive(Vector3 target); 
    void ApplyEvade(IAgent predator); 
}
