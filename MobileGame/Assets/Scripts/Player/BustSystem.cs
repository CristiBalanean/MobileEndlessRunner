using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BustSystem : MonoBehaviour
{
    [SerializeField] private UnityEvent deathTrigger;

    [SerializeField] private float countdownUntilBust;
    public float currentCountdown;
    public int currentCount;

    [SerializeField] private LayerMask policeMask;

    [SerializeField] private Image bustUI;

    private void Start()
    {
        currentCountdown = 0;

        InvokeRepeating("CheckPoliceCount", 0, 0.5f);
    }

    private void Update()
    {
        float currentSpeed = CarMovement.Instance.GetSpeed();

        if (currentCount >= 4 && currentSpeed <= 45 && PoliceEvent.instance.hasStarted)
        {
            currentCountdown += Time.deltaTime;

            if (currentCountdown >= countdownUntilBust)
            {
                deathTrigger?.Invoke();
            }
        }
        else
        {
            // Gradually decrease the timer when there are fewer than 4 cars near
            float decreaseRate = 0.5f;  // Adjust this value based on how quickly you want the countdown to decrease

            currentCountdown = Mathf.Clamp(currentCountdown - decreaseRate * Time.deltaTime, 0, countdownUntilBust);
        }

        bustUI.fillAmount = currentCountdown / countdownUntilBust;
        if (currentCountdown > 0)
            bustUI.gameObject.SetActive(true);
        else
            bustUI.gameObject.SetActive(false);
    }

    private void CheckPoliceCount()
    {
        Collider2D[] policeColliders = Physics2D.OverlapCircleAll(transform.position, 2.5f, policeMask);

        // Use a HashSet to store unique root transforms
        HashSet<Transform> uniqueRootTransforms = new HashSet<Transform>();

        foreach (Collider2D collider in policeColliders)
        {
            // Add the collider's root transform to the HashSet
            uniqueRootTransforms.Add(collider.transform.root);
        }

        // Update the currentCount based on the number of unique root transforms
        currentCount = uniqueRootTransforms.Count;
    }
}
