using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SCALETYPE _scaleType;
    [SerializeField] private float _autoDestroyTime;
    // The bullet prefab
    [SerializeField] private GameObject _explosionResizeObj;
    // The bullet prefab
    [SerializeField] private GameObject _explosion;

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
            ScalableObjectBase sObj = other.GetComponent<ScalableObjectBase>();

            if (sObj != null)
            {
                sObj.Scale(_scaleType);
                // Instantiate explosion when colliding with resizable object
                Instantiate(_explosionResizeObj, gameObject.transform.position, Quaternion.identity);
                
                // Play sound
                SoundManager.instance.PlayClip(AUDIOCLIPTYPE.ExplosionResizableObj);
            }
            else
            {
                // Instantiate explosion when colliding with object
                Instantiate(_explosion, gameObject.transform.position, Quaternion.identity);

                // Play sound
                SoundManager.instance.PlayClip(AUDIOCLIPTYPE.Explosion);
            }

            Destroy(gameObject);
        }
    }
}
