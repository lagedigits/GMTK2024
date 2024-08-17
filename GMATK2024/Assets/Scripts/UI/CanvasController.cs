using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;

    private void Start()
    {
        _gameOverScreen.SetActive(false);
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerDied += StaticEventHandler_OnPlayerDied;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnPlayerDied -= StaticEventHandler_OnPlayerDied;
    }

    private void StaticEventHandler_OnPlayerDied()
    {
        _gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        var activeScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(activeScene.name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
