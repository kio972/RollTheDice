using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    TextMeshProUGUI inputText;
    List<string> lines;
    Transform bg;

    float elapsed = 0;

    bool isEnable = true;

    private void Toggle()
    {
        if(isEnable)
        {
            isEnable = false;
            bg.gameObject.SetActive(false);
        }
        else
        {
            isEnable = true;
            bg.gameObject.SetActive(true);
        }
    }

    private void BattleEnd(int isWin)
    {
        if (!(isWin == 1 || isWin == 0))
            return;

        if (GameManager.instance == null)
            return;

        if(isWin == 1)
            GameManager.instance.isWin = true;

        GameManager.instance.battleEnd = true;
    }

    private void AddStack(int stack)
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.player.stack += stack;
            GameManager.instance.uiManager.stackUpdater.UpdateStack();
        }
    }

    private void AddSkill(int index)
    {
        if(GameData.playerSkills == null)
            GameData.playerSkills = new List<int>();
        GameData.playerSkills.Add(index);

        if (GameData.quickSlotSkills == null)
        {
            GameData.quickSlotSkills = new List<int>();
            GameData.quickSlotSkills.Add(index);
        }
        else
        {
            for (int i = 0; i < GameData.quickSlotSkills.Count; i++)
            {
                if (GameData.quickSlotSkills[i] == -1)
                {
                    GameData.quickSlotSkills[i] = index;
                    break;
                }
                else if(i == GameData.quickSlotSkills.Count - 1)
                {
                    GameData.quickSlotSkills.Add(index);
                    break;
                }
            }
        }

        CommonSkillTree[] tree = FindObjectsOfType<CommonSkillTree>();
        foreach(CommonSkillTree skillTree in tree)
        {
            SkillFunc[] skills = skillTree.GetComponentsInChildren<SkillFunc>();
            foreach(SkillFunc skillFunc in skills)
            {
                if(skillFunc.skillInfo.index == index)
                {
                    skillFunc.isLeran = true;
                    Image image = skillFunc.GetComponentInParent<Image>();
                    if (image != null)
                        image.color = Color.white;
                }
            }
        }

        if(GameManager.instance != null)
        {
            GameManager.instance.skillManager.UpdateSkill();
        }
    }

    private void AddPermanentPoint(int point)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.permanentSkillPoint += point;
            player.statBar.UpdateStat();
        }
    }

    private void AddSkillPoint(int point)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.skillPoint += point;
            player.statBar.UpdateStat();
        }
    }

    private void AddGold(int gold)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if(player != null)
        {
            player.gold += gold;
            player.statBar.UpdateStat();
        }
    }

    private void RemoveRelic(int index)
    {
        if (PlayerStat.instance != null)
        {
            PlayerStat.instance.relic.RemoveRelic(index);
        }
    }

    private void AddRelic(int index)
    {
        if(PlayerStat.instance != null)
        {
            PlayerStat.instance.relic.GetRelic(index);
        }
    }

    private void Additem(int index)
    {
        ItemBag bag = FindObjectOfType<ItemBag>();
        if(bag != null)
        {
            bag.GetItem(index);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DebugConsole[] temp = FindObjectsOfType<DebugConsole>(true);
        if (temp.Length > 1)
            Destroy(this.gameObject, 0.1f);

        inputText = GetComponentInChildren<TextMeshProUGUI>();
        lines = new List<string>();
        bg = transform.Find("bg");
        bg.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            Toggle();
            return;
        }

        if (isEnable)
        {
            if (Input.anyKey)
            {
                string total = "";
                if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
                    return;

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    foreach (string line in lines)
                    {
                        total = total + line;
                    }

                    string[] comand = total.Split(' ');
                    int index = 0;
                    if (comand.Length > 1 && int.TryParse(comand[1], out index))
                        SendMessage(comand[0], index, SendMessageOptions.DontRequireReceiver);
                    lines = new List<string>();
                    inputText.text = "_";
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (lines.Count == 0)
                        return;

                    lines[lines.Count - 1] = "";
                    lines.Remove(lines[lines.Count - 1]);
                    foreach (string line in lines)
                    {
                        total = total + line;
                    }
                    inputText.text = total + "_";
                    elapsed = 0;
                    return;
                }

                if (Input.GetKey(KeyCode.Backspace))
                {
                    elapsed += Time.deltaTime;

                    if (elapsed < 0.1f)
                        return;
                    if (lines.Count == 0)
                        return;

                    lines[lines.Count - 1] = "";
                    lines.Remove(lines[lines.Count - 1]);
                    foreach (string line in lines)
                    {
                        total = total + line;
                    }
                    inputText.text = total + "_";
                    elapsed = 0;
                    return;
                }

                if (Input.GetKey(KeyCode.Backspace))
                    return;

                string str = Input.inputString;
                if (str != "")
                {
                    lines.Add(str);
                    foreach (string line in lines)
                    {
                        total = total + line;
                    }
                    inputText.text = total + "_";
                }
            }
        }
    }
}
