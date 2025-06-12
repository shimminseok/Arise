using UnityEngine;
using UnityEngine.UI;

public class UITurretDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Button installButton;
    [SerializeField] private Button cancelButton;

    private void Start()
    {
        installButton.onClick.AddListener(() => gameObject.SetActive(false));
        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}