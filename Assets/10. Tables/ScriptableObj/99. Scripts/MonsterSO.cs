using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/Monster", order = 0)]
public class MonsterSO : ScriptableObject, IStatProvider
{
    [SerializeField] public string Name;
    public int ID;
    public GameObject Prefab;
    public List<StatData> MonsterStats;
    public List<StatData> Stats => MonsterStats;
}