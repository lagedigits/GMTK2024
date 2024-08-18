using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTargetDoor : SwitchTarget
{
    [SerializeField] private DIRECTIONDOOR _direction;

    public override void Move()
    {
        if (_direction == DIRECTIONDOOR.Up)
        {
            _animator.SetBool("MoveUp", true);
        }
        else
        {
            _animator.SetBool("MoveDown", true);
        }
        SoundManager.instance.PlayClip(AUDIOCLIPTYPE.Open);        
    }
}
