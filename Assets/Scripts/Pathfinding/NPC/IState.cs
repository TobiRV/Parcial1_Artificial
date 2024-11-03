using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //metodos implementados por cada estado
    void Enter();  
    void Update(); 
    void Exit();   
}
