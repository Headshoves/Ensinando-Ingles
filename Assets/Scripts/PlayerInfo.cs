using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;

    public GameInfo GameInfo;

    private void Awake() {

        if(FindObjectsOfType<PlayerInfo>().Length > 1) {
            DestroyImmediate(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {

        GameInfo.username = "bpasofkpafias pfaso";

        if(!Directory.Exists(Application.persistentDataPath + "/JSON")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/JSON");
        }

        if(!File.Exists(Application.persistentDataPath + "/JSON/gameinfo.json")) {
            File.Create(Application.persistentDataPath + "/JSON/gameinfo.json");
            File.WriteAllText(Application.persistentDataPath + "/JSON/gameinfo.json", "Hello");
            //var json = JsonUtility.ToJson(GameInfo);
            //File.WriteAllText(Application.persistentDataPath + "/JSON/gameinfo.json", json);
        }
        //else {
        //    GameInfo = JsonUtility.FromJson<GameInfo>(Application.persistentDataPath + "/JSON/GameInfo.json");
        //}
        
    }
}

[Serializable]
public class GameInfo {
    public string username;
    //public List<ProgressInfo> progress = new List<ProgressInfo>();
}

[Serializable]
public class ProgressInfo {
    public int indexGame;
    public bool complete;
    public List<int> highScore = new List<int>();
}
