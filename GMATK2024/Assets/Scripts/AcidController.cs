using System.Collections;
using TarodevController;
using UnityEngine;

public class AcidController : MonoBehaviour
{
    [SerializeField] private float _timeBeforeRising = 5f;
    [SerializeField] private float _risingSpeed = 1f;

    private void Start()
    {
        Invoke(nameof(StartRisingRoutine), _timeBeforeRising);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            StaticEventHandler.CallPlayerDiedEvent();
            Destroy(player.gameObject);

            StopAllCoroutines();
        }
    }

    private void StartRisingRoutine()
    {
        StartCoroutine(RisingRoutine());
    }

    private IEnumerator RisingRoutine()
    {
        while (true)
        {
            Vector3 newScale = transform.localScale;
            newScale.y += Time.deltaTime * _risingSpeed;
            transform.localScale = newScale;
            yield return null;
        }
    }
}
