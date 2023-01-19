using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static GameObject save;
    static GameObject Save
    {
        get { return save; }
    }

    static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if(!instance)
            {
                save = new GameObject();
                save.name = "SaveManager";
                instance = save.AddComponent<SaveManager>();
                DontDestroyOnLoad(save);
            }
            return instance;
        }
    }


    public string settingFileName = "SettingData.json";
    public GameSettingData settingData;

    public string saveFileName = "SaveData.json";
    public SaveFile gameData;

    public void LoadSettingData()
    {
        string saveFilePath = Application.persistentDataPath + settingFileName;
        print(saveFilePath);

        if (File.Exists(saveFilePath))
        {
            string loadData = File.ReadAllText(saveFilePath);
            settingData = JsonUtility.FromJson<GameSettingData>(loadData);
        }
        else
        {
            settingData = new GameSettingData();
        }
    }

    public void SaveSettingData()
    {
        string ToJsonData = JsonUtility.ToJson(settingData);
        string saveFilePath = Application.persistentDataPath + settingFileName;

        File.WriteAllText(saveFilePath, ToJsonData);
    }

    public void LoadFile()
    {
        GameData.sceneName = gameData.sceneName;
        GameData.playerCurHp = gameData.playerCurHp;
        GameData.playerMaxHp = gameData.playerMaxHp;
        GameData.playerGold = gameData.playerGold;
        GameData.playerSkillPoint = gameData.playerSkillPoint;
        GameData.playerPermanentPoint = gameData.playerPermanentPoint;
        GameData.week = gameData.week;

        GameData.clearedQuest = new List<int>();
        if(gameData.clearedQuest != null)
        {
            for (int i = 0; i < gameData.clearedQuest.Length; i++)
            {
                GameData.clearedQuest.Add(gameData.clearedQuest[i]);
            }
        }

        GameData.playerItems = new List<int>();
        if (gameData.playerItems != null)
        {
            for (int i = 0; i < gameData.playerItems.Length; i++)
            {
                GameData.playerItems.Add(gameData.playerItems[i]);
            }
        }

        GameData.playerRelics = new List<int>();
        if (gameData.playerRelics != null)
        {
            for (int i = 0; i < gameData.playerRelics.Length; i++)
            {
                GameData.playerRelics.Add(gameData.playerRelics[i]);
            }
        }

        GameData.playerSkills = new List<int>();
        if (gameData.playerSkills != null)
        {
            for (int i = 0; i < gameData.playerSkills.Length; i++)
            {
                GameData.playerSkills.Add(gameData.playerSkills[i]);
            }
        }

        GameData.quickSlotSkills = new List<int>();
        if (gameData.quickSlotSkills != null)
        {
            for (int i = 0; i < gameData.quickSlotSkills.Length; i++)
            {
                GameData.quickSlotSkills.Add(gameData.quickSlotSkills[i]);
            }
        }

        GameData.tapDanceCount = gameData.tapDanceCount;
        GameData.speedGamer = gameData.speedGamer;
        GameData.monsterKillCount = gameData.monsterKillCount;
        GameData.clearNormalCount = gameData.clearNormalCount;
        GameData.clearEliteCount = gameData.clearEliteCount;
        GameData.clearBossCount = gameData.clearBossCount;
        GameData.untouchableCount = gameData.untouchableCount;
    }

    public void RemoveGameData()
    {
        File.Delete(Application.persistentDataPath + saveFileName);
    }

    public void LoadGameData()
    {
        string saveFilePath = Application.persistentDataPath + saveFileName;

        if(File.Exists(saveFilePath))
        {
            string loadData = File.ReadAllText(saveFilePath);
            gameData = JsonUtility.FromJson<SaveFile>(loadData);
        }
        else
        {
            gameData = new SaveFile();
        }

        LoadFile();
    }

    private void SaveFile()
    {
        if(gameData == null)
        {
            LoadGameData();
        }

        gameData.sceneName = GameData.sceneName;
        gameData.playerCurHp = GameData.playerCurHp;
        gameData.playerMaxHp = GameData.playerMaxHp;
        gameData.playerGold = GameData.playerGold;
        gameData.playerSkillPoint = GameData.playerSkillPoint;
        gameData.playerPermanentPoint = GameData.playerPermanentPoint;

        gameData.week = GameData.week;

        if(GameData.clearedQuest != null)
        {
            gameData.clearedQuest = new int[GameData.clearedQuest.Count];
            for (int i = 0; i < GameData.clearedQuest.Count; i++)
            {
                gameData.clearedQuest[i] = GameData.clearedQuest[i];
            }
        }

        if(GameData.playerItems != null)
        {
            gameData.playerItems = new int[GameData.playerItems.Count];
            for (int i = 0; i < GameData.playerItems.Count; i++)
            {
                gameData.playerItems[i] = GameData.playerItems[i];
            }
        }
        
        if(GameData.playerRelics != null)
        {
            gameData.playerRelics = new int[GameData.playerRelics.Count];
            for (int i = 0; i < GameData.playerRelics.Count; i++)
            {
                gameData.playerRelics[i] = GameData.playerRelics[i];
            }
        }
        
        if(GameData.playerSkills != null)
        {
            gameData.playerSkills = new int[GameData.playerSkills.Count];
            for (int i = 0; i < GameData.playerSkills.Count; i++)
            {
                gameData.playerSkills[i] = GameData.playerSkills[i];
            }
        }
        
        if(GameData.quickSlotSkills != null)
        {
            gameData.quickSlotSkills = new int[GameData.quickSlotSkills.Count];
            for (int i = 0; i < GameData.quickSlotSkills.Count; i++)
            {
                gameData.quickSlotSkills[i] = GameData.quickSlotSkills[i];
            }
        }

        gameData.tapDanceCount = GameData.tapDanceCount;
        gameData.speedGamer = GameData.speedGamer;
        gameData.monsterKillCount = GameData.monsterKillCount;
        gameData.clearNormalCount = GameData.clearNormalCount;
        gameData.clearEliteCount = GameData.clearEliteCount;
        gameData.clearBossCount = GameData.clearBossCount;
        gameData.untouchableCount = GameData.untouchableCount;
    }

    public void SaveGameData()
    {
        SaveFile();
        string ToJsonData = JsonUtility.ToJson(gameData);
        string saveFilePath = Application.persistentDataPath + saveFileName;

        File.WriteAllText(saveFilePath, ToJsonData);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void Awake()
    {
        LoadSettingData();
        SaveSettingData();

        LoadGameData();
        SaveGameData();
    }
}
