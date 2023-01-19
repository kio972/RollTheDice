using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildController : MonoBehaviour
{
    public static GuildController instance;
    public NPCs[] npc;
    public SkillLearn skillLearn;
    public SkillNPCInteract skillNPCOnClick;
    public PlayerSkillSlot quickSlot;
    public Transform guildUI;

    public void UpdateInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "NPC")
            {
                if(Input.GetKeyUp(KeyCode.Mouse0))
                {
                    NPCs curNPC = hit.transform.GetComponent<NPCs>();
                    if(curNPC != null)
                        curNPC.Interact();
                }
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //skillNPCOnClick.gameObject.SetActive(false);
            }
        }
    }

    public void Init()
    {
        instance = this;

        guildUI = TownController.instance.canvas.transform.Find("GuildUI");

        if(guildUI != null)
        {
            skillLearn = guildUI.GetComponentInChildren<SkillLearn>(true);
            if (skillLearn != null)
            {
                skillLearn.Init();
                skillLearn.gameObject.SetActive(false);
            }

            skillNPCOnClick = guildUI.GetComponentInChildren<SkillNPCInteract>(true);
            if (skillNPCOnClick != null)
            {
                skillNPCOnClick.Init();
                skillNPCOnClick.gameObject.SetActive(false);
            }

            quickSlot = guildUI.GetComponentInChildren<PlayerSkillSlot>(true);
            if (quickSlot != null)
                quickSlot.Init();
        }

        npc = GetComponentsInChildren<NPCs>(true);
        foreach (NPCs npcs in npc)
        {
            npcs.Init();
        }


    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }
}
