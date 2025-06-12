using UnityEngine;

public class TurretInstallListener : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO installEvent;
    [SerializeField] private VoidEventChannelSO cancelEvent;

    private void OnEnable()
    {
        installEvent.RegisterListener(() => Debug.Log("[설치] 눌림"));
        cancelEvent.RegisterListener(() => Debug.Log("[취소] 눌림"));
    }

    private void OnDisable()
    {
        installEvent.UnregisterListener(() => Debug.Log("[설치] 리스너 해제"));
        cancelEvent.UnregisterListener(() => Debug.Log("[취소] 리스너 해제"));
    }
}
