using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Health : MonoBehaviour
    {
        public int MaxHealth;
        public int CurrentHealth;

        protected Character _character;

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
            _character = GetComponent<Character>();
        }

        public virtual void ApplyDamage(int damage)
        {
            CurrentHealth -= damage;
            Debug.Log(gameObject.name + "took damage: " + damage);
            Debug.Log(gameObject.name + " CurrentHealth: " + CurrentHealth);

            CheckHealth();
        }

        protected void CheckHealth()
        {
            if (CurrentHealth <= 0)
            {
                _character.SwitchStateTo(Character.CharacterState.Dead);
            }
        }

        public void AddHealth(int health)
        {
            CurrentHealth += health;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
    }
}

