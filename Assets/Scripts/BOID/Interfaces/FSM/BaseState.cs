using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T, J> where J : MonoBehaviour
{
    protected J _avatar;
    protected FSM<T, J> _fsm;
    protected LayerMask _birdMask;
    protected float energy;

    public void SetUp(J newAvatar, FSM<T, J> fsm,float initialEnergy = 10f)
    {
        _fsm = fsm;
        _avatar = newAvatar;
        energy = initialEnergy; //Establecer el valor de energy
    }

    public virtual void OnUpdate() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}