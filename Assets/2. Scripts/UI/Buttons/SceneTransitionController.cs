using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;
    [SerializeField] private SceneFlowData sceneFlowData;
    [SerializeField] private string sceneToLoadOverride;
    [SerializeField] private bool reloadCurrentSceneInstead = false;
    [SerializeField] private GameObject panelToDisableAfterClick;
    

    public void TriggerSceneLoad()
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

    // 이벤트 시스템 대신 직접 로드
    SceneManager.LoadScene(targetScene);

    if (panelToDisableAfterClick != null)
        panelToDisableAfterClick.SetActive(false);
}

}
