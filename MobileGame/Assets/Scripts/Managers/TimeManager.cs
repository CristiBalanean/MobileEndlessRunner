using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public event EventHandler<TimeSpan> WorldTimeChanged;

    public UnityEvent notifyIsDay;
    public UnityEvent notifyIsNight;

    [SerializeField] private float dayLength;
    private float minuteLength => dayLength / 1440;

    private TimeSpan currentTime;
    public bool isDay = true; // Track the current state

    private void Awake()
    {
        instance = this;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        currentTime = TimeSpan.Zero;
        StartCoroutine(AddMinute());
    }

    private IEnumerator AddMinute()
    {
        while (true)
        {
            currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, currentTime);

            if (currentTime.TotalMinutes >= 1440)
            {
                // Reset time to the start of the day
                currentTime = TimeSpan.Zero;
            }

            bool isDayNow = currentTime.Hours >= 0 && currentTime.Hours < 12;

            if (isDayNow && !isDay)
            {
                isDay = true;
                notifyIsDay?.Invoke();
            }
            else if (!isDayNow && isDay)
            {
                isDay = false;
                notifyIsNight?.Invoke();
            }

            yield return new WaitForSeconds(minuteLength);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        if(enabled)
            StartCoroutine(AddMinute());
        else
            StopAllCoroutines();
    }
}
