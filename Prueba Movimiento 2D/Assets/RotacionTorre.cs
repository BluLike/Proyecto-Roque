using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RotacionTorre : MonoBehaviour, IDataPersistence
{
    [SerializeField] Quaternion rotation;
    [SerializeField] int caraTorre;
    public void LoadData(GameData data)
    {
        caraTorre = data.currentFace;
    }
    public void SaveData(GameData data)
    {
        data.towerRotation = this.transform.localRotation;
    }
    private void Start()
    {
        if (caraTorre == 1)
        {
            transform.localRotation = new Quaternion(0,0,0,0);
        }
        else if (caraTorre == 2)
        {
            transform.localRotation = new Quaternion(0, 90, 0, 90);
        }
        else if (caraTorre == 3)
        {
            transform.localRotation = new Quaternion(0, 180, 0, 180);
        }
        else if (caraTorre == 4)
        {
            transform.localRotation = new Quaternion(0, 360, 0, 360);
        }
    }

    void OnDestroy()
    {
        caraTorre = 1;
    }
}
