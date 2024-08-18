using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private SwitchTargetBase _target;
    [SerializeField] private LayerMask _ignoreLayers;
    [SerializeField] private BoxCollider2D _colliderTrigger;
    [SerializeField] private Animator _animator;
    private bool _hasCollided;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasCollided == false)
        {
            if (((1 << other.gameObject.layer) & _ignoreLayers) != 0)
            {
                // Ignore the collision
                return;
            }

            if (!other.CompareTag("Bullet"))
            {
                _animator.SetBool("On", true);
                _hasCollided = true;
                _target.Move();
            }
        }
    }

    public void Reset()
    {
        _hasCollided = false;
    }
}
