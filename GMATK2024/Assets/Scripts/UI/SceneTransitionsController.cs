using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsController : MonoBehaviour
{
    [SerializeField] private Animator _transition;

    private void OnEnable()
    {
        StaticEventHandler.OnLoadNextLevel += StaticEventHandler_OnLoadNextLevel;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnLoadNextLevel -= StaticEventHandler_OnLoadNextLevel;
    }

    private void StaticEventHandler_OnLoadNextLevel()
    {
        StartCoroutine(LoadLevelRoutine());
    }

    public IEnumerator LoadLevelRoutine()
    {
        _transition.SetTrigger("End");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        _transition.SetTrigger("Start");
    }

}
