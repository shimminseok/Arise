using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;

    private void OnEnable()
    {
        sceneLoadEvent.RegisterListener(LoadScene);
    }

    private void OnDisable()
    {
        sceneLoadEvent.UnregisterListener(LoadScene);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}