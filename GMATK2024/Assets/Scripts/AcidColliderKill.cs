using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class AcidColliderKill : MonoBehaviour
{
    private AcidController _acidController;

    private void Start()
    {
        _acidController = transform.parent.GetComponent<AcidController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.Die();
            _acidController.Stop();
        }
    }
}
