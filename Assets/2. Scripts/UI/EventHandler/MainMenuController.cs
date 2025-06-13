using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SceneLoadButton startButton;
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;

    private void Start()
    {
        startButton.SetData(sceneLoadEvent, "StageSelectScene");
    }
}