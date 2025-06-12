using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObject/Tower", order = 0)]
public class TowerSO : ScriptableObject
{
    public int ID;
    public List<StatData> Tower;
}