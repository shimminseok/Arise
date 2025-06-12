using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UISceneButton startButton;
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;

    private void Start()
    {
        startButton.SetData(sceneLoadEvent, "StageSelectScene");
    }
}