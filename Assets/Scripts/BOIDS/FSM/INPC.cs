using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPC
{
    // Los m�todos que deben implementar los NPCs.
    void UpdateNPC(); 
    void Patrol(); 
    void Rest(); 
    void Shoot(IAgent target); 
    void Chase(IAgent target); 
}
