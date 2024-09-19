using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorIA : MonoBehaviour
{
    public List<Transform> waypoints;
    public LayerMask birdMask;
    public LayerMask obstacleMask;
    public Transform target;
   


    public FSM<IAStates, CazadorIA> fsm;

    public enum IAStates
    {
        Rest,
        Patrol,
        CHASE,
    }


    public float speed;

    void Awake()
    {
        fsm = new FSM<IAStates, CazadorIA>()

            .AddState(IAStates.Rest, new Rest())
            .AddState(IAStates.Patrol, new Patrol())
            .AddState(IAStates.CHASE, new Chase());

        fsm.Done(this);

        fsm.ChangeState(IAStates.Patrol);
    }

    void Update()
    {
        fsm.OnUpdate();
    }


}
