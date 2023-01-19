using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRelic : MonoBehaviour
{
    // 유물 index에 따라 유물추가기능, 제거기능
    RelicFunc[] relics;

    public void RemoveRelic(int index)
    {
        relics = GetComponentsInChildren<RelicFunc>();
        foreach (RelicFunc relic in relics)
        {
            if(relic.relicInfo.index == index)
            {
                if(GameData.playerRelics != null)
                    GameData.playerRelics.Remove(index);
                Destroy(relic.gameObject);
                break;
            }
        }

        relics = GetComponentsInChildren<RelicFunc>();
    }

    public void GetRelic(int index, bool save = false)
    {
        RelicFunc relic = Resources.Load<RelicFunc>("Prefab/Relics/Relic"+index.ToString());
        if (relic != null)
        {
            relic = Instantiate(relic, transform);
            relic.Init();
            relic.haveItem = true;
        }
        else
        {
            print("잘못된 유물 인덱스넘버입니다.");
            return;
        }

        relics = GetComponentsInChildren<RelicFunc>();

        if (save)
        {
            SaveRelic();
        }
    }

    public void SaveRelic()
    {
        GameData.playerRelics = new List<int>();

        foreach (RelicFunc relic in relics)
        {
            GameData.playerRelics.Add(relic.relicInfo.index);
        }
    }

    private void LoadRelic()
    {
        relics = GetComponentsInChildren<RelicFunc>();
        foreach(RelicFunc relic in relics)
        {
            Destroy(relic.gameObject);
        }

        if (GameData.playerRelics != null)
        {
            List<int> list = new List<int>();
            foreach (int index in GameData.playerRelics)
            {
                list.Add(index);
            }

            foreach (int index in list)
            {
                GetRelic(index, false);
            }
        }
    }

    private void Start()
    {
        LoadRelic();
    }
}
