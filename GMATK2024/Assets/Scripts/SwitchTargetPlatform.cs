using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTargetPlatform : SwitchTarget
{
    [SerializeField] protected Animator _animator;
    [SerializeField] private DIRECTIONPLATFORM _direction;

    public override void Move()
    {
        if (_direction == DIRECTIONPLATFORM.Left)
        {
            _animator.SetBool("MoveLeft", true);
        }
        else
        {
            _animator.SetBool("MoveRight", true);
        }
        SoundManager.instance.PlayClip(AUDIOCLIPTYPE.Open);        
    }
}
