using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEventChannelSO<T> : ScriptableObject
{
    public event UnityAction<T> OnEventRaised;

    public void Raise(T data)
    {
#if UNITY_EDITOR
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"[EventChannel: {name}] 이벤트가 호출되었지만 리스너가 없습니다.");
        }
        else
        {
            //Debug.Log($"[EventChannel: {name}] 이벤트 발생 (데이터: {data})");
        }
#endif
        OnEventRaised?.Invoke(data);
    }
    
    public void RegisterListener(UnityAction<T> listener)
    {
        OnEventRaised += listener;
#if UNITY_EDITOR
        Debug.Log($"[EventChannel: {name}] 리스너 등록됨");
#endif
    }
    
    public void UnregisterListener(UnityAction<T> listener)
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
