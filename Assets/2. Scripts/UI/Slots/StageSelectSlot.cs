using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectSlot : MonoBehaviour
{
    [SerializeField] private Button stageButton;
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private GameObject lockIcon;

    public void SetData(string displayName, bool isUnlocked)
    {
        stageNameText.text = displayName;
        lockIcon.SetActive(!isUnlocked);
        stageButton.interactable = isUnlocked;
    }
}