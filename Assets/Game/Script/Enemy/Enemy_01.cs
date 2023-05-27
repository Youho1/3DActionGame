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
        public GameObject ItemToDrop;
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

        public override void SwitchStateTo(CharacterState newState)
        {
            //Exiting state
            switch (CurrentState)
            {
                case CharacterState.Normal:
                    break;
                case CharacterState.Attacking:
                    if (_damageCaster != null)
                    {
                        DisableDamageCaster();
                    }
                    break;
                case CharacterState.Dead:
                    return;
                case CharacterState.BeingHit:
                    break;


            }
            //Entering state
            switch (newState)
            {
                case CharacterState.Normal:
                    break;
                case CharacterState.Attacking:
                    _animator.SetTrigger("Attack");
                    attackStartTime = Time.time;
                    break;
                case CharacterState.Dead:
                    _characterController.enabled = false;
                    _animator.SetTrigger("Dead");
                    StartCoroutine(MaterialDissolve());
                    break;
                case CharacterState.BeingHit:
                    _animator.SetTrigger("BeingHit");
                    break;


            }

            CurrentState = newState;

            Debug.Log("Switch to" + CurrentState);
        }
        protected override void FixedUpdate()
        {
            switch (CurrentState)
            {
                case CharacterState.Normal:
                    CalculateEnemyMovement();
                    break;
                case CharacterState.Attacking:
                    break;
                case CharacterState.Dead:
                    return;

            }

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

            StartCoroutine(MaterialBlink());
        }

        protected override IEnumerator MaterialDissolve()
        {
            yield return null;
            StartCoroutine(base.MaterialDissolve());
            DropItem();
        }

        public void DropItem()
        {
            if (ItemToDrop != null)
            {
                Instantiate(ItemToDrop, transform.position, Quaternion.identity);
            }
        }

        private void BeingHitAnimationEnds()
        {
            SwitchStateTo(CharacterState.Normal);
        }
        
        public void RotateToTarget()
        {
            if (CurrentState != CharacterState.Dead)
            {
                transform.LookAt(TargetPlayer, Vector3.up);
            }
        }
    }

}