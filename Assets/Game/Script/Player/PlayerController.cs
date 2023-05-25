using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : Character
    {

        //State Machine

        private PlayerInput _playerInput;
        private Health _health;
        // Player slides
        public float AttackSlideDuration = 0.4f;
        public float AttackSlideSpeed = 0.06f;
        protected override void Awake()
        {
            base.Awake();
            _playerInput = GetComponent<PlayerInput>();
            _health = GetComponent<Health>();
            _damageCaster = GetComponentInChildren<DamageCaster>();
        }

        private void CalculatePlayerMovement()
        {
            if (_playerInput.MouseButtonDown && _characterController.isGrounded)
            {
                SwitchStateTo(CharacterState.Attacking);
                return;
            }
            _movementVelocity.Set(_playerInput.HorizontalInput, 0f, _playerInput.VerticalInput);
            _movementVelocity.Normalize();
            _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;
            _animator.SetFloat("Speed", _movementVelocity.magnitude);
            _movementVelocity *= MoveSpeed * Time.deltaTime;
            if (_movementVelocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_movementVelocity);

            _animator.SetBool("AirBorne", !_characterController.isGrounded);
        }

        protected override void FixedUpdate()
        {
            switch (CurrentState)
            {
                case CharacterState.Normal:
                    CalculatePlayerMovement();
                    break;
                case CharacterState.Attacking:
                    _movementVelocity = Vector3.zero;
                    if (Time.time < attackStartTime + AttackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / AttackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * AttackSlideSpeed, Vector3.zero, lerpTime);
                    }
                    break;
            }

            base.FixedUpdate();
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _characterController.Move(_movementVelocity);
        }

        protected override void SwitchStateTo(CharacterState newState)
        {
            _playerInput.MouseButtonDown = false;
            base.SwitchStateTo(newState);
        }

        protected override void AttackAnimationEnds()
        {
            base.AttackAnimationEnds();
        }

        public override void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
        {
            if (_health != null)
            {
                _health.ApplyDamage(damage);
            }
        }

    }
}
