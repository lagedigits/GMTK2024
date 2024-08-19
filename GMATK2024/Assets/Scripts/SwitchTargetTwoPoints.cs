using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTargetTwoPoints : SwitchTarget
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _duration;

    private float _elapsedTime;

    private void Start()
    {
        _elapsedTime = 0f;
    }

    public override void Move()
    {
        Debug.Log("Move");

        StopCoroutine("MoveToB");
        StopCoroutine("MoveToA");

        if (_elapsedTime > 0)
        {
            if (_elapsedTime >= _duration)
            {
                _elapsedTime = 0;
            }
            else
            {
                _elapsedTime = _duration - _elapsedTime;
            }
        }

        StartCoroutine("MoveToB");
    }

    public override void MoveBack()
    {
        Debug.Log("MoveBack");

        StopCoroutine("MoveToA");
        StopCoroutine("MoveToB");

        if (_elapsedTime > 0)
        {
            if (_elapsedTime >= _duration)
            {
                _elapsedTime = 0;
            }
            else
            {
                _elapsedTime = _duration - _elapsedTime;
            }
        }

        StartCoroutine("MoveToA");
    }

    IEnumerator MoveToA()
    {
        while (_elapsedTime < _duration)
        {
            yield return new WaitForFixedUpdate();

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _duration)
            {
                _elapsedTime = _duration;
            }

            _target.transform.localPosition = Vector3.Lerp(_pointB.localPosition, _pointA.localPosition, _elapsedTime/_duration);
        }
    }

    IEnumerator MoveToB()
    {
        while (_elapsedTime < _duration)
        {
            yield return new WaitForFixedUpdate();

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _duration)
            {
                _elapsedTime = _duration;
            }

            _target.transform.localPosition = Vector3.Lerp(_pointA.localPosition, _pointB.localPosition, _elapsedTime / _duration);
        }
    }
}
