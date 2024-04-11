using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    string _pathToJson;

    [SerializeField] public GameLevels Game;

    private void Awake() {
        //Singleton
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; DontDestroyOnLoad(this); }

        //Path to json's game progress
        _pathToJson = Application.persistentDataPath + "/JSON/GameProgress.json";
    }
}

//Infos of levels to save and load progress
[Serializable]
public class Level{
    public string LevelName;
    public bool IsDone;
    public int Score;
    public string SceneReference;
}

//List of levels to transform to json
[Serializable]
public class GameLevels {
    [SerializeField] public List<Level> Levels;

    public GameLevels() { }
    public GameLevels(List<Level> levels) { Levels = levels; }

    public void AddLevel(Level level) {  Levels.Add(level); }
}
