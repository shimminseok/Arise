using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneEventSenderButton : MonoBehaviour
{
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;
    [SerializeField] private SceneFlowData sceneFlowData;
    [SerializeField] private string sceneToLoadOverride;
    [SerializeField] private bool reloadCurrentSceneInstead = false;
    [SerializeField] private Button button;
    [SerializeField] private GameObject panelToDisableAfterClick;
    
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            string targetScene;

            if (reloadCurrentSceneInstead)
            {
                targetScene = SceneManager.GetActiveScene().name;
            }
            else if (!string.IsNullOrEmpty(sceneToLoadOverride))
            {
                targetScene = sceneToLoadOverride;
            }
            else
            {
                targetScene = sceneFlowData.GetNextScene(SceneManager.GetActiveScene().name);
            }

            sceneLoadEvent.Raise(targetScene);
            if (panelToDisableAfterClick != null)
                panelToDisableAfterClick.SetActive(false);
        });
    }



}