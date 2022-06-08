using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public float life;
    public int potionsNumber;
    public int coins;
    public string currentScene;
    public int currentFace;
    public float currHPSLow;
    public Quaternion towerRotation;

    public GameData()
    {
        this.life = 100f;
        this.potionsNumber = 3;
        this.coins = 0;
        this.currentScene = "Tutorial";
        this.currentFace = 1;
        this.towerRotation = new Quaternion(0, 0, 0, 1);
        this.currHPSLow = 100;

    }
}
