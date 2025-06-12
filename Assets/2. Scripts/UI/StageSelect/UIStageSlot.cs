using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSlot : MonoBehaviour
{
    [SerializeField] private Button stageButton;
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private GameObject lockIcon;

    public void SetData(string stageName, bool isUnlocked)
    {
        stageNameText.text = stageName;
        lockIcon.SetActive(!isUnlocked);
        stageButton.interactable = isUnlocked;
    }
}

