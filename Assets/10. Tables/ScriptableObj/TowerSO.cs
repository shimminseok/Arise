using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObject/Tower", order = 0)]
public class TowerSO : ScriptableObject, IStatProvider
{
    public int ID;

    [FormerlySerializedAs("Stats")]
    public List<StatData> TowerStats;

    public List<StatusEffectData> StatusEffects;
    public int BuildCost;

    public List<StatData> Stats => TowerStats;
}