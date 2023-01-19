using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillBtn : MonoBehaviour
{
    private Button btn;
    private SkillFunc skill;
    private bool coolDown = false;
    public bool CoolDown { get { return coolDown; }  set { coolDown = value; } }
    public int coolTime = 0;

    private bool mouseOver = false;

    private Coroutine fail;

    private TextMeshProUGUI helper;

    private void HelpText()
    {
        if(helper != null)
        {
            helper.gameObject.SetActive(true);
            if (coolDown)
            {
                helper.color = Color.white;
                helper.text = "coolDown : " + coolTime.ToString();
            }
            else
            {
                helper.color = Color.yellow;
                helper.text = "cost : " + skill.skillInfo.cost.ToString();
            }
        }
    }

    private void OnPointerEnter(PointerEventData data)
    {
        if (DescManager.instance != null)
        {
            mouseOver = true;
            HelpText();

            AudioClip clip = Resources.Load<AudioClip>("Sounds/mouseOver");
            AudioManager.Instance.PlayEffect(clip);

            if (MousePointer.instance != null)
                MousePointer.instance.mouseType = MouseType.Interact;

            if (SkillManager.instance.currSkill == null || Input.GetKeyDown(KeyCode.Mouse0))
            {
                SkillFunc skill = GetComponentInChildren<SkillFunc>();
                if (skill != null)
                    DescManager.instance.DrawSkill(skill.skillInfo);
            }
        }
    }

    private void OnPointerExit(PointerEventData data)
    {
        mouseOver = false;
        if (DescManager.instance != null)
        {
            if(helper != null)
            {
                helper.gameObject.SetActive(false);
            }

            if (MousePointer.instance != null)
            {
                if(SkillManager.instance.currSkill != null)
                    MousePointer.instance.mouseType = MouseType.Attack;
                else
                    MousePointer.instance.mouseType = MouseType.Normal;
            }

            if (SkillManager.instance.currSkill == null)
                DescManager.instance.DescOff();
        }
    }

    private IEnumerator FailEffect()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/fail2");
        AudioManager.Instance.PlayEffect(clip, 0.6f);
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlayEffect(clip, 0.6f);

        if (helper != null)
        {
            helper.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            helper.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
            helper.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            helper.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.2f);
            if (!mouseOver)
                helper.gameObject.SetActive(false);
        }

        fail = null;
    }

    private void SkillOnClick()
    {
        if (SkillManager.instance.effect != null)
            return;

        if(!coolDown)
        {
            if (GameManager.instance.player.stack >= skill.skillInfo.cost)
            {
                GameManager.instance.attackInput = false;
                GameManager.instance.skillManager.currSkill = skill;
                GameManager.instance.skillManager.currSkill.CallAni();

                if (MousePointer.instance != null)
                    MousePointer.instance.mouseType = MouseType.Attack;
            }
            else
            {
                if(fail != null)
                    StopCoroutine(fail);
                fail = StartCoroutine(FailEffect());
                print("코스트가 부족합니다");
            }
        }
        else
        {
            if (fail != null)
                StopCoroutine(fail);
            fail = StartCoroutine(FailEffect());
            print("재사용 대기중입니다");
        }
    }

    public void Init()
    {

        helper = GetComponentInChildren<TextMeshProUGUI>();
        if(helper != null)
            helper.gameObject.SetActive(false);

        skill = GetComponentInChildren<SkillFunc>();
        if(skill != null)
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(SkillOnClick);

            skill.Init();
            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
            UtillHelper.SetEvent(trigger, EventTriggerType.PointerEnter, OnPointerEnter);
            UtillHelper.SetEvent(trigger , EventTriggerType.PointerExit, OnPointerExit);
        }
    }
}
