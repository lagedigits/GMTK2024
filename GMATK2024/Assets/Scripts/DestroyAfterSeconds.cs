using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float _time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSecondsRealtime(_time);

        Destroy(gameObject);
    }

}
