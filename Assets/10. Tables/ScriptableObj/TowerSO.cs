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
    public AttackTypeSO AttackType;
    public int BuildCost;

    public bool UseMultishot;

    [BoolShowIf("UseMultishot")]
    public int ProjectileCount;

    public bool UseSplashAttack;

    [BoolShowIf("UseSplashAttack")]
    public float SplashRadius;

    public List<StatData> Stats => TowerStats;
}