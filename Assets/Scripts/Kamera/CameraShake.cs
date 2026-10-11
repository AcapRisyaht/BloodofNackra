using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
   public IEnumerator Shake(float duration, float magnitude)
    {
        CameraFollow follow = GetComponent<CameraFollow>();
        if (follow != null) follow.isShaking = true;

        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            
            float x = Mathf.Sin(elapsed * 10f) * magnitude;
            float y = Mathf.Sin(elapsed * 10f) * magnitude * 0.5f; // Lebih kecik pada Y

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        if (follow != null) follow.isShaking = false;
    }
}
