using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Type, UIBase> UIDict = new();
    private List<UIBase> openedUIList = new();

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

        DontDestroyOnLoad(gameObject); // 씬 전환 시 살아있도록 유지
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "IntroScene")
        {
            SetCanvasGroupActive(false);
        }
        else
        {
            SetCanvasGroupActive(true);
            InitializeUIRoot();
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "IntroScene")
        {
            SetCanvasGroupActive(false);
            Debug.Log("[UIManager] IntroScene → UI 숨김");
        }
        else
        {
            SetCanvasGroupActive(true);
            InitializeUIRoot();
            Debug.Log($"[UIManager] {sceneName} → UI 활성화 및 초기화");
        }

        ChangeSceneBGM(sceneName);
    }

    private void ChangeSceneBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "StageSelectScene":
                break;
            case "Tutorial":
                break;
            case "Stage 1":
            case "Stage 2":
            case "Stage 3":
                SoundManager.Instance.ChangeBGM(BGM.InGame);
                break;
        }
    }
    private void SetCanvasGroupActive(bool active)
    {
        Transform inGameUI = transform.Find("Canvas - InGameUI");
        Transform systemUI = transform.Find("Canvas - SystemUI");

        if (inGameUI != null) inGameUI.gameObject.SetActive(active);
        if (systemUI != null) systemUI.gameObject.SetActive(active);
    }

    private void InitializeUIRoot()
    {
        UIDict.Clear();

        Transform uiRoot = GameObject.Find("UIRoot")?.transform;
        if (uiRoot == null)
        {
            Debug.LogWarning("[UIManager] UIRoot를 찾을 수 없습니다.");
            return;
        }

        UIBase[] uiComponents = uiRoot.GetComponentsInChildren<UIBase>(true);
        foreach (UIBase uiComponent in uiComponents)
        {
            UIDict[uiComponent.GetType()] = uiComponent;
            uiComponent.Close();
        }
    }

    public void ConnectStatUI(GameObject playerObject, GameObject weaponObject)
    {
        if (playerStatPanel == null) return;

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
        return UIDict.TryGetValue(typeof(T), out var ui) ? ui as T : null;
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