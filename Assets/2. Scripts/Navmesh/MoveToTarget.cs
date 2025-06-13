using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;           // 이동 목표 지점
    private NavMeshAgent agent;        // 내 NavMeshAgent


    void OnEnable()
    {
                // 내 NavMeshAgent 가져오기
        agent = GetComponent<NavMeshAgent>();

        // 목표 지점으로 이동 명령
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
    void Start()
    {

    }

    void Update()
    {
        // 목표 지점이 설정되어 있고 아직 도달하지 않았다면 계속 이동
        if (target != null && agent.remainingDistance > agent.stoppingDistance)
        {
            Debug.Log($"{target}+이동중");
            agent.SetDestination(target.position);
        }
    }
}
