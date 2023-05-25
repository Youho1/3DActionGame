using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Character : MonoBehaviour
    {

        protected CharacterController _characterController;
        public float MoveSpeed = 5f;
        protected Vector3 _movementVelocity;
        protected float _verticalVelocity;
        protected Animator _animator;
        protected float gravity = -20f;

        protected float attackStartTime;
        public bool IsPlayer = true;


        protected DamageCaster _damageCaster;

        public enum CharacterState
        {
            Normal,
            Attacking,
        }
        public CharacterState CurrentState;
        protected virtual void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void FixedUpdate()
        {
            if (_characterController.isGrounded == false)
            {
                _verticalVelocity = gravity;
            }
            else
            {
                _verticalVelocity = gravity * 0.3f;
            }
        }

        protected virtual void SwitchStateTo(CharacterState newState)
        {
            // clear cache

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
            }

            CurrentState = newState;

            Debug.Log("Switch to" + CurrentState);
        }

        protected virtual void AttackAnimationEnds()
        {
            SwitchStateTo(CharacterState.Normal);
        }

        public virtual void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
        {

        }

        public void EnableDamageCaster()
        {
            _damageCaster.EnableDamageCaster();
        }

        public void DisableDamageCaster()
        {
            _damageCaster.DisableDamageCaster();
        }
    }
}
