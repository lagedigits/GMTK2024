using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _pauseBtn;

    private TextMeshProUGUI _panelTitle;

    private void Awake()
    {
        _panelTitle = _panel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _panelTitle.text = string.Empty;

        _panel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerDied += StaticEventHandler_OnPlayerDied;
        _pauseBtn.onClick.AddListener(TogglePauseMenu);
    }

    private void OnDisable()
    {
        StaticEventHandler.OnPlayerDied -= StaticEventHandler_OnPlayerDied;
        _pauseBtn.onClick.RemoveAllListeners();
    }

    private void StaticEventHandler_OnPlayerDied()
    {
        _panelTitle.text = "Game Over";
        _panel.SetActive(true);
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

    private void TogglePauseMenu()
    {
        if (_panel.activeSelf)
        {
            _panel.SetActive(false);
            _panelTitle.text = string.Empty;
        }
        else
        {
            _panelTitle.text = "Game Paused";
            _panel.SetActive(true);
        }

        StaticEventHandler.CallGamePausedEvent(_panel.activeSelf);
    }
}
