using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IFood
{
    Vector3 Position { get; } // Posición de la comida en el escenario.
    void Consume(); 
}