using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseState<CazadorIA.IAStates, CazadorIA>
{
    public override void OnEnter()
    {
        Debug.Log("Entro en chase");
        energy--;

        if (energy <= 0)
        {
            _fsm.ChangeState(CazadorIA.IAStates.Rest);
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        Debug.Log(energy + " energia");
        energy-=Time.deltaTime;
        if (energy <= 0)
        {
            _fsm.ChangeState(CazadorIA.IAStates.Rest);
            return;
        }
        var dir = _avatar.target.transform.position - _avatar.transform.position;
        dir.y = 0;
        _avatar.transform.forward = (_avatar.transform.forward * 0.9f + dir * 0.1f);

        if (dir.magnitude < 1) return;

        _avatar.transform.position += _avatar.transform.forward * _avatar.speed * Time.deltaTime;

    }
}
   

