using UnityEngine;
using UnityEngine.UI;

public class StageSelectPanel : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject stageSlotPrefab;
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;

    private void Start()
    {
        AddStageSlot("IntroScene", "Tutorial", true);
        AddStageSlot("Stage 1", "Stage 1", true);
        AddStageSlot("Stage 2", "Stage 2", false);
        AddStageSlot("Stage 3", "Stage 3", false);
        AddStageSlot("Stage 4", "Stage 4", false);
        AddStageSlot("Stage 5", "Stage 5", false);
        AddStageSlot("Stage 6", "Stage 6", false);
        AddStageSlot("Stage 7", "Stage 7", false);
        AddStageSlot("Stage 8", "Stage 8", false);
        AddStageSlot("Stage 9", "Stage 9", false);
        AddStageSlot("Stage 10", "Stage 10", false);
    }

    void AddStageSlot(string sceneName, string displayName, bool isUnlocked)
    {
        GameObject go = Instantiate(stageSlotPrefab, contentParent);
        StageSelectSlot slot = go.GetComponent<StageSelectSlot>();
        slot.SetData(displayName, isUnlocked);

        if (isUnlocked)
        {
            SceneLoadButton sceneButton = go.GetComponent<SceneLoadButton>();
            sceneButton.SetData(sceneLoadEvent, sceneName);
        }
    }
}
