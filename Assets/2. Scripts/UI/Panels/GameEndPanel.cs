using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndPanel : MonoBehaviour
{
    [SerializeField] private StringEventChannelSO sceneLoadEvent;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextStageButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            string currentScene = SceneManager.GetActiveScene().name;
            sceneLoadEvent.Raise(currentScene);
        });

        nextStageButton.onClick.AddListener(() =>
        {
            sceneLoadEvent.Raise("Stage 1");
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            sceneLoadEvent.Raise("MainMenuScene");
        });
    }
}