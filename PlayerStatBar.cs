using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatBar : MonoBehaviour
{
    private TextMeshProUGUI gold;
    private TextMeshProUGUI permanentPoint;
    private TextMeshProUGUI skillPoint;

    public void UpdateStat()
    {
        gold.text = TownController.instance.player.gold.ToString();
        permanentPoint.text = TownController.instance.player.permanentSkillPoint.ToString();
        skillPoint.text = TownController.instance.player.skillPoint.ToString();

    }   
    
    public void Init()
    {
        gold = transform.Find("Gold").GetComponentInChildren<TextMeshProUGUI>();
        permanentPoint = transform.Find("SkillPoint").Find("Permanent").GetComponent<TextMeshProUGUI>();
        skillPoint = transform.Find("SkillPoint").Find("Skill").GetComponent<TextMeshProUGUI>();

        UpdateStat();
    }
}
