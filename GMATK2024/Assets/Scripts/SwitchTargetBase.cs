using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchTargetBase : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    public abstract void Move();
}
