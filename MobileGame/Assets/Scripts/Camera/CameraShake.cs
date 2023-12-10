using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Transform cameraTransform;
    private Vector3 originalPosition;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.015f; // Default magnitude
    private float shakeSpeed = 25f;      // Default speed

    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
        originalPosition = cameraTransform.localPosition;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void Shake(float duration, float magnitude, float speed)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeSpeed = speed;
    }

    private void Update()
    {
        if (player.activeSelf == true)
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

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
