using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapPortal : MonoBehaviour
{
    public GameObject portalPos;
    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyController>(out var enemyController))
        {
            enemyController.Agent.Warp(portalPos.transform.position);
            // enemyController.AssignAttackPoint();
            enemyController.SetTargetPosition(target.transform.position);
        }

        // other.GetComponent<NavMeshAgent>().Warp(portalPos.transform.position);
        // 목적지 다시 지정
        // other.GetComponent<MoveToTarget>().target = target.transform;
        // other.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }
}