using System;
using UnityEngine;

public class LightVanishWall : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Transform playerLight; // プレイヤーのライトのtransform
    [SerializeField] private Light spotLight; // プレイヤーのライト
    [SerializeField] private LayerMask rayMask; // 障害物レイヤー

    [Header("設定")]
    [SerializeField] private float vanishDuration = 2f;

    private MeshRenderer rend;
    private Collider col;

    private bool isVanished = false; // 今、消失中かどうか
    private float vanishTImer = 0f;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        // すでに消えている状態ならタイマー処理だけ
        if(isVanished)
        {
            vanishTImer -= Time.deltaTime;
            if(vanishTImer <= 0f)
            {
                RestoreWall();
            }
            return;
        }

        bool hit = LightChecker.IsInSpotLight(
            playerLight,
            spotLight,
            transform,
            rayMask
            );

        if( hit )
        {
            VanishWall();
        }
    }

    private void VanishWall()
    {
        isVanished = true;
        vanishTImer = vanishDuration;

        rend.enabled = false;
        col.enabled = false;
    }

    private void RestoreWall()
    {
        isVanished = false;

        rend.enabled = true;
        col.enabled = true;
    }
}
