using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public struct BuffInfo
{
    public string name;
    public string description;
}

public class Buff : MonoBehaviour
{
    public BuffInfo buffInfo;

    [SerializeField]
    protected Controller target;

    public int stack;
    public int id;

    private TextMeshProUGUI text;
    private bool mouseOver;

    EventTrigger eventTrigger;

    private void OnPointerExit(PointerEventData data)
    {
        if (PlayerStat.instance != null)
        {
            PlayerStat.instance.popUp.gameObject.SetActive(false);
            mouseOver = false;
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;
    }

    private void OnPointerEnter(PointerEventData data)
    {
        if (PlayerStat.instance != null)
        {
            PlayerStat.instance.popUp.gameObject.SetActive(true);
            PlayerStat.instance.popUp.UpdateInfo(this);
            mouseOver = true;
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Interact;
    }


    private void SetMouseOver()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
            UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerEnter, OnPointerEnter);
            UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerExit, OnPointerExit);
        }
    }

    public void SetStack()
    {
        if(stack == 1)
        {
            text.gameObject.SetActive(false);
        }   
        else if(stack > 1)
        {
            text.gameObject.SetActive(true);
            text.text = stack.ToString();
        }
    }

    protected virtual void Effect()
    {

    }

    public virtual void Init(Controller target, int stack = 1)
    {
        this.target = target;
        text = GetComponentInChildren<TextMeshProUGUI>();
        this.stack = stack;
        SetStack();
        SetMouseOver();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (mouseOver && PlayerStat.instance != null)
        {
            Vector2 pos = Input.mousePosition;
            pos.x += 160;
            pos.y += 80;
            PlayerStat.instance.popUp.transform.position = pos;
        }
    }
}
