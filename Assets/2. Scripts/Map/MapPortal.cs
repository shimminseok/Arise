using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapPortal : MonoBehaviour
{
    public GameObject portalPos;
    public Collider target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyController>(out var enemyController))
        {
            enemyController.Agent.Warp(portalPos.transform.position);
            enemyController.AssignAttackPoint();
            // enemyController.SetTargetPosition(target.ClosestPoint(enemyController.transform.position));
            // enemyController.SetTargetPosition(target.transform.position);
        }

        if (other.TryGetComponent<BossController>(out var bossController))
        {
            bossController.Agent.Warp(portalPos.transform.position);
            bossController.AssignAttackPoint();
        }
        // other.GetComponent<NavMeshAgent>().Warp(portalPos.transform.position);
        // 목적지 다시 지정
        // other.GetComponent<MoveToTarget>().target = target.transform;
        // other.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }
}