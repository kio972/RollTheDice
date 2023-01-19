using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType
{
    Basic,
    Range,
    Target,
    Buff,
}

public struct SkillInfo
{
    public int index;
    public float cost;
    public int skillLv;
    public AttackType type;
    public int targetRange;
    public int range;

    public List<int[]> targetPos;
    public List<int[]> rangePos;

    public float damage;
    public int coolTime;

    public Sprite rangeImg;
    public int[] needSkill;

    public string skillName;
    public string skillDesc;
    public int needPermanentPoint;
    public int needSkillPoint;
    public int needGold;
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public int maxSkill = 6;
    public List<SkillBtn> skillBtn;

    public SkillFunc currSkill;
    public Coroutine effect;

    public bool isAttacking = false;

    public SkillDice skillDice;

    public bool attackInput = false;

    public int attackTargetRow;
    public int attackTargetCol;

    public EnemyController targetEnemy;

    public void CoolDown()
    {
        foreach(SkillBtn btn in skillBtn)
        {
            btn.coolTime--;
            if(btn.coolTime <= 0)
            {
                btn.CoolDown = false;
            }
        }
    }

    private void SetButton()
    {
        if(GameData.quickSlotSkills != null)
        {
            for (int i = 0; i < GameData.quickSlotSkills.Count; i++)
            {
                if(GameData.quickSlotSkills[i] != -1)
                {
                    skillBtn[i].gameObject.SetActive(true);
                    SkillFunc skill = Resources.Load<SkillFunc>(DataManger.SkillData[GameData.quickSlotSkills[i]]["Path"]);
                    if (skill != null)
                    {
                        skill = Instantiate(skill, skillBtn[i].transform);
                        skillBtn[i].Init();
                    }
                }
            }
        }
    }

    public void UpdateSkill()
    {
        SetButton();
        skillDice.SetDice();
    }

    public void Init()
    {
        instance = this;
        skillBtn = new List<SkillBtn>();
        for (int i = 0; i < maxSkill; i++)
        {
            SkillBtn btn = transform.Find("SkillBtn" + i.ToString()).GetComponent<SkillBtn>();
            if (btn != null)
            {
                skillBtn.Add(btn);
            }
        }

        foreach (SkillBtn btn in skillBtn)
        {
            btn.gameObject.SetActive(false);
        }

        SetButton();

        skillDice = FindObjectOfType<SkillDice>();
        if(skillDice != null)
            skillDice.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
