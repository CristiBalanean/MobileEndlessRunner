using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePool : MonoBehaviour
{
    public static SpritePool Instance;

    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite ChooseSprite()
    {
        int rand = Random.Range(0, sprites.Length);
        return sprites[rand];
    }
}
