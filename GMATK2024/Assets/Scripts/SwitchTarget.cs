using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTarget : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    public virtual void Move(){}
}
