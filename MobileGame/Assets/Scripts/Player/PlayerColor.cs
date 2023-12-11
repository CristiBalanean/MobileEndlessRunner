using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private Material colorSwapMaterial;
    [SerializeField] private Color[] colors;

    void Start()
    {
        if (PlayerPrefs.HasKey("Color"))
            colorSwapMaterial.SetColor("_BaseColor", colors[PlayerPrefs.GetInt("Color") - 1]);
        else
            colorSwapMaterial.SetColor("_BaseColor", colors[0]);
    }
}
