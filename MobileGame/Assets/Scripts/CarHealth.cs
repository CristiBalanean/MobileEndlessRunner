using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHealth : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle") || collision.transform.CompareTag("Police"))
        {
            if(ScoreManager.Instance.GetFinalScore() > PlayerPrefs.GetInt("HighScore"))
                PlayerPrefs.SetInt("HighScore", ScoreManager.Instance.GetFinalScore());

            GetComponent<CarMovement>().SetSpeedAtDeath();
            gameObject.SetActive(false);
            GameManager.Instance.ActivateRestartScreen();
        }
    }
}
