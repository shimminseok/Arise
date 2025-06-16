using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : SceneOnlySingleton<HealthBarManager>
{
    public Canvas hpBarCanvas;

    List<HPBarUI> activeBars = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void LateUpdate()
    {
        foreach (var bar in activeBars)
        {
            bar.UpdatePosion();
        }
    }

    public HPBarUI SpawnHealthBar(IDamageable owner)
    {
        HPBarUI bar = ObjectPoolManager.Instance.GetObject("HealthBar").GetComponent<HPBarUI>();
        bar.Initialize(owner);
        bar.gameObject.SetActive(true);
        activeBars.Add(bar);
        return bar;
    }
    
    public void DespawnHealthBar(HPBarUI _bar)
    {
        ObjectPoolManager.Instance.ReturnObject(_bar.gameObject);
        activeBars.Remove(_bar);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}