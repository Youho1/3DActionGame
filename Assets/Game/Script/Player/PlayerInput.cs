using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {

    public class PlayerInput : MonoBehaviour
    {
        public float HorizontalInput;
        public float VerticalInput;
        public bool MouseButtonDown;


        private void Update() {
            if (!MouseButtonDown && Time.timeScale != 0) {
                MouseButtonDown = Input.GetMouseButtonDown(0);
            }

            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
        }

        private void OnDisable() {
            MouseButtonDown = false;
            HorizontalInput = 0;
            VerticalInput = 0;
        }
    }

}
