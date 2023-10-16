using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 入力を受け取るためのクラス
namespace Player
{

    public class PlayerInput : MonoBehaviour
    {
        public float HorizontalInput;
        public float VerticalInput;
        public bool MouseButtonDown;
        public bool SpeaceKeyDown;


        private void Update()
        {
            if (!MouseButtonDown && Time.timeScale != 0)
            {
                MouseButtonDown = Input.GetMouseButtonDown(0);
            }

            if (!SpeaceKeyDown && Time.timeScale != 0)
            {
                SpeaceKeyDown = Input.GetKeyDown(KeyCode.Space);
            }

            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
        }

        private void OnDisable()
        {
            ClearCache();
        }

        public void ClearCache()
        {
            MouseButtonDown = false;
            SpeaceKeyDown = false;
            HorizontalInput = 0;
            VerticalInput = 0;
        }
    }

}
