using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonSkillTree : MonoBehaviour
{
    Transform treeZone;
    Transform skillZone;
    public Transform SkillZone { get { return skillZone; } }
    Coroutine holding;
    GraphicRaycaster raycaster;
    [SerializeField]
    private SkillFunc curSkill;
    Image dragImg;
    RectTransform dragImgTrans;
    private bool isdrag = false;
    private float learnHoldTime = 1.9f;

    AudioSource effectClip;
    public ParticleSystem effect;
    public RectTransform effectImg;

    private bool NeedSkillCheck(SkillInfo skillInfo)
    {
        if (skillInfo.needSkill == null)
            return true;

        bool haveSkill = false;
        foreach (int needIndex in skillInfo.needSkill)
        {
            foreach (int learnedIndex in GameData.playerSkills)
            {
                if (needIndex == learnedIndex)
                {
                    haveSkill = true;
                    break;
                }
            }

            if (!haveSkill)
                break;
        }
        return haveSkill;
    }

    private bool CheckSkillLearnAvail(SkillInfo skillInfo)
    {
        if (!NeedSkillCheck(skillInfo))
        {
            print("필요 스킬을 배우지 못했습니다");
            return false;
        }

        if(TownController.instance.player.skillPoint < skillInfo.needSkillPoint)
        {
            print("필요 스킬포인트가 부족합니다");
            return false;
        }

        if(TownController.instance.player.permanentSkillPoint < skillInfo.needPermanentPoint)
        {
            print("필요 영구스킬포인트가 부족합니다");
            return false;
        }

        if (TownController.instance.player.gold < skillInfo.needGold)
        {
            print("필요 골드가 부족합니다");
            return false;
        }

        return true;
    }

    private void PlayEffectSound()
    {
        if (effectClip != null)
        {
            
            effectClip.Play();
        }
    }

    private void SetLearnSkill()
    {
        curSkill.isLeran = true;
        if (GameData.playerSkills != null)
            GameData.playerSkills.Add(curSkill.skillInfo.index);
        TownController.instance.player.skillPoint -= curSkill.skillInfo.needSkillPoint;
        GameData.playerSkillPoint -= curSkill.skillInfo.needSkillPoint;
        TownController.instance.player.permanentSkillPoint -= curSkill.skillInfo.needPermanentPoint;
        GameData.playerPermanentPoint -= curSkill.skillInfo.needPermanentPoint;
        TownController.instance.player.gold -= curSkill.skillInfo.needGold;
        GameData.playerGold -= curSkill.skillInfo.needGold;
        TownController.instance.player.statBar.UpdateStat();
    }

    private IEnumerator LearnHold()
    {
        PlayEffectSound();

        Transform skillImg = curSkill.transform.parent.transform;
        Image image = skillImg.GetComponent<Image>();
        float elapsed = 0;
        float b = 0.5f;
        while (elapsed < learnHoldTime)
        {
            b += Time.deltaTime / learnHoldTime / 2;
            elapsed += Time.deltaTime;
            if(image != null)
            {
                Color color = new Color(b, b, b, 1);
                image.color = color;
            }
            yield return null;
        }

        if (effectClip != null)
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/skillLearn");
            effectClip.clip = clip;
            effectClip.Play();
        }

        effectImg.transform.position = curSkill.transform.position;
        effect.Play();

        image.color = Color.white;
        SetLearnSkill();
        holding = null;
    }

    private void OverLapCheck()
    {
        for (int i = 0; i < GuildController.instance.quickSlot.skillIndexs.Length; i++)
        {
            if (GuildController.instance.quickSlot.skillIndexs[i] == curSkill.skillInfo.index)
            {
                SkillFunc skill = GuildController.instance.quickSlot.skillSlotTrans[i].GetComponentInChildren<SkillFunc>();
                if (skill != null)
                {
                    skill.transform.parent.gameObject.SetActive(false);
                    Destroy(skill.transform.parent.gameObject, 0.1f);
                }
                break;
            }
        }
    }

    private void SetSkill(Transform trans)
    {
        dragImg.raycastTarget = true;
        dragImgTrans.transform.SetParent(trans.transform);
        dragImgTrans.position = trans.position;
        EventTrigger trigger = dragImgTrans.GetComponent<EventTrigger>();
        Destroy(trigger);
        GuildController.instance.quickSlot.UpdateSlot();
        dragImgTrans = null;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillEquip");
        AudioManager.Instance.PlayEffect(clip);
    }

    private void EndDrag(PointerEventData data)
    {
        // data 위치에서 레이캐스트하고, skillslot이있을경우만 슬롯에들어가게
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        if (results.Count > 0)
        {
            if(results[0].gameObject.tag == "SkillSlot")
            {
                OverLapCheck();

                RectTransform trans = results[0].gameObject.GetComponent<RectTransform>();
                SetSkill(trans);
            }
            else if (results[0].gameObject.tag == "Skill")
            {
                CommonSkillTree skillTree = results[0].gameObject.GetComponentInParent<CommonSkillTree>();
                if(skillTree == null)
                {
                    OverLapCheck();
                    Transform parentTrans = results[0].gameObject.transform.parent;
                    results[0].gameObject.SetActive(false);
                    Destroy(results[0].gameObject, 0.1f);
                    SetSkill(parentTrans);
                }
            }
        }

        if (dragImgTrans != null)
        {
            Destroy(dragImgTrans.gameObject);
            dragImgTrans = null;

            AudioClip clip = Resources.Load<AudioClip>("Sounds/skillEquipFail");
            AudioManager.Instance.PlayEffect(clip);
        }

        dragImg = null;
        isdrag = false;
    }

    private void OnDrag(PointerEventData data)
    {
        OnPointerEnter(data);
        isdrag = true;
        if (curSkill == null)
            OnPointerEnter(data);
        else if(curSkill.isLeran)
        {
            if (dragImgTrans == null)
            {
                AudioClip clip = Resources.Load<AudioClip>("Sounds/skillDrag");
                AudioManager.Instance.PlayEffect(clip);

                Canvas canvas = TownController.instance.uiManager.GetComponent<Canvas>();
                GuildController.instance.skillLearn.skillDesc.gameObject.SetActive(false);
                Image img = curSkill.transform.parent.GetComponent<Image>();
                Vector2 pos = img.transform.position;
                dragImg = Instantiate(img, canvas.transform);
                dragImg.raycastTarget = false;
                dragImgTrans = dragImg.GetComponent<RectTransform>();
                dragImgTrans.position = pos;
                SkillFunc skill = dragImg.GetComponentInChildren<SkillFunc>();
                if (skill != null)
                    skill.Init();
            }
            else
                dragImgTrans.position = Input.mousePosition;
        }
    }



    private void OnPointerEnter(PointerEventData data)
    {
        if(holding == null && !isdrag)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(data, results);
            if (results.Count > 0)
            {
                SkillFunc skill = results[0].gameObject.transform.GetComponentInChildren<SkillFunc>();
                if(skill != null)
                {
                    curSkill = skill;
                    GuildController.instance.skillLearn.skillDesc.SetSkillInfo(curSkill.skillInfo);
                    GuildController.instance.skillLearn.skillDesc.gameObject.SetActive(true);
                    RectTransform target = curSkill.transform.parent.GetComponent<RectTransform>();
                    RectTransform desc = GuildController.instance.skillLearn.skillDesc.GetComponent<RectTransform>();
                    float posY = target.position.y;
                    posY = Mathf.Clamp(posY, 262, 802);
                    desc.position = new Vector3(target.position.x + 300, posY);
                }
            }
        }
    }

    private void OnPointerExit(PointerEventData data)
    {
        if(holding == null && !isdrag)
        {
            curSkill = null;
            GuildController.instance.skillLearn.skillDesc.gameObject.SetActive(false);
        }
    }

    private void OnPointerDown(PointerEventData data)
    {
        if(!isdrag)
        {
            OnPointerEnter(data);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!curSkill.isLeran)
                {
                    if (CheckSkillLearnAvail(curSkill.skillInfo))
                    {
                        holding = StartCoroutine(LearnHold());
                    }
                }
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (curSkill.isLeran)
                {
                    for(int i = 0; i < GuildController.instance.quickSlot.skillIndexs.Length; i++)
                    {
                        if (GuildController.instance.quickSlot.skillIndexs[i] == curSkill.skillInfo.index)
                            return;
                    }

                    // 퀵슬롯 빈칸찾아서 들어가게만들어야함
                    for(int i = 0; i < GuildController.instance.quickSlot.skillIndexs.Length; i++)
                    {
                        if(GuildController.instance.quickSlot.skillIndexs[i] == -1)
                        {
                            AudioClip clip = Resources.Load<AudioClip>("Sounds/skillEquip");
                            AudioManager.Instance.PlayEffect(clip);

                            Image img = curSkill.transform.parent.GetComponent<Image>();
                            img = Instantiate(img, GuildController.instance.quickSlot.skillSlotTrans[i]);
                            RectTransform imgTrans = img.GetComponent<RectTransform>();
                            RectTransform targetTrans = GuildController.instance.quickSlot.skillSlotTrans[i].GetComponent<RectTransform>();
                            imgTrans.position = targetTrans.position;
                            SkillFunc skill = img.GetComponentInChildren<SkillFunc>();
                            if (skill != null)
                                skill.Init();
                            EventTrigger trigger = img.GetComponent<EventTrigger>();
                            Destroy(trigger);
                            GuildController.instance.quickSlot.UpdateSlot();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnPointerUp(PointerEventData data)
    {
        if(holding != null)
        {
            if (effectClip != null)
                effectClip.Stop();

            StopCoroutine(holding);
            holding = null;
            if (!curSkill.isLeran)
            {
                Transform skillImg = curSkill.transform.parent.transform;
                Image image = skillImg.GetComponent<Image>();
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        OnPointerEnter(data);
        if (curSkill == null)
            OnPointerExit(data);
    }

    private void SetLearnEffect()
    {
        effectClip = GuildController.instance.transform.GetComponentInChildren<AudioSource>();
        if(effectClip != null)
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/skillHold");
            effectClip.clip = clip;
        }
        
        effect = GuildController.instance.transform.Find("EffectParticle").GetComponent<ParticleSystem>();
        effectImg = GuildController.instance.guildUI.Find("Skill/EffectImg") as RectTransform;
    }

    public void SetEventTrigger(Transform parent)
    {
        EventTrigger[] triggers = parent.GetComponentsInChildren<EventTrigger>();
        foreach (EventTrigger trigger in triggers)
        {
            UtillHelper.SetEvent(trigger, EventTriggerType.PointerEnter, OnPointerEnter);
            UtillHelper.SetEvent(trigger, EventTriggerType.PointerExit, OnPointerExit);
            UtillHelper.SetEvent(trigger, EventTriggerType.PointerDown, OnPointerDown);
            UtillHelper.SetEvent(trigger, EventTriggerType.PointerUp, OnPointerUp);
            UtillHelper.SetEvent(trigger, EventTriggerType.Drag, OnDrag);
            UtillHelper.SetEvent(trigger, EventTriggerType.EndDrag, EndDrag);
        }
    }

    private void InitSkills(Transform parent)
    {
        SkillFunc[] skills = parent.GetComponentsInChildren<SkillFunc>();
        foreach(SkillFunc skill in skills)
        {
            skill.Init();
            if (!skill.isLeran)
            {
                Transform skillImg = skill.transform.parent.transform;
                Image image = skillImg.GetComponent<Image>();
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    public void Init()
    {
        SetLearnEffect();
        raycaster = TownController.instance.canvas.GetComponent<GraphicRaycaster>();
        treeZone = transform.Find("TreeZone");
        skillZone = transform.Find("SkillZone");
        if(skillZone != null)
        {
            InitSkills(skillZone);
            SetEventTrigger(skillZone);
        }
    }
}
