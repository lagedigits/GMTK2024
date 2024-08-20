using TarodevController;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            StaticEventHandler.CallLoadNextLevelEvent();
        }
    }
}
