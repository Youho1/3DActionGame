using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player{
    public class PlayerController : Character 
    {

        private PlayerInput _playerInput;
        protected override void Awake() 
        {
            base.Awake();
            _playerInput = GetComponent<PlayerInput>();
        }

         private void CalculatePlayerMovement() 
         {
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
            CalculatePlayerMovement();
            base.FixedUpdate();
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _characterController.Move(_movementVelocity);
        }
    }
}
