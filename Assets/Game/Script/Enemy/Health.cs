using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy{
    public class Health : Player.Health {

        protected override void Awake()
        {
            base.Awake();
        }

        public override void ApplyDamage(int damage)
        {
            base.ApplyDamage(damage);
        }
    }
}