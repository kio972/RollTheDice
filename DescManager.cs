using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescManager : MonoBehaviour
{
    public static DescManager instance;

    private Transform skillDesc;
    private Image skillIcon;
    private TextMeshProUGUI skillName;
    private TextMeshProUGUI skillDescription;

    private Transform stackDice;
    private Animator diceAnimator;

    private Transform monsterDesc;
    private TextMeshProUGUI monsterName;
    private TextMeshProUGUI monsterDescription;

    public void DiceOn()
    {
        stackDice.gameObject.SetActive(true);
        
    }

    public void DescOff()
    {
        skillDesc.gameObject.SetActive(false);
        monsterDesc.gameObject.SetActive(false);
        stackDice.gameObject.SetActive(false);
    }

    public void DrawEnemy(EnemyController enemy)
    {
        skillDesc.gameObject.SetActive(false);
        monsterDesc.gameObject.SetActive(true);
        monsterName.text = enemy.enemyName;
        monsterDescription.text = "체력 : " + enemy.curHp.ToString() + " / " + enemy.maxHp.ToString()
            + "\n\n" + enemy.enemyDesc;
    }

    public void DrawSkill(SkillInfo info)
    {
        monsterDesc.gameObject.SetActive(false);
        skillDesc.gameObject.SetActive(true);
        //스킬아이콘 업데이트 추가예정
        skillName.text = info.skillName;
        skillDescription.text = info.skillDesc;
    }

    public void UpdateDesc()
    {
        if (GameManager.instance.skillManager.currSkill != null)
        {
            DrawSkill(GameManager.instance.skillManager.currSkill.skillInfo);
        }
    }

    public void Init()
    {
        instance = this;

        Transform[] temp = GetComponentsInChildren<Transform>(true);
        foreach(Transform t in temp)
            t.gameObject.SetActive(true);

        skillDesc = transform.Find("SkillDesc");
        if(skillDesc != null)
        {
            //skillIcon = skillDesc.transform.Find("Icon").Find("IconImg").GetComponent<Image>();
            skillName = skillDesc.transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
            skillDescription = skillDesc.transform.Find("SkillDescription").GetComponent<TextMeshProUGUI>();
            skillDesc.gameObject.SetActive(false);
        }

        monsterDesc = transform.Find("MonsterDesc");
        if(monsterDesc != null)
        {
            monsterName = UtillHelper.GetComponent<TextMeshProUGUI>("MonsterName", monsterDesc.transform);
            monsterDescription = UtillHelper.GetComponent<TextMeshProUGUI>("MonsterDescription", monsterDesc.transform);
            monsterDesc.gameObject.SetActive(false);
        }

        stackDice = transform.Find("StackDice");
        if (stackDice != null)
        {
            diceAnimator = GameObject.Find("DiceZone").transform.Find("StackDice").GetComponentInChildren<Animator>();
            stackDice.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
