using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SavingScript : MonoBehaviour
{
    private CharacterControllerNonUnity player;

    public static SavingScript instance;

    public bool hasLoaded;

    int numberOfSaves;

    private void Awake()
    {
        player = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
        instance = this;
        Load();
        System.DateTime dateToDisplay = System.DateTime.Now;
    }
    public SaveData activeSave;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Load();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            DeleteSaveData();
        }

    }
    public void Save()
    {
            string dataPath = Application.persistentDataPath;

            if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
            {
                File.Delete(dataPath + "/" + activeSave.saveName + ".save");
                numberOfSaves--;
                Debug.Log("Deleted!");
            }
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
            serializer.Serialize(stream, activeSave);
            stream.Close();
            Debug.Log("Saved!");
        
    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();
            hasLoaded = true;
            Debug.Log("Loaded!");
        }
    }
    public void DeleteSaveData()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName  + ".save"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".save");
            Debug.Log("Deleted!");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string saveName;

    public int CurrentLevel;

    public float saveLife;

    public int saveNumberOfPotions;

    public Vector3 CharacterPosition;

    public Quaternion TowerPosition;
}
