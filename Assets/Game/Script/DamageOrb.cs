using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    public float Speed = 8f;
    public int Damage = 10;
    public ParticleSystem HitVFX;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 侵入したキャラクターのタイプを判定　（Player.CharacterとはPlayerフォルダのCharacter.csファイルを指す）
        Player.Character cc = other.gameObject.GetComponent<Player.Character>();
        // ダメージを適用
        if (cc != null && cc.IsPlayer)
        {
        // 位置を渡す理由はダメージを受ける時の衝撃の方向を計算するため
            cc.ApplyDamage(Damage, transform.position);
        }
        // VFXを生成
        Instantiate(HitVFX, transform.position, Quaternion.identity);
        // 弾を削除
        Destroy(gameObject);
    }
}
