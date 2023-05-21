using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {

    public class PlayerInput : MonoBehaviour
    {
        public float HorizontalInput;
        public float VerticalInput;

        private void Update() {
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
        }

        private void OnDisable() {
            HorizontalInput = 0;
            VerticalInput = 0;
        }
    }

}
