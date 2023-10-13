using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]

public class WorldLight : MonoBehaviour
{
    private Light2D worldLight;

    [SerializeField] private TimeManager timeManager;

    [SerializeField] private Gradient colorGradient;

    private void Awake()
    {
        worldLight = GetComponent<Light2D>();
        timeManager.WorldTimeChanged += OnWorldTimeChange;
    }

    private void OnDestroy()
    {
        timeManager.WorldTimeChanged -= OnWorldTimeChange;
    }

    private void OnWorldTimeChange(object sender, TimeSpan newTime)
    {
        worldLight.color = colorGradient.Evaluate(PercentOfDay(newTime));
    }

    private float PercentOfDay(TimeSpan timeSpan)
    {
        return (float)timeSpan.TotalMinutes % 1440 / 1440;
    }
}
