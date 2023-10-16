using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Health : MonoBehaviour
    {
        // 最大のHP
        public int MaxHealth;
        // 現在のHP
        public int CurrentHealth;
        // コンポーネントにアタッチするキャラクター
        protected Character _character;

        // 初期化
        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
            _character = GetComponent<Character>();
        }

        // ダメージを計算する
        public virtual void ApplyDamage(int damage)
        {
            CurrentHealth -= damage;
            Debug.Log(gameObject.name + "took damage: " + damage);
            Debug.Log(gameObject.name + " CurrentHealth: " + CurrentHealth);

            CheckHealth();
        }

        // HPをチェックし、生と死を確認する
        protected void CheckHealth()
        {
            if (CurrentHealth <= 0)
            {
                _character.SwitchStateTo(Character.CharacterState.Dead);
            }
        }

        // 回復アイテムなどを拾った時に、HPを回復する
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

