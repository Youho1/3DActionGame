using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Character : MonoBehaviour
    {

        protected CharacterController _characterController;
        public float MoveSpeed = 5f;
        protected Vector3 _movementVelocity;
        protected float _verticalVelocity;
        protected Animator _animator;
        protected float gravity = -20f;

        public bool IsPlayer = true;

        protected virtual void Awake() 
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void  FixedUpdate() 
        {
            if (_characterController.isGrounded == false) {
                _verticalVelocity = gravity;
            }else {
                _verticalVelocity = gravity * 0.3f;
            }
        }
    }

}
