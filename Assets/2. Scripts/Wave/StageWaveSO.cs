using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageWaves", menuName = "ScriptableObject/StageWaves")]
public class StageWaveSO : ScriptableObject
{
    public List<WaveSO> waves = new List<WaveSO>();
}