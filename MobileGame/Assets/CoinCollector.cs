using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MoneyManager.Instance.currentMoney += 1;
            SoundManager.instance.Play("Coin");
            Destroy(gameObject);
        }
    }
}
