using UnityEngine;
using UnityEngine.UI;

public class UITurretDescriptionButton : MonoBehaviour
{
    [Header("설명창 오브젝트")]
    [SerializeField] private GameObject parentPanel;

    [Header("이벤트 채널")]
    [SerializeField] private VoidEventChannelSO clickEvent;

    [Header("버튼 컴포넌트")]
    [SerializeField] private Button thisButton;

    private void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            clickEvent?.Raise();
            parentPanel?.SetActive(false);
        });
    }
}