using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ParticleSystem rainParticleSystem;

    [SerializeField] private Vector3 offset;

    void Start()
    {
        if (mainCamera == null)
        {
            // If the mainCamera is not assigned, try to find it in the scene
            mainCamera = Camera.main;
        }

        if (rainParticleSystem == null)
        {
            // If the rainParticleSystem is not assigned, try to find it in the scene
            rainParticleSystem = GetComponent<ParticleSystem>();
        }

        if (mainCamera != null && rainParticleSystem != null)
        {
            // Calculate the initial offset between the camera and the particle system
            offset = transform.position - mainCamera.transform.position;
        }
        else
        {
            Debug.LogError("Camera or ParticleSystem not assigned.");
        }
    }

    void LateUpdate()
    {
        // Update the position of the particle system to follow the camera's position
        if (mainCamera != null && rainParticleSystem != null)
        {
            transform.position = mainCamera.transform.position + offset;
        }
    }
}