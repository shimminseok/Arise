using System.Collections;
using UnityEngine;

public class TutorialWaveController : MonoBehaviour
{
    [SerializeField] private GameObject enemyManagerObject;
    [SerializeField] private float delayAfterTowerPhase = 3f;

    private void Awake()
    {
        if (enemyManagerObject != null)
            enemyManagerObject.SetActive(false); // 시작 시 꺼두기
    }

    public void StartFirstWaveAfterDelay()
    {
        StartCoroutine(SpawnTutorialWave());
    }

    private IEnumerator SpawnTutorialWave()
    {
        yield return new WaitForSecondsRealtime(delayAfterTowerPhase); // timeScale = 0이어도 작동

        if (enemyManagerObject != null)
        {
            enemyManagerObject.SetActive(true);
            Debug.Log("[TutorialWaveController] 튜토리얼 웨이브 시작됨.");
        }
    }
}