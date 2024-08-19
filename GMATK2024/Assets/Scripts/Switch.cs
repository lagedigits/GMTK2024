using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private SwitchTargetBase _target;
    [SerializeField] private bool _isMomentary;
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
                if (_isMomentary == false)
                {
                    _hasCollided = true;
                }
                _target.Move();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(_isMomentary)
        {
            if (((1 << other.gameObject.layer) & _ignoreLayers) != 0)
            {
                // Ignore the collision
                return;
            }

            if (!other.CompareTag("Bullet"))
            {
                _animator.SetBool("Off", true);
                _target.MoveBack();
            }
        }
    }

    public void ResetHasCollided()
    {
        _hasCollided = false;
    }

    public void Reset()
    {
        _hasCollided = false;
    }
}
