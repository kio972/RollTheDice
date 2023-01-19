using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC0 : NPCs
{
    private SkillNPCInteract interact;  

    private void LearnSkill()
    {
        if(GuildController.instance != null)
        {
            GuildController.instance.skillLearn.SetSkillLearnDesc(SkillType.Common);
            GuildController.instance.skillLearn.gameObject.SetActive(true);
            interact.gameObject.SetActive(false);
        }
    }

    private void Converseation()
    {
        if (TownController.instance != null)
        {
            TownController.instance.npc.SetNPCName(npcType, npcName);
            TownController.instance.npc.SetConv("");
        }
    }

    public override void Interact()
    {
        if(interact != null)
        {
            interact.learnSkillBtn.onClick.RemoveAllListeners();
            interact.learnSkillBtn.onClick.AddListener(LearnSkill);

            interact.converseationBtn.onClick.RemoveAllListeners();
            interact.converseationBtn.onClick.AddListener(Converseation);

            interact.gameObject.SetActive(true);
        }
    }

    public override void Init()
    {
        npcName = "±ÙÀ°ÁúÀÇ º£³ë";
        npcType = "ÈÆ·Ã±³°ü";
        interact = FindObjectOfType<SkillNPCInteract>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
