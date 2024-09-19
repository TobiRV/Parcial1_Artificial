using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rest : BaseState<CazadorIA.IAStates, CazadorIA>
{

    [SerializeField] private float _restDuration = 10;
    private bool _isResting = false;
    public CazadorIA cazador;
    public override void OnEnter()
    {
        _isResting = true;
        cazador.StartCoroutine(RestCountDown());
    }

    public override void OnExit()
    {
        _isResting = false;
    }

    public override void OnUpdate()
    {
        Debug.Log("Rest");
    }

    private IEnumerator RestCountDown()
    {
        float _timer = 0f;

        while (_timer < _restDuration)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        energy = 10f;
        _fsm.ChangeState(CazadorIA.IAStates.Patrol);
    }
}
