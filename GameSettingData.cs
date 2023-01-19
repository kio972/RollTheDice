using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameSettingData
{
    public float totalVolume = 1;
    public float bgVolume = 1;
    public float fxVolume = 1;
    public int[] startRelics = new int[2] { 0 , 1 };
    public float gameSpeed = 1;
}
