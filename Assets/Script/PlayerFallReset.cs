using System;
using UnityEngine;

public class PlayerFallReset : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // CharacterControllerの場合はvelocityをリセット
            PlayerControll playerControll = other.GetComponent<PlayerControll>();
            if(playerControll != null)
            {
                playerControll.ResetVelocity();
            }

            // プレイヤーをリスポーン
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // 移動中の衝突を回避
                other.transform.position = respawnPoint.position;
                other.transform.rotation = respawnPoint.rotation;
                cc.enabled = true;
            }
            else
            {
                other.transform.position = respawnPoint.position;
                other.transform.rotation = respawnPoint.rotation;
            }
        }
    }
}
