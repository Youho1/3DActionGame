using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Character : MonoBehaviour
    {

        private CharacterController _characterController;
        public float MoveSpeed = 5f;
        private Vector3 _movementVelocity;
        private PlayerInput _playerInput;

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
            _characterController.Move(_movementVelocity);
        }
    }

}
