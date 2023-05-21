using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player{
    public class PlayerVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;

        public void Update_FootStrp(bool state) {
            if (state) {
                footStep.Play();
            }else {
                footStep.Stop();
            }              
        }
    }
}
