using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.015f; // Default magnitude
    private float shakeSpeed = 25f;      // Default speed

    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
        originalPosition = cameraTransform.localPosition;
    }

    public void Shake(float duration, float magnitude, float speed)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeSpeed = speed;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            Vector3 newPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            newPosition.z = originalPosition.z; // Maintain the camera's z position
            cameraTransform.localPosition = newPosition;

            shakeDuration -= Time.deltaTime * shakeSpeed;
        }
        else
        {
            // Reset the camera position
            cameraTransform.localPosition = originalPosition;
        }
    }
}
