using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IAgent
{
    Vector3 Position { get; } //obtener la posici�n del agente
    Vector3 Velocity { get; } //Obetner Velocidad del Agente
    void UpdateAgent(); // Actualiza la l�gica del agente.
    void ApplyFlocking(IEnumerable<IAgent> neighbors); 
    void ApplyArrive(Vector3 target); 
    void ApplyEvade(IAgent predator);
}
