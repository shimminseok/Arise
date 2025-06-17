using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private int weaponID;
    private GameObject _currentWeaponInstance;
    
    public StatManager StatManager { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }
    public WeaponSO WeaponData { get; private set; }
    private void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
    }

    private void Start()
    {
        EquipWeapon(weaponID);
    }
    
    public void EquipWeapon(int newWeaponID)
    {
        WeaponTable weaponTable = TableManager.Instance.GetTable<WeaponTable>();
        WeaponSO newWeaponData = weaponTable.GetDataByID(newWeaponID);

        if (newWeaponData == null)
        {
            Debug.LogError($"Weapon ID {newWeaponID} not found in WeaponTable.");
            return;
        }

        if (_currentWeaponInstance != null)
        {
            Destroy(_currentWeaponInstance);
        }

        WeaponData = newWeaponData;
        StatManager.Initialize(WeaponData);

        _currentWeaponInstance = Instantiate(WeaponData.Prefab, transform);
        weaponID = newWeaponID;
    }

    [ContextMenu("Sword")]
    public void ChangeWeaponSword() => EquipWeapon(0);
    
    [ContextMenu("Axe")]
    public void ChangeWeaponAxe() => EquipWeapon(1);
    
    [ContextMenu("Hammer")]
    public void ChangeWeaponHammer() => EquipWeapon(2);
}
