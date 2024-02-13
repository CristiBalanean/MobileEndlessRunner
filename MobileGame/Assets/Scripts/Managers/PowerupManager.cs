using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerupManager : MonoBehaviour
{
    public delegate IEnumerator fillBar();
    public event fillBar FillBar;
    public delegate IEnumerator emptyBar();
    public event emptyBar EmptyBar;

    public PowerUp currentPowerup;
    public bool isActive = false;
    private bool isShield = false;

    [SerializeField] private ParticleSystem speedLines;
    [SerializeField] private ParticleSystem nitroParticle;
    [SerializeField] private GameObject shieldGO;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private GameObject slowdownGO;
    [SerializeField] private Animator slowdownAnimator;
    [SerializeField] private TrailRenderer[] slowdownTrails;

    [SerializeField] private PowerUpBar powerUpBar;

    private void Awake()
    {
        currentPowerup = PowerUpData.Instance.currentPowerUp;
        speedLines.Stop();
        nitroParticle.Stop();
        shieldGO.SetActive(false);
        foreach (TrailRenderer trail in slowdownTrails)
            trail.emitting = false;
        slowdownGO.SetActive(false);

        if (PowerUpData.Instance.currentPowerUp is Shield)
            isShield = true;
    }

    public void Activate()
    {
        if (isShield && powerUpBar.currentTime <= powerUpBar.powerUpCountdown / 2)
            return;

        isActive = true;
        currentPowerup.ActivatePowerUp(gameObject);
        StopAllCoroutines();
        StartCoroutine(EmptyBar?.Invoke());

        if (currentPowerup is Nitro)
        {
            speedLines.Emit(1);
            speedLines.Play();
            nitroParticle.Emit(1);
            nitroParticle.Play();
        }
        else if(currentPowerup is Shield)
        {
            shieldGO.SetActive(true);
            shieldAnimator.SetTrigger("Activate");
        }
        else
        {
            slowdownGO.SetActive(true);
            slowdownAnimator.SetTrigger("Activate");
            foreach (TrailRenderer trail in slowdownTrails)
                trail.emitting = true;
            SoundManager.instance.SlowSounds();
        }
    }

    public void Deactivate()
    {
        isActive = false;
        currentPowerup.DeactivatePowerUp(gameObject);
        StopAllCoroutines();
        StartCoroutine(FillBar?.Invoke());

        if (currentPowerup is Nitro)
        {
            speedLines.Stop();
            nitroParticle.Stop();
        }
        else if(currentPowerup is Shield)
        {
            shieldAnimator.SetTrigger("Deactivate");
        }
        else
        {
            slowdownAnimator.SetTrigger("Deactivate");
            foreach (TrailRenderer trail in slowdownTrails)
                trail.emitting = false;
            SoundManager.instance.NormalSounds();
        }
    }
}
