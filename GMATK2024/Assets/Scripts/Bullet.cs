using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SCALETYPE _scaleType;
    [SerializeField] private float _autoDestroyTime;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSecondsRealtime(_autoDestroyTime);

        Destroy(gameObject);
    }

    public void SetScaleType(SCALETYPE s)
    {
        _scaleType = s;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Collided");
            Destroy(gameObject);
        }
    }
}
