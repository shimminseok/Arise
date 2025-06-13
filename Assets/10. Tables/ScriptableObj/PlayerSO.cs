using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/Player", order = 0)]
public class PlayerSO : ScriptableObject, IStatProvider
{
    public int ID;
    public List<StatData> PlayerStats;
    public List<StatData> Stats => PlayerStats;
}