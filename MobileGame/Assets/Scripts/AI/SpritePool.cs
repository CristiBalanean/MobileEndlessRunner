using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePool : MonoBehaviour
{
    public static SpritePool Instance;

    [SerializeField] private CarGraphics[] carGraphics;

    private void Awake()
    {
        Instance = this;
    }

    public CarGraphics ChooseSprite()
    {
        int rand = Random.Range(0, carGraphics.Length);
        return carGraphics[rand];
    }
}

[System.Serializable]
public class CarGraphics
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject collider;

    public Sprite GetSprite() { return sprite; }

    public GameObject GetCollider() { return collider;}
}

