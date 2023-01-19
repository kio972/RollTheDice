using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public bool start = false;
    public string sceneName;

    public float playerCurHp;
    public float playerMaxHp;
    public int[] playerItems;
    public int[] playerRelics;
    public int[] playerSkills;
    public int[] quickSlotSkills;
    public int playerGold;
    public int playerSkillPoint;
    public int playerPermanentPoint;

    public int week;
    public int[] clearedQuest;

    public int tapDanceCount;
    public int speedGamer;
    public int monsterKillCount;
    public int clearNormalCount;
    public int clearEliteCount;
    public int clearBossCount;
    public int untouchableCount;
}
