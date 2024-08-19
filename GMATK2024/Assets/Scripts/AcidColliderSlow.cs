using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class AcidColliderSlow : MonoBehaviour
{
    private float _maxVelocity;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            _maxVelocity = player.GetMaxVelocity();

            player.SetMaxVelosity(player.GetRestrictedVelocity());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.SetMaxVelosity(_maxVelocity);
        }
    }
}
