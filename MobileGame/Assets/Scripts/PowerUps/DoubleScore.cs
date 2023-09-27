using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScore : PowerUp
{
    protected override IEnumerator PickUp(Collider2D player)
    {
        GetComponent<SpriteRenderer>().enabled = false;

        ScoreManager.Instance.doubleMultiplier = true;
        yield return new WaitForSeconds(duration);
        ScoreManager.Instance.doubleMultiplier = false;

        GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(false);
    }
}
