using UnityEngine;

public class LightVanishFloor : MonoBehaviour
{
    // プレイヤーに設定しているライトが当たったら消える処理

    [Header("参照")]
    [SerializeField] private Transform playerLight;
    [SerializeField] private Light spotLight;
    [SerializeField] private LayerMask rayMask;

    [Header("設定")]
    [SerializeField] private float visibleDuration = 2f;

    private MeshRenderer rend;
    private float visibleTimer = 0f;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
    }

    void Update()
    {
        bool inLight = LightChecker.IsInSpotLight(
            playerLight,
            spotLight,
            transform,
            rayMask
            );

        if (inLight)
        {
            visibleTimer = visibleDuration;
        }

        if (visibleTimer > 0f)
        {
            visibleTimer -= Time.deltaTime;
        }

        // 見えるのはvisibleTImerが残っている間
        rend.enabled = visibleTimer > 0f;
    }
}