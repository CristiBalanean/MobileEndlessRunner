using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTruckAnimation : MonoBehaviour
{
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        float speedRatio = CarMovement.Instance.GetSpeed() / CarMovement.Instance.GetTopSpeed();
        playerAnimator.speed = Mathf.Lerp(0.1f, 1, speedRatio);
    }
}
