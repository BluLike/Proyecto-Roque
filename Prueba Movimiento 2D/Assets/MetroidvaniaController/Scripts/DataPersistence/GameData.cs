using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public float life;
    public int potionsNumber;
    public int coins;
    public int currentFace;

    public GameData()
    {
        this.life = 100f;
        this.potionsNumber = 3;
        this.coins = 0;
    }
}
