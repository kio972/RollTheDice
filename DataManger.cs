using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManger : MonoBehaviour
{
    public TextAsset skillCSV;

    public TextAsset itemCSV;

    public TextAsset relicCSV;

    public TextAsset questCSV;

    private static List<Dictionary<string, string>> skillData;
    private static List<Dictionary<string, string>> itemData;
    private static List<Dictionary<string, string>> relicData;
    private static List<Dictionary<string, string>> questData;
    
    private static List<int> item_Common;
    private static List<int> item_Uncommon;
    private static List<int> item_Rare;
    private static List<int> item_Epic;


    public static List<Dictionary<string, string>> SkillData
    {
        get { return skillData; }
    }

    public static List<Dictionary<string, string>> ItemData
    {
        get { return itemData; }
    }

    public static List<Dictionary<string, string>> RelicData
    {
        get { return relicData; }
    }

    public static List<Dictionary<string, string>> QuestData
    {
        get { return questData; }
    }

    public static List<int> Item_Common
    {
        get { return item_Common; }
    }

    public static List<int> Item_Uncommon
    {
        get { return item_Uncommon; }
    }

    public static List<int> Item_Rare
    {
        get { return item_Rare; }
    }

    public static List<int> Item_Epic
    {
        get { return item_Epic; }
    }

    private List<Dictionary<string, string>> LoadCSV(string str)
    {
        string[] lines = str.Split('\n');

        string[] heads = lines[0].Split(',');


        heads[heads.Length - 1] = heads[heads.Length - 1].Replace("\r", "");
        // List<줄수> dic<내용,헤더>
        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
        
        for(int i = 1; i < lines.Length; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] col = lines[i].Split(',');
            col[col.Length - 1] = col[col.Length - 1].Replace("\r", "");
            for (int j = 0; j < heads.Length; j++)
            {
                string value = col[j];

                dic.Add(heads[j], col[j]);
            }

            list.Add(dic);
        }

        return list;
    }

    private void Awake()
    {
        DataManger[] dataMangers = FindObjectsOfType<DataManger>();
        if(dataMangers.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Init();
        }
    }

    // Start is called before the first frame update
    private void Init()
    {
        if (skillCSV != null)
            skillData = LoadCSV(skillCSV.text);

        if (itemCSV != null)
        {
            itemData = LoadCSV(itemCSV.text);
            item_Common = new List<int>();
            item_Uncommon = new List<int>();
            item_Rare = new List<int>();
            item_Epic = new List<int>();
            for (int i = 0; i < itemData.Count; i++)
            {
                int value = int.Parse(itemData[i]["Value"]);
                switch (value)
                {
                    case 0: item_Common.Add(i); break;
                    case 1: item_Uncommon.Add(i); break;
                    case 2: item_Rare.Add(i); break;
                    case 3: item_Epic.Add(i); break;
                }
            }
        }

        if(relicCSV != null)
        {
            relicData = LoadCSV(relicCSV.text);
        }

        if(questCSV != null)
        {
            questData = LoadCSV(questCSV.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
