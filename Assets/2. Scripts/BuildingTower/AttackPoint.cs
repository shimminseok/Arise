using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 10;


    public int      CurrentCount { get; private set; } = 0;
    public Collider Collider     { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    public bool TryReserve()
    {
        if (CurrentCount >= maxCapacity)
            return false;

        CurrentCount++;
        return true;
    }

    public void Release()
    {
        CurrentCount = Mathf.Max(CurrentCount - 1, 0);
    }
}