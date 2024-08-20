using UnityEngine;

public class MainScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StaticEventHandler.CallLoadNextLevelEvent();
        }
    }
}
