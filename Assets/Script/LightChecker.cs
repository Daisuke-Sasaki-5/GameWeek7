using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LightChecker : MonoBehaviour
{
    // ==== Transform版(壁用) ====
    public static bool IsInSpotLight(Transform spotLight,Light light,Transform target,LayerMask layerMask)
    {
        Vector3 dir = target.position - spotLight.position;
        float dist = dir.magnitude;

        // 距離判定
        if (dist > light.range) return false;

        // 角度判定
        float angle = Vector3.Angle(spotLight.forward, dir);
        if (angle > light.spotAngle * 0.5f) return false;

        // Raycastで遮蔽物チェック
        if(Physics.Raycast(spotLight.position,dir.normalized,out RaycastHit hit,light.range,layerMask))
        {
            return hit.collider.transform == target;
        }

        return false;
    }

    // ==== Vector3版(床用) ====
    public static bool IsInSpotLightPosition(Transform spotLight, Light light, Vector3 targetPos, LayerMask layerMask)
    {
        Vector3 dir = targetPos - spotLight.position;
        float dist = dir.magnitude;

        if(dist > light.range)return false;

        float angle = Vector3.Angle(spotLight.forward, dir);
        if(angle > light.spotAngle * 0.5f)return false;

        if (Physics.Raycast(spotLight.position, dir.normalized, out RaycastHit hit, light.range, layerMask))
        {
            // ヒット位置とターゲット位置が近ければtrueとする
            return Vector3.Distance(hit.point,targetPos) < 0.3f;
        }

        return false;
    }
}
