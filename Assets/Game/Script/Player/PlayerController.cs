using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : Character
    {

        //State Machine

        private PlayerInput _playerInput;
        private Health _health;
        // Player slides
        public float AttackSlideDuration = 0.4f;
        public float AttackSlideSpeed = 0.06f;

        private Vector3 impactOnCharacter;

        public bool IsInvincible;
        public float invincibleDuration = 2f;

        public int Coin;

        private float attackAnimationDuration;
        protected override void Awake()
        {
            base.Awake();
            _playerInput = GetComponent<PlayerInput>();
            _health = GetComponent<Health>();
            _damageCaster = GetComponentInChildren<DamageCaster>();
        }

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

        protected override void FixedUpdate()
        {
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

            base.FixedUpdate();
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _characterController.Move(_movementVelocity);
            _movementVelocity = Vector3.zero;
        }

        public override void SwitchStateTo(CharacterState newState)
        {
            _playerInput.ClearCache();
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
                    GetComponent<PlayerVFXManager>().StopBlade();
                    break;
                case CharacterState.Dead:
                    return;
                case CharacterState.BeingHit:
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

                    IsInvincible = true;
                    StartCoroutine(DelayCanceInvincible());
                    break;
                case CharacterState.Slide:
                    _animator.SetTrigger("Slide");
                    break;
            }

            CurrentState = newState;

            Debug.Log("Switch to" + CurrentState);
        }

        protected override void AttackAnimationEnds()
        {
            base.AttackAnimationEnds();
        }

        public override void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
        {
            if (IsInvincible)
            {
                return;
            }
            if (_health != null)
            {
                _health.ApplyDamage(damage);
            }
            StartCoroutine(MaterialBlink());
            SwitchStateTo(CharacterState.BeingHit);
            AddImpact(attackerPos, 10f);
        }

        IEnumerator DelayCanceInvincible()
        {
            yield return new WaitForSeconds(invincibleDuration);
            IsInvincible = false;
        }

        private void AddImpact(Vector3 attackerPos, float force)
        {
            Vector3 impactDir = transform.position - attackerPos;
            impactDir.Normalize();
            impactDir.y = 0;
            impactOnCharacter = impactDir * force;
        }

        private void BeingHitAnimationEnds()
        {
            SwitchStateTo(CharacterState.Normal);
        }

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

        private void AddHealth(int health)
        {
            _health.AddHealth(health);
            GetComponent<PlayerVFXManager>().PlayHealVFX();
        }

        private void AddCoin(int coin)
        {
            Coin += coin;
        }
    }
}
