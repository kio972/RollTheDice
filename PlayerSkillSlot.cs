using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerSkillSlot : MonoBehaviour
{
    public int[] skillIndexs;
    public List<Transform> skillSlotTrans;
    GraphicRaycaster raycaster;

    [SerializeField]
    SkillFunc curSkill;
    RectTransform curSkillRect;
    Image dragImg;
    RectTransform dragImgTrans;
    private bool isdrag = false;
    private Transform prevSkillTrans;
    private Transform nextSkillTrans;

    private void OnDrag(PointerEventData data)
    {
        if (curSkill != null)
        {
            if (dragImgTrans == null)
            {
                Canvas canvas = TownController.instance.uiManager.GetComponent<Canvas>();

                isdrag = true;
                GuildController.instance.skillLearn.skillDesc.gameObject.SetActive(false);
                dragImg = curSkill.transform.parent.GetComponent<Image>();
                prevSkillTrans = dragImg.transform.parent;
                dragImg.raycastTarget = false;
                dragImg.transform.SetParent(canvas.transform);
                dragImgTrans = dragImg.GetComponent<RectTransform>();
            }
            else
            {
                dragImgTrans.position = Input.mousePosition;
            }
        }
        else
        {
            OnPointerEnter(data);
        }
    }

    private void EquipSkill(RaycastResult results, Transform parent = null)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillEquip");
        AudioManager.Instance.PlayEffect(clip);

        RectTransform trans = results.gameObject.GetComponent<RectTransform>();
        if(dragImg != null)
        {
            dragImg.raycastTarget = true;
            if (parent != null)
                trans = parent.GetComponent<RectTransform>();
            dragImgTrans.transform.SetParent(trans.transform);
            dragImgTrans.position = trans.position;
            UpdateSlot();
            dragImgTrans = null;
        }
    }

    private void EndDrag(PointerEventData data)
    {
        // data 위치에서 레이캐스트하고, skillslot이있을경우만 슬롯에들어가게
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        if (results.Count > 0)
        {
            if (results[0].gameObject.tag == "SkillSlot")
            {
                EquipSkill(results[0]);
            }
            else if(results[0].gameObject.tag == "Skill")
            {
                //해당 스킬슬롯이랑 나랑 바뀌게 해야함
                Image image = results[0].gameObject.GetComponent<Image>();
                nextSkillTrans = image.transform.parent;
                if(prevSkillTrans != null)
                {
                    image.transform.SetParent(prevSkillTrans);
                    image.transform.position = prevSkillTrans.position;
                }

                EquipSkill(results[0], nextSkillTrans);
            }
        }

        if (dragImgTrans != null)
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/skillEquipFail");
            AudioManager.Instance.PlayEffect(clip);

            Destroy(dragImgTrans.gameObject);
            UpdateSlot();
            dragImgTrans = null;
        }

        prevSkillTrans = null;
        nextSkillTrans = null;
        dragImg = null;
        isdrag = false;
    }

    private void OnPointerDown(PointerEventData data)
    {
        if (!isdrag)
        {
            OnPointerEnter(data);
            if (Input.GetKey(KeyCode.Mouse1))
            {
                AudioClip clip = Resources.Load<AudioClip>("Sounds/skillEquipFail");
                AudioManager.Instance.PlayEffect(clip);
                Destroy(curSkill.transform.parent.gameObject);
                Invoke("UpdateSlot", 0.1f);
            }
        }
    }

    private void OnPointerExit(PointerEventData data)
    {
        if (!isdrag)
        {
            curSkill = null;
            GuildController.instance.skillLearn.skillDesc.gameObject.SetActive(false);
        }
    }

    private void OnPointerEnter(PointerEventData data)
    {
        if (!isdrag)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(data, results);
            if (results.Count > 0)
            {
                SkillFunc skill = results[0].gameObject.transform.GetComponentInChildren<SkillFunc>();
                curSkill = skill;
                GuildController.instance.skillLearn.skillDesc.SetSkillInfo(curSkill.skillInfo);
                GuildController.instance.skillLearn.skillDesc.gameObject.SetActive(true);
                RectTransform target = curSkill.transform.parent.GetComponent<RectTransform>();
                RectTransform desc = GuildController.instance.skillLearn.skillDesc.GetComponent<RectTransform>();
                float posY = target.position.y;
                posY = Mathf.Clamp(posY, 262, 802);
                desc.position = new Vector3(target.position.x -300, posY);
            }
        }
    }

    public void SetTrigger(Transform parent)
    {
        EventTrigger newTrigger = parent.gameObject.GetComponent<EventTrigger>();
        if (newTrigger == null)
            newTrigger = parent.gameObject.AddComponent<EventTrigger>();

        UtillHelper.SetEvent(newTrigger, EventTriggerType.PointerEnter, OnPointerEnter);
        UtillHelper.SetEvent(newTrigger, EventTriggerType.PointerExit, OnPointerExit);
        UtillHelper.SetEvent(newTrigger, EventTriggerType.PointerDown, OnPointerDown);
        UtillHelper.SetEvent(newTrigger, EventTriggerType.Drag, OnDrag);
        UtillHelper.SetEvent(newTrigger, EventTriggerType.EndDrag, EndDrag);
    }

    public void UpdateSlot()
    {
        //skillslot내에 스킬(이미지)가 있는지 확인하고,
        //해당 이미지의 이름과 같은 인덱스 넘버 추가, eveintTrigger없으면 추가하고, 함수추가
        
        int[] temp = new int[skillSlotTrans.Count];
        // 스킬슬롯이 비어있으면 -1, 아니면 해당스킬인덱스를 추가
        for(int i = 0; i < skillSlotTrans.Count; i++)
        {
            SkillFunc skill = skillSlotTrans[i].GetComponentInChildren<SkillFunc>();
            if(skill != null)
            {
                temp[i] = skill.skillInfo.index;
                if (skillIndexs[i] != temp[i])
                {
                    SetTrigger(skill.transform.parent);
                }
            }
            else
            {
                temp[i] = -1;
            }
        }

        skillIndexs = temp;
        SaveSkill();
    }


    public void SaveSkill()
    {
        GameData.quickSlotSkills = new List<int>();
        foreach (int index in skillIndexs)
        {
            if(index != -1)
            {
                GameData.quickSlotSkills.Add(index);
            }
        }
    }

    private void LoadSkill()
    {
        skillIndexs = new int[skillSlotTrans.Count];
        for (int i = 0; i < skillIndexs.Length; i++)
        {
            skillIndexs[i] = -1;
        }

        if (GameData.quickSlotSkills != null)
        {
            for(int i = 0; i < GameData.quickSlotSkills.Count; i++)
            {
                skillIndexs[i] = GameData.quickSlotSkills[i];
                if (skillIndexs[i] == -1)
                    continue;
                Image skillIcon = Resources.Load<Image>("Prefab/Skill/QuickSkill");
                skillIcon = Instantiate(skillIcon, skillSlotTrans[i]);
                Sprite sprite = Resources.Load<Sprite>(DataManger.SkillData[skillIndexs[i]]["Icon"]);
                if (sprite != null)
                    skillIcon.sprite = sprite;
                SkillFunc skill = Resources.Load<SkillFunc>(DataManger.SkillData[skillIndexs[i]]["Path"]);
                if(skill != null)
                {
                    skill = Instantiate(skill, skillIcon.transform);
                    skill.Init();
                    SetTrigger(skill.transform.parent);
                }
            }
        }
    }

    public void Init()
    {
        raycaster = TownController.instance.canvas.GetComponent<GraphicRaycaster>();
        skillSlotTrans = new List<Transform>();
        int count = 0;
        while(true)
        {
            Transform slot = transform.Find("Slot" + count.ToString());
            if (slot != null)
            {
                skillSlotTrans.Add(slot);
            }
            else
                break;

            count++;
        }

        LoadSkill();
    }
}
