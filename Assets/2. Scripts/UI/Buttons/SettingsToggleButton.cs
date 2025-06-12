using UnityEngine;
using UnityEngine.UI;

public class SettingsToggleButton : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO eventToRaise;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            eventToRaise.Raise();
        });
    }
}