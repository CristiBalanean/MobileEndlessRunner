using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : PowerUp
{
    [SerializeField] private float topSpeed;

    protected override IEnumerator PickUp(Collider2D player)
    {
        /*GetComponent<SpriteRenderer>().enabled = false;

        CarMovement playerSpeed = player.GetComponent<CarMovement>();
        player.isTrigger = true;
        float currentSpeed = playerSpeed.GetTopSpeed();
        float currentAcceleration = playerSpeed.GetAcceleration();
        playerSpeed.SetTopSpeed(topSpeed);
        playerSpeed.SetAcceleration(currentAcceleration + 15);

        yield return new WaitForSeconds(duration);

        playerSpeed.SetTopSpeed(currentSpeed);
        playerSpeed.SetAcceleration(currentAcceleration);

        yield return new WaitForSeconds(1);
        player.isTrigger = false;

        GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(false);*/

        yield return null;
    }
}
