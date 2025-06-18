using System;
using UnityEngine;

public class GoldManager : SceneOnlySingleton<GoldManager>
{

    [SerializeField] private int startingGold = 500;
    public int CurrentGold { get; private set; }

    public event Action<int> OnGoldChanged;

    protected override void Awake()
    {
        base.Awake();
        CurrentGold = startingGold;
        OnGoldChanged?.Invoke(CurrentGold); // 초기화 시도
    }

    public bool TrySpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            OnGoldChanged?.Invoke(CurrentGold);
            return true;
        }

        Debug.Log("골드 부족!");
        return false;
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public int Gold
    {
        get => CurrentGold;
        set
        {
            CurrentGold = value;
            OnGoldChanged?.Invoke(CurrentGold);
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}