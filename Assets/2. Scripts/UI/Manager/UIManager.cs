using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, UIBase> UIDict = new Dictionary<Type, UIBase>();

    private List<UIBase> openedUIList = new List<UIBase>();

    [SerializeField] private UIPlayerStatPanel playerStatPanel;
    
    [SerializeField] private GameObject turretModeButton;
    
    [SerializeField] private GameObject questPanel; 

    public GameObject QuestPanelObject => questPanel;
    public GameObject TurretModeButton => turretModeButton;
    
    protected override void Awake()
    {
        base.Awake();
        if (IsDuplicate)
            return;
        SceneLoader.Instance.AddChangeSceneEvent(InitializeUIRoot);
    }

    public void ConnectStatUI(GameObject playerObject, GameObject weaponObject)
    {
        if (playerStatPanel == null)
        {
            return;
        }

        var playerStat = playerObject?.GetComponent<StatManager>();
        var weaponCtrl = weaponObject?.GetComponent<WeaponController>();
        var weaponStat = weaponCtrl?.StatManager;

        if (playerStat != null && weaponStat != null)
        {
            playerStatPanel.SetStatManagers(playerStat, weaponStat);
            Debug.Log("[UIManager] 스탯 UI 연결 완료");
        }
        else
        {
            Debug.LogWarning("[UIManager] Stat 연결 실패: StatManager 참조가 비어 있음");
        }
    }

    private void InitializeUIRoot()
    {
        UIDict.Clear();
        Transform uiRoot = GameObject.Find("UIRoot")?.transform;
        if (uiRoot == null)
            return;
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
            openedUIList.Add(ui);
            ui.Open();
        }
    }

    public void Close<T>() where T : UIBase
    {
        if (UIDict.TryGetValue(typeof(T), out UIBase ui) && openedUIList.Contains(ui))
        {
            openedUIList.Remove(ui);
            ui.Close();
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