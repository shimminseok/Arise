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
        Debug.Log($"씬 전환 : {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}