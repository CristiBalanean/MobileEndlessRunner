using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : ScriptableObject
{
    public abstract void ActivatePowerUp(GameObject target);

    public abstract void DeactivatePowerUp(GameObject target);
}

[System.Serializable]
public class Powerups
{
    public PowerUp powerup;
    public Sprite powerupImage;
    public string powerUpName;
    public string powerUpDescription;
    public int powerupPrice;
    public bool unlocked = false;
}

[System.Serializable]
public class PowerupUnlockedData
{
    public string powerupName;
    public bool unlocked;

    public PowerupUnlockedData()
    {

    }
}

[System.Serializable]
public class PowerupUnlockedDataWrapper
{
    public PowerupUnlockedData[] powerupDataList;

    public PowerupUnlockedDataWrapper(PowerupUnlockedData[] powerupDataList)
    {
        this.powerupDataList = powerupDataList;
    }
}

