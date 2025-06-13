using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private int weaponID;
    
    public StatManager StatManager { get; private set; }
    public WeaponSO WeaponData { get; private set; }
    private void Awake()
    {
        StatManager = GetComponent<StatManager>();
    }

    private void Start()
    {
        WeaponTable weaponTable = TableManager.Instance.GetTable<WeaponTable>();
        WeaponData = weaponTable.GetDataByID(weaponID);
        StatManager.Initialize(WeaponData);
        Instantiate(WeaponData.Prefab, transform);
    }
}
