using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    string _pathToJson;

    [SerializeField] private GameLevels _game;

    private void Awake() {
        //Singleton
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; DontDestroyOnLoad(this); }

        //Path to json's game progress
        _pathToJson = Application.persistentDataPath + "/JSON/GameProgress.json";

        //If don't have a json, create it, else, load game progress
        if(!File.Exists(_pathToJson)) { 
            Directory.CreateDirectory(Application.persistentDataPath + "/JSON"); 
            File.Create(_pathToJson);
        }
        else { LoadProgress(); }
    }

    #region SAVE AND LOAD PROGRESS
    public void SaveProgress() {
        string json = JsonUtility.ToJson(_game);
        File.WriteAllText(_pathToJson, json);
    }

    public void LoadProgress() {
        _game = JsonUtility.FromJson<GameLevels>(_pathToJson);
    }

    #endregion
}

//Infos of levels to save and load progress
[Serializable]
public class Level{
    public string LevelName;
    public bool IsDone;
    public int Score;
}

//List of levels to transform to json
[Serializable]
public class GameLevels {
    [SerializeField] private List<Level> Levels;

    public GameLevels() { }
    public GameLevels(List<Level> levels) { Levels = levels; }

    public void AddLevel(Level level) {  Levels.Add(level); }
}
