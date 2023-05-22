using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy {
    public class EnemyVFXManager : MonoBehaviour
    {
        public VisualEffect FootStep;
        
        public void BurstFootStep() {
            FootStep.SendEvent("OnPlay");
        }
    }
}
