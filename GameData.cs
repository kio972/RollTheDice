using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Score
{
    
}

public class GameData
{
    // 현재 맵 데이터
    public static string sceneName;

    // 플레이어 데이터
    public static float playerCurHp = 60;
    public static float playerMaxHp = 60;

    public static int playerGold = 0;
    public static int playerSkillPoint = 0;
    public static int playerPermanentPoint = 0;

    public static List<int> playerItems;
    public static List<int> playerRelics;
    public static List<int> playerSkills;
    public static List<int> quickSlotSkills;

    public static int week = 0;
    public static List<int> clearedQuest;

    public static int tapDanceCount;
    public static int speedGamer;
    public static int monsterKillCount;
    public static int clearNormalCount;
    public static int clearEliteCount;
    public static int clearBossCount;
    public static int untouchableCount;

    public static void SaveData(PlayerController player)
    {
        playerCurHp = player.curHp;
        playerMaxHp = player.maxHp;

        playerGold = player.gold;
        playerSkillPoint = player.skillPoint;
        playerPermanentPoint = player.permanentSkillPoint;

        if (player.playerStat != null)
            player.playerStat.relic.SaveRelic();

        if (player.bag != null)
            player.bag.SaveItem();
    }

}
