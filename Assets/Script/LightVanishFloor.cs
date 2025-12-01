using UnityEngine;

public class LightVanishFloor : MonoBehaviour
{
    [Header("éQè∆")]
    [SerializeField] private Transform playerLight;
    [SerializeField] private Light spotLight;
    [SerializeField] private LayerMask rayMask;

    [Header("ê›íË")]
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

        // å©Ç¶ÇÈÇÃÇÕvisibleTImerÇ™écÇ¡ÇƒÇ¢ÇÈä‘
        rend.enabled = visibleTimer > 0f;
    }
}