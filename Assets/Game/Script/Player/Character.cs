using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Character : MonoBehaviour
    {

        protected CharacterController _characterController;
        public float MoveSpeed = 5f;
        protected Vector3 _movementVelocity;
        protected float _verticalVelocity;
        protected Animator _animator;
        protected float gravity = -20f;

        protected float attackStartTime;
        public bool IsPlayer = true;


        protected DamageCaster _damageCaster;

        public enum CharacterState
        {
            Normal,
            Attacking,
            Dead,
            BeingHit,
            Slide,
        }
        public CharacterState CurrentState;

        protected MaterialPropertyBlock _materialPropertBlock;
        protected SkinnedMeshRenderer _skinnedMeshRender;

        public float SlideSpeed = 9f;
        // 初期化
        protected virtual void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();


            _skinnedMeshRender = GetComponentInChildren<SkinnedMeshRenderer>();
            _materialPropertBlock = new MaterialPropertyBlock();
            _skinnedMeshRender.GetPropertyBlock(_materialPropertBlock);
        }
        // 重力制御
        protected virtual void FixedUpdate()
        {
            if (_characterController.isGrounded == false)
            {
                _verticalVelocity = gravity;
            }
            else
            {
                _verticalVelocity = gravity * 0.3f;
            }
        }
        // 状態遷移
        public virtual void SwitchStateTo(CharacterState newState)
        {
            // clear cache

            //Exiting state
            switch (CurrentState)
            {
                case CharacterState.Normal:
                    break;
                case CharacterState.Attacking:
                    if (_damageCaster != null)
                    {
                        DisableDamageCaster();
                    }
                    break;
                case CharacterState.Dead:
                    return;
                case CharacterState.BeingHit:
                    break;
                case CharacterState.Slide:
                    break;
            }
            //Entering state
            switch (newState)
            {
                case CharacterState.Normal:
                    break;
                case CharacterState.Attacking:
                    _animator.SetTrigger("Attack");
                    attackStartTime = Time.time;
                    break;
                case CharacterState.Dead:
                    _characterController.enabled = false;
                    _animator.SetTrigger("Dead");
                    StartCoroutine(MaterialDissolve());
                    break;
                case CharacterState.BeingHit:
                    _animator.SetTrigger("BeingHit");
                    break;
                case CharacterState.Slide:
                    _animator.SetTrigger("Slide");
                    break;
            }

            CurrentState = newState;

            Debug.Log("Switch to" + CurrentState);
        }
        // 攻撃アニメーションが終わったら
        protected virtual void AttackAnimationEnds()
        {
            SwitchStateTo(CharacterState.Normal);
        }

        public virtual void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
        {

        }

        public void EnableDamageCaster()
        {
            _damageCaster.EnableDamageCaster();
        }

        public void DisableDamageCaster()
        {
            _damageCaster.DisableDamageCaster();
        }

        // 死亡時のマテリアルの制御
        protected IEnumerator MaterialBlink()
        {
            _materialPropertBlock.SetFloat("_blink", 0.4f);
            _skinnedMeshRender.SetPropertyBlock(_materialPropertBlock);

            yield return new WaitForSeconds(0.2f);

            _materialPropertBlock.SetFloat("_blink", 0f);
            _skinnedMeshRender.SetPropertyBlock(_materialPropertBlock);

        }
        // 死亡時のマテリアルの制御
        protected virtual IEnumerator MaterialDissolve()
        {
            yield return new WaitForSeconds(2);

            float dissolveTimeDuration = 2.0f;
            float currentDissolveTime = 0;
            float dissolveHight_start = 20f;
            float dissolveHight_target = -10f;
            float dissolveHight;

            _materialPropertBlock.SetFloat("_enableDissolve", 1f);
            _skinnedMeshRender.SetPropertyBlock(_materialPropertBlock);

            while (currentDissolveTime < dissolveTimeDuration)
            {
                currentDissolveTime += Time.deltaTime;
                dissolveHight = Mathf.Lerp(dissolveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);
                _materialPropertBlock.SetFloat("_dissolve_height", dissolveHight);
                _skinnedMeshRender.SetPropertyBlock(_materialPropertBlock);
                yield return null;
            }
        }
        // 攻撃された斬撃のVFXが終わったら
        public void SlideAnimationEnds()
        {
            SwitchStateTo(CharacterState.Normal);
        }
    }
}
