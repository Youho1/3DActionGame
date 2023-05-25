using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy_01 : Player.Character
    {
        private UnityEngine.AI.NavMeshAgent _navMeshAgent;
        private Transform TargetPlayer;
        Health _health;
        private DamageCaster _damageCaster;
        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            TargetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = MoveSpeed;
            _health = GetComponent<Health>();
            _damageCaster = GetComponentInChildren<DamageCaster>();
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

                SwitchStateTo(CharacterState.Attacking);
            }
        }

        protected override void SwitchStateTo(CharacterState newState)
        {
            base.SwitchStateTo(newState);
        }
        protected override void FixedUpdate()
        {
            CalculateEnemyMovement();
            base.FixedUpdate();
        }

        protected override void AttackAnimationEnds()
        {
            base.AttackAnimationEnds();
        }

        public override void ApplyDamage(int damage, Vector3 attackerPos = default)
        {
            if (_health != null)
            {
                _health.ApplyDamage(damage);
            }

            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackerPos);
        }
    }

}