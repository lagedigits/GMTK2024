using TarodevController;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            StaticEventHandler.CallGameOverEvent();

            player.enabled = false;
        }
    }
}
