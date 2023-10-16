using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : Character
    {

        //　入力を受け取るPlayerInput型
        private PlayerInput _playerInput;
        // HPコンポーネント
        private Health _health;
        // １つの攻撃期間を調整
        public float AttackSlideDuration = 0.4f;
        //　攻撃速度を調整
        public float AttackSlideSpeed = 0.06f;
        // 攻撃を受けた時のインパクトの方向
        private Vector3 impactOnCharacter;

        // 無敵の状態かどうか
        public bool IsInvincible;
        // 無敵の状態になった時の期間
        public float invincibleDuration = 2f;
        // コインの数
        public int Coin;
        // 攻撃アニメーションの期間
        private float attackAnimationDuration;
        // 初期化
        protected override void Awake()
        {
            base.Awake();
            _playerInput = GetComponent<PlayerInput>();
            _health = GetComponent<Health>();
            _damageCaster = GetComponentInChildren<DamageCaster>();
        }
        // 移動を制御
        private void CalculatePlayerMovement()
        {
            if (_playerInput.MouseButtonDown && _characterController.isGrounded)
            {
                SwitchStateTo(CharacterState.Attacking);
                return;
            }
            else if (_playerInput.SpeaceKeyDown && _characterController.isGrounded)
            {
                SwitchStateTo(CharacterState.Slide);
                return;
            }
            _movementVelocity.Set(_playerInput.HorizontalInput, 0f, _playerInput.VerticalInput);
            _movementVelocity.Normalize();
            _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;
            _animator.SetFloat("Speed", _movementVelocity.magnitude);
            _movementVelocity *= MoveSpeed * Time.deltaTime;
            if (_movementVelocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_movementVelocity);

            _animator.SetBool("AirBorne", !_characterController.isGrounded);
        }

        // 状態管理に（毎フーレムチェック）
        protected override void FixedUpdate()
        {
            // 状態管理（処理）
            switch (CurrentState)
            {
                case CharacterState.Normal:
                    CalculatePlayerMovement();
                    break;
                case CharacterState.Attacking:
                    if (Time.time < attackStartTime + AttackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / AttackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * AttackSlideSpeed, Vector3.zero, lerpTime);
                    }
                    if (_playerInput.MouseButtonDown && _characterController.isGrounded)
                    {
                        string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                        if (currentClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                        {
                            _playerInput.MouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attacking);

                            CalculatePlayerMovement();
                        }
                    }
                    break;
                case CharacterState.Dead:
                    return;
                case CharacterState.BeingHit:
                    if (impactOnCharacter.magnitude > 0.2f)
                    {
                        _movementVelocity = impactOnCharacter * Time.deltaTime;
                    }
                    impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
                    break;
                case CharacterState.Slide:
                    _movementVelocity = transform.forward * SlideSpeed * Time.deltaTime;
                    break;
            }
            // 継承元のメソッドを呼び出す
            base.FixedUpdate();
            // 速度を計算
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            // 移動を処理
            _characterController.Move(_movementVelocity);
            // 速度を初期化
            _movementVelocity = Vector3.zero;
        }

        // 状態管理（変換）
        public override void SwitchStateTo(CharacterState newState)
        {
            // 初期化
            _playerInput.ClearCache();
            //Exiting state 状態から離れる時
            switch (CurrentState)
            {
                case CharacterState.Normal:
                    break;
                case CharacterState.Attacking:
                    if (_damageCaster != null)
                    {
                        DisableDamageCaster();
                    }
                    GetComponent<PlayerVFXManager>().StopBlade();
                    break;
                case CharacterState.Dead:
                    return;
                case CharacterState.BeingHit:
                    break;
            }
            //Entering state 状態に入る時
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

                    IsInvincible = true;
                    StartCoroutine(DelayCanceInvincible());
                    break;
                case CharacterState.Slide:
                    _animator.SetTrigger("Slide");
                    break;
            }

            // 現在の状態を変更
            CurrentState = newState;

            Debug.Log("Switch to" + CurrentState);
        }
        // 攻撃アニメーションが終わったら
        protected override void AttackAnimationEnds()
        {
            base.AttackAnimationEnds();
        }
        // ダメージを計算
        public override void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
        {
            if (IsInvincible)
            {
                return;
            }
            if (_health != null)
            {
                // ダメージを計算
                _health.ApplyDamage(damage);
            }
            // マテリアルを適用
            StartCoroutine(MaterialBlink());
            // 状態を変更（攻撃された）
            SwitchStateTo(CharacterState.BeingHit);
            // インパクトを追加
            AddImpact(attackerPos, 10f);
        }

        // 無敵の状態から解除
        IEnumerator DelayCanceInvincible()
        {
            yield return new WaitForSeconds(invincibleDuration);
            IsInvincible = false;
        }
        // ダメージを受けた時のインパクトのベクトルを計算
        private void AddImpact(Vector3 attackerPos, float force)
        {
            Vector3 impactDir = transform.position - attackerPos;
            impactDir.Normalize();
            impactDir.y = 0;
            impactOnCharacter = impactDir * force;
        }
        // 攻撃されたアニメーションが終わったら
        private void BeingHitAnimationEnds()
        {
            SwitchStateTo(CharacterState.Normal);
        }
        // アイテムを拾ったら
        public void PickUpItem(PickUp item)
        {
            switch (item.Type)
            {
                case PickUp.PickUpType.Heal:
                    AddHealth(item.Value);
                    break;
                case PickUp.PickUpType.Coin:
                    AddCoin(item.Value);
                    break;
            }
        }
        // HPを回復する
        private void AddHealth(int health)
        {
            _health.AddHealth(health);
            GetComponent<PlayerVFXManager>().PlayHealVFX();
        }
        // コインを追加
        private void AddCoin(int coin)
        {
            Coin += coin;
        }
    }
}
