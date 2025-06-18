using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private SceneLoadEventChannelSO sceneLoadEvent;


    private event UnityAction _onSceneChanged;
    private readonly HashSet<UnityAction> _callbacks = new HashSet<UnityAction>();

    protected override void Awake()
    {
        base.Awake();
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnEnable()
    {
        sceneLoadEvent.RegisterListener(LoadScene);
    }

    private void OnDisable()
    {
        sceneLoadEvent.UnregisterListener(LoadScene);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AddChangeSceneEvent(UnityAction callback)
    {
        if (!_callbacks.Contains(callback))
        {
            _onSceneChanged += callback;
            _callbacks.Add(callback);
        }
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        _onSceneChanged?.Invoke();
    }
    
    
}