using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class Enemy_01 : Player.Character
    {
        private UnityEngine.AI.NavMeshAgent _navMeshAgent;
        private Transform TargetPlayer;
        protected override void Awake() 
        {
            base.Awake();
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            TargetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = MoveSpeed;
        }

        private void CalculateEnemyMovement() 
        {
            if (Vector3.Distance(TargetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
            {
                _navMeshAgent.SetDestination(TargetPlayer.position);
                _animator.SetFloat("Speed", 0.2f);
            }
            else
            {
                _navMeshAgent.SetDestination(transform.position);
                _animator.SetFloat("Speed", 0f);
            }
        }

        protected override void FixedUpdate()
        {
            CalculateEnemyMovement();
            base.FixedUpdate();
        }
    }

}