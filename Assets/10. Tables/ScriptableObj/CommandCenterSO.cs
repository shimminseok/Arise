using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewCommandCenter", menuName = "ScriptableObject/CommandCenter", order = 0)]
public class CommandCenterSO : ScriptableObject, IStatProvider
{
    public List<StatData> CommandStats;
    public List<StatData> Stats => CommandStats;
}