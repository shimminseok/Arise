using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/Weapon", order = 0)]
public class WeaponSO : ScriptableObject
{
    public int ID;
    public GameObject Prefab;
    public List<StatData> Stats;
}
