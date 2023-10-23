using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 normalOffset;
    [SerializeField] private Vector3 policeChaseOffset;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Canvas canvas;

    private Vector3 currentOffset;
    private Camera mainCamera;
    private float transitionDuration = 3f;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        var panelHeight = panel.rect.height * canvas.scaleFactor;
        Vector2 resolutionOffset = mainCamera.ScreenToWorldPoint(new Vector2(0, panelHeight + 1));
        normalOffset = new Vector3(0, resolutionOffset.y + 1, normalOffset.z);
        currentOffset = normalOffset;
    }

    private void Update()
    {
        transform.position = new Vector3(0, targetTransform.position.y, 0) - currentOffset;
    }

    public void ChangeToPoliceOffset()
    {
        StartCoroutine(ChangeOffset(policeChaseOffset));
    }

    public void ChangeToNormalOffset()
    {
        StartCoroutine (ChangeOffset(normalOffset));
    }

    public IEnumerator ChangeOffset(Vector3 offset)
    {
        float currentTime = 0;
        Vector3 startOffset = currentOffset;

        while(currentTime <  transitionDuration) 
        {
            currentOffset = Vector3.Lerp(startOffset, offset, currentTime / transitionDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
