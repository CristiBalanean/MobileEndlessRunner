using System.Collections;
using UnityEngine;

public class SlowDown : PowerUp
{
    protected override IEnumerator PickUp(Collider2D player)
    {
        GetComponent<SpriteRenderer>().enabled = false;

        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;

        GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(false);
    }
}
