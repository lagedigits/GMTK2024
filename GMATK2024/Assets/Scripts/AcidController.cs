using System.Collections;
using TarodevController;
using UnityEngine;

public class AcidController : MonoBehaviour
{
    [SerializeField] private float _timeBeforeRising = 5f;
    [SerializeField] private float _risingSpeed = 1f;
    private Transform _acidTransform;
    private Transform _acidColliderKill;
    private Transform _acidColliderSlow;

    private void Start()
    {
        _acidTransform = transform.GetChild(0);
        _acidColliderKill = transform.GetChild(1);
        _acidColliderSlow = transform.GetChild(2);
        Invoke(nameof(StartRisingRoutine), _timeBeforeRising);
    }

    private void StartRisingRoutine()
    {
        StartCoroutine(RisingRoutine());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator RisingRoutine()
    {
        while (true)
        {
            Vector3 newPosAT = _acidTransform.localPosition;
            newPosAT.y += Time.deltaTime * _risingSpeed;
            _acidTransform.localPosition = newPosAT;

            Vector3 newPosACK = _acidColliderKill.localPosition;
            newPosACK.y += Time.deltaTime * _risingSpeed;
            _acidColliderKill.localPosition = newPosACK;

            Vector3 newPosACS = _acidColliderSlow.localPosition;
            newPosACS.y += Time.deltaTime * _risingSpeed;
            _acidColliderSlow.localPosition = newPosACS;

            yield return null;
        }
    }
}
