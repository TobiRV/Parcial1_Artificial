using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState<CazadorIA.IAStates, CazadorIA>
{
    int wayPointCounter = 0;
    bool isWaiting;
    float waitingTime;
    float viewAngle = 50;
    public override void OnEnter()
    {
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

        var posiblePlayer = Physics.OverlapSphere(_avatar.transform.position, 5, _birdMask);

        if (posiblePlayer.Length > 0)
        {
            var playerDir = (posiblePlayer[0].transform.position - _avatar.transform.position);
            if (Vector3.Angle(_avatar.transform.forward, playerDir) < viewAngle / 2 &&
                !Physics.Raycast(_avatar.transform.position,
                playerDir, playerDir.magnitude, _avatar.obstacleMask))
            {
                _avatar.target = posiblePlayer[0].transform;
                _fsm.ChangeState(CazadorIA.IAStates.CHASE);
                return;
            }
        }

        var actualDir = (_avatar.waypoints[wayPointCounter].position - _avatar.transform.position);
        actualDir.y = 0;

        _avatar.transform.forward = (_avatar.transform.forward * 0.99f + actualDir.normalized * 0.01f);

        if (isWaiting)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime < 0)
                isWaiting = false;
            return;
        }

        _avatar.transform.position +=
            actualDir.normalized
            * Time.deltaTime * _avatar.speed;

        if (actualDir.magnitude < .2f)
        {
            wayPointCounter++;

            if (wayPointCounter >= _avatar.waypoints.Count)
            {
                wayPointCounter = 0;
            }
            isWaiting = true;
            waitingTime = 1;
        }
    }
}
