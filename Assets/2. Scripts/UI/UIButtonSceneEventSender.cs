using UnityEngine;
using UnityEngine.UI;

public class UIButtonSceneEventSender : MonoBehaviour
{
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(() => sceneLoadEvent.Raise(sceneToLoad));
    }
}