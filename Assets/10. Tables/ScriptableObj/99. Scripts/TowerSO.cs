using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObject/Tower", order = 0)]
public class TowerSO : ScriptableObject, IStatProvider
{
    public int ID;

    [Header("기본 정보")]
    public string TowerName;
    [TextArea]
    public string Description;

    [FormerlySerializedAs("Stats")]
    public List<StatData> TowerStats;

    public List<StatusEffectSO> StatusEffects;
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