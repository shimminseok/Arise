using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, UIBase> UIDict = new Dictionary<Type, UIBase>();

    private List<UIBase> openedUIList = new List<UIBase>();

    protected override void Awake()
    {
        base.Awake();
        InitializeUIRoot();
    }

    public void InitializeUIRoot()
    {
        UIDict.Clear();
        Transform uiRoot       = GameObject.Find("UIRoot").transform;
        UIBase[]  uiComponents = uiRoot.GetComponentsInChildren<UIBase>(true);

        foreach (UIBase uiComponent in uiComponents)
        {
            UIDict[uiComponent.GetType()] = uiComponent;
            uiComponent.Close();
        }
    }

    public void Open<T>() where T : UIBase
    {
        if (UIDict.TryGetValue(typeof(T), out UIBase ui))
        {
            ui.Open();
            openedUIList.Add(ui);
        }
    }

    public void Close<T>() where T : UIBase
    {
        if (UIDict.TryGetValue(typeof(T), out UIBase ui) && openedUIList.Contains(ui))
        {
            ui.Close();
            openedUIList.Remove(ui);
        }
    }


    public T GetUIComponent<T>() where T : UIBase
    {
        return UIDict[typeof(T)] as T;
    }
}

public class UIBase : MonoBehaviour
{
    [SerializeField] private RectTransform contents;

    protected RectTransform Contents => contents;

    public virtual void Open()
    {
        contents.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        contents.gameObject.SetActive(false);
    }
}