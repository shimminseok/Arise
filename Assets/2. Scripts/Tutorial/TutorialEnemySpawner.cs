using UnityEngine;

public class TutorialEnemySpawner : MonoBehaviour
{
    public static TutorialEnemySpawner Instance { get; private set; }

    [SerializeField] private MonsterSO tutorialMonster;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnFirstWave()
    {
        TutorialEnemyManager.Instance.SpawnWave(tutorialMonster, 3);
    }

    public void SpawnNextWave()
    {
        TutorialEnemyManager.Instance.SpawnWave(tutorialMonster, 5);
    }
}