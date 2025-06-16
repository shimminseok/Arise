using UnityEngine;

public class TurretInstallListener : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO installEvent;
    [SerializeField] private VoidEventChannelSO cancelEvent;

    private void OnEnable()
    {
        installEvent.RegisterListener(OnInstall);
        cancelEvent.RegisterListener(OnCancel);
    }

    private void OnDisable()
    {
        installEvent.UnregisterListener(OnInstall);
        cancelEvent.UnregisterListener(OnCancel);
    }

    private void OnInstall()
    {
        Debug.Log("[설치] 눌림");
        
        // 설치 로직 추가
    }

    private void OnCancel()
    {
        Debug.Log("[취소] 눌림");
    }
}