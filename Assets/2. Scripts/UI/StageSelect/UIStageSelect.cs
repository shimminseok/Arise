using UnityEngine;
using UnityEngine.UI;

public class UIStageSelect : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject stageSlotPrefab;
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;

    private void Start()
    {
        AddStageSlot("Tutorial", true);
        AddStageSlot("Stage 1", true);
        AddStageSlot("Stage 2", false);
        AddStageSlot("Stage 3", false);
        AddStageSlot("Stage 4", false);
        AddStageSlot("Stage 5", false);
        AddStageSlot("Stage 6", false);
        AddStageSlot("Stage 7", false);
        AddStageSlot("Stage 8", false);
        AddStageSlot("Stage 9", false);
        AddStageSlot("Stage 10", false);
    }

    void AddStageSlot(string sceneName, bool isUnlocked)
    {
        GameObject go = Instantiate(stageSlotPrefab, contentParent);
        
        UIStageSlot slot = go.GetComponent<UIStageSlot>();
        slot.SetData(sceneName, isUnlocked);

        if (isUnlocked)
        {
            UISceneButton sceneButton = go.GetComponent<UISceneButton>();
            sceneButton.SetData(sceneLoadEvent, sceneName);
        }
    }
}
