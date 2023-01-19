using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNPCInteract : MonoBehaviour
{
    public Button learnSkillBtn;
    public Button converseationBtn;
    
    public void Init()
    {
        learnSkillBtn = transform.Find("LearnSkill").GetComponent<Button>();
        converseationBtn = transform.Find("Converseation").GetComponent<Button>();
    }
}
