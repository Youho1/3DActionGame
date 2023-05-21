using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Character : MonoBehaviour
    {

        private CharacterController _characterController;
        public float MoveSpeed = 5f;
        private Vector3 _movementVelocity;
        private float _verticalVelocity;
        private PlayerInput _playerInput;

        private float gravity = -20f;
        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void CalculatePlayerMovement() {
            _movementVelocity.Set(_playerInput.HorizontalInput, 0f, _playerInput.VerticalInput);
            _movementVelocity.Normalize();
            _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;
            _movementVelocity *= MoveSpeed * Time.deltaTime;
        }

        private void FixedUpdate() {
            CalculatePlayerMovement();

            if (_characterController.isGrounded == false) {
                _verticalVelocity = gravity;
            }else {
                _verticalVelocity = gravity * 0.3f;
            }
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _characterController.Move(_movementVelocity);
        }
    }

}
