using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/Monster", order = 0)]
public class MonsterSO : ScriptableObject
{
    [SerializeField] public string Name;
    public int ID;
    public GameObject Prefab;
    public List<StatData> Stats;
}