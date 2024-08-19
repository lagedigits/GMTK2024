using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _pauseBtn;

    private TextMeshProUGUI _panelTitle;
    private bool _isPlayerDead = false;

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
        if (Input.GetKeyDown(KeyCode.Escape) && !_isPlayerDead)
        {
            TogglePauseMenu();
        }
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerDied += StaticEventHandler_OnPlayerDied;
        StaticEventHandler.OnGameOver += StaticEventHandler_OnGameOver;
        _pauseBtn.onClick.AddListener(TogglePauseMenu);
    }

    private void OnDisable()
    {
        StaticEventHandler.OnPlayerDied -= StaticEventHandler_OnPlayerDied;
        StaticEventHandler.OnGameOver -= StaticEventHandler_OnGameOver;
        _pauseBtn.onClick.RemoveAllListeners();
    }

    private void StaticEventHandler_OnPlayerDied()
    {
        StartCoroutine(ShowGameOverPanelWithDelay(0.5f));
    }

    private void StaticEventHandler_OnGameOver()
    {
        ShowGameCompletedPanel();
    }

    private IEnumerator ShowGameOverPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _panelTitle.text = "Game Over";
        _panel.SetActive(true);

        _panel.transform.localScale = Vector3.zero;

        _panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
    }

    private void ShowGameCompletedPanel()
    {
        _panelTitle.text = "Thank you for playing!";
        _panel.SetActive(true);

        _panel.transform.localScale = Vector3.zero;

        _panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
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

            _panel.transform.localScale = Vector3.zero;
            _panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }

        StaticEventHandler.CallGamePausedEvent(_panel.activeSelf);
    }
}
