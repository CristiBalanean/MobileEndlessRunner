using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude, float maxSpeed)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        // Pre-generate a set of random offsets
        List<Vector2> randomOffsets = new List<Vector2>();
        int numberOfOffsets = 100; // Adjust this number based on how many unique offsets you need
        for (int i = 0; i < numberOfOffsets; i++)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            randomOffsets.Add(new Vector2(x, y).normalized * magnitude);
        }

        int offsetIndex = 0;

        while (elapsed < duration)
        {
            // Cycle through the pre-generated random offsets
            Vector2 offset = randomOffsets[offsetIndex];
            offsetIndex = (offsetIndex + 1) % numberOfOffsets;

            float x = offset.x;
            float y = offset.y;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

}
