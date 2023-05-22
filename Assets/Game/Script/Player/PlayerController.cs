using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : Character
    {

        //State Machine
        public enum PlayerState
        {
            Normal,
            Attacking,
        }
        public PlayerState CurrentState;

        private PlayerInput _playerInput;
        protected override void Awake()
        {
            base.Awake();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void CalculatePlayerMovement()
        {
            if (_playerInput.MouseButtonDown && _characterController.isGrounded)
            {
                SwitchStateTo(PlayerState.Attacking);
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
                case PlayerState.Normal:
                    CalculatePlayerMovement();
                    break;
                case PlayerState.Attacking:
                    break;
            }

            base.FixedUpdate();
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _characterController.Move(_movementVelocity);
        }


        private void SwitchStateTo(PlayerState newState)
        {
            // clear cache
            _playerInput.MouseButtonDown = false;
            //Exiting state
            switch (CurrentState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Attacking:
                    break;
            }
            //Entering state
            switch (newState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Attacking:
                    _animator.SetTrigger("Attack");
                    break;
            }

            CurrentState = newState;

            Debug.Log("Switch to" + CurrentState);
        }

        public void AttackAnimationEnds()
        {
            SwitchStateTo(PlayerState.Normal);
        }
    }
}
