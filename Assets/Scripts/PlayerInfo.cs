using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

    //private async void Start() {

    //    if (!Directory.Exists(Application.persistentDataPath + "/JSON")) {
    //        Directory.CreateDirectory(Application.persistentDataPath + "/JSON");
    //    }

    //    if (!File.Exists(Application.persistentDataPath + "/JSON/gameinfo.json")) {
    //        File.Create(Application.persistentDataPath + "/JSON/gameinfo.json");
    //    }
    //    else {
    //        GameInfo = JsonUtility.FromJson<GameInfo>(Application.persistentDataPath + "/JSON/GameInfo.json");
    //    }

    //    await Task.Delay(3000);

    //    SaveGameData(0, 6, true);

    //}

    public void SaveGameData(int indexGame, int score, bool complete) {

        if(complete) { GameInfo.progress[indexGame].complete = true; }

        GameInfo.progress[indexGame].highScore.Add(score);

        for(int i = 0; i < GameInfo.progress[indexGame].highScore.Count; i++) {
            for(int j = 0; j < GameInfo.progress[indexGame].highScore.Count; j++) {
                if (GameInfo.progress[indexGame].highScore[i] > GameInfo.progress[indexGame].highScore[j]) {
                    int prev = GameInfo.progress[indexGame].highScore[i];
                    int now = GameInfo.progress[indexGame].highScore[j];

                    GameInfo.progress[indexGame].highScore[i] = now;
                    GameInfo.progress[indexGame].highScore[j] = prev;
                }
            }
        }

        if(GameInfo.progress[indexGame].highScore.Count > 5) {
            GameInfo.progress[indexGame].highScore.RemoveAt(5);
        }

        var json = JsonUtility.ToJson(GameInfo);

        File.WriteAllText(Application.persistentDataPath + "/JSON/gameinfo.json", json);
    }
}

[Serializable]
public class GameInfo {
    public string username;
    public List<ProgressInfo> progress = new List<ProgressInfo>();
}

[Serializable]
public class ProgressInfo {
    public int indexGame;
    public bool complete;
    public List<int> highScore = new List<int>();
}
