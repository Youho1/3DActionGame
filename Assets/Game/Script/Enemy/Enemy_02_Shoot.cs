using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy_02_Shoot : MonoBehaviour
    {
        public Transform ShootingPoint;
        public GameObject DamageOrb;
        private Enemy_01 cc;
        private void Awake() {
            cc = GetComponent<Enemy_01>();
        }
         public void ShootTheDamageOrb()
         {
             Instantiate(DamageOrb, ShootingPoint.position, Quaternion.LookRotation(ShootingPoint.forward));
         }

        private void Update() {
            cc.RotateToTarget();
        }
    }
}
