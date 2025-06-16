using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObject/Boss", order = 0)]
public class BossSO : ScriptableObject, IStatProvider
{
    [SerializeField] public string Name;
    public int ID;
    public GameObject Prefab;
    public List<StatData> BossStats;
    public List<StatData> Stats => BossStats;
}