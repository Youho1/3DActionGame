using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // 拾ったアイテムのタイプ
    public enum PickUpType {
        Heal,
        Coin,
    }

    public PickUpType Type;
    //　回復量 or コインの数
    public int Value = 20;
    // プレイヤーがアイテムを拾ったら、アイテムを削除
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            other.gameObject.GetComponent<Player.PlayerController>().PickUpItem(this);
            Destroy(gameObject);
        }    
    }

}
