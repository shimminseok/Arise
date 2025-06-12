using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "VoidEvent", menuName = "Scriptable Objects/Events/Void Event")]
public class VoidEventChannelSO : ScriptableObject
{
    public event UnityAction OnEventRaised;

    public void Raise()
    {
#if UNITY_EDITOR
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"[EventChannel: {name}] 이벤트가 호출되었지만 리스너가 없습니다.");
        }
        else
        {
            //Debug.Log($"[EventChannel: {name}] 이벤트 발생 (데이터: Void)");
        }
#endif
        OnEventRaised?.Invoke();
    }
    
    public void RegisterListener(UnityAction listener)
    {
        OnEventRaised += listener;
#if UNITY_EDITOR
        Debug.Log($"[EventChannel: {name}] 리스너 등록됨");
#endif
    }
    
    public void UnregisterListener(UnityAction listener)
    {
        OnEventRaised -= listener;
#if UNITY_EDITOR
        Debug.Log($"[EventChannel: {name}] 리스너 제거됨");
#endif
    }
    
    public void ClearAllListeners()
    {
        OnEventRaised = null;
#if UNITY_EDITOR
        Debug.Log($"[EventChannel: {name}] 모든 리스너 제거됨");
#endif
    }
}