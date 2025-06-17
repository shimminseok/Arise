using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObject/Wave")]
public class WaveSO : ScriptableObject
{
    [System.Serializable]
    public struct SpawnInfo
    {
        public MonsterSO monster;
        public int count;
    }

    public List<SpawnInfo> spawnList = new List<SpawnInfo>();
    public float waveDelay = 3f; // 웨이브 간 딜레이 시간
}