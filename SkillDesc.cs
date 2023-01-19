using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDesc : MonoBehaviour
{
    private TextMeshProUGUI skillName;
    private TextMeshProUGUI skillDesc;
    private TextMeshProUGUI skillCost;
    private Image skillRange;
    private TextMeshProUGUI needSkills;
    private Transform needPermanentPoint;
    private TextMeshProUGUI needPermentText;
    private Transform needSkillPoint;
    private TextMeshProUGUI needSkillPointText;
    private Transform needGold;
    private TextMeshProUGUI needGoldText;

    public void SetSkillInfo(SkillInfo skillInfo)
    {
        if(skillName != null)
            skillName.text = skillInfo.skillName;
        if(skillDesc != null)
            skillDesc.text = skillInfo.skillDesc;
        if(skillCost != null)
            skillCost.text = "cost : " + skillInfo.cost.ToString();
        if(skillRange != null)
        {
            if (skillInfo.rangeImg != null)
                skillRange.sprite = skillInfo.rangeImg;
            else
                skillRange.sprite = null;
        }
        if(needSkills != null)
        {
            if (skillInfo.needSkill != null)
            {
                string skills = "";
                for (int i = 0; i < skillInfo.needSkill.Length; i++)
                {
                    if (i > 0)
                        skills = skills + ", ";
                    // i스킬명 받아오는 함수 작성필요
                    skills = skills + DataManger.SkillData[skillInfo.needSkill[i]]["Name"];
                }
                needSkills.text = skills;
            }
            else
                needSkills.text = "없음";
        }
        
        if(needPermanentPoint != null)
        {
            if (skillInfo.needPermanentPoint != 0)
            {
                needPermanentPoint.gameObject.SetActive(true);
                needPermentText.text = skillInfo.needPermanentPoint.ToString();

            }
            else
                needPermanentPoint.gameObject.SetActive(false);
        }
        
        if(needSkillPoint != null)
        {
            if (skillInfo.needSkillPoint != 0)
            {
                needSkillPoint.gameObject.SetActive(true);
                needSkillPointText.text = skillInfo.needSkillPoint.ToString();

            }
            else
                needSkillPoint.gameObject.SetActive(false);
        }
        
        if(needGold != null)
        {
            if (skillInfo.needGold != 0)
            {
                needGold.gameObject.SetActive(true);
                needGoldText.text = skillInfo.needGold.ToString();

            }
            else
                needGold.gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        skillName = UtillHelper.GetComponent<TextMeshProUGUI>("SkillName", transform);
        skillDesc = UtillHelper.GetComponent<TextMeshProUGUI>("SkillDesc", transform);
        skillCost = UtillHelper.GetComponent<TextMeshProUGUI>("SkillCost", transform);
        skillRange = UtillHelper.GetComponent<Image>("SkillRange", transform);
        needSkills = UtillHelper.GetComponent<TextMeshProUGUI>("NeedSkills", transform);
        Transform needPoint = transform.Find("NeedPoints");
        if(needPoint != null)
        {
            needPermanentPoint = needPoint.transform.Find("NeedPermanent");
            needPermentText = needPermanentPoint.GetComponentInChildren<TextMeshProUGUI>();
            needSkillPoint = needPoint.transform.Find("NeedSkillPoint");
            needSkillPointText = needSkillPoint.GetComponentInChildren<TextMeshProUGUI>();
            needGold = needPoint.transform.Find("NeedGold");
            needGoldText = needGold.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
