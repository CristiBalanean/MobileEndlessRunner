using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private GameObject sparkEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Instantiate(sparkEffect, transform.position, Quaternion.identity);
            MoneyManager.Instance.currentMoney += 1;
            SoundManager.instance.Play("Coin");
            Destroy(gameObject);
        }
    }
}
