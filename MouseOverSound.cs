using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    private void MouseOver(PointerEventData data)
    {
        AudioManager.Instance.PlayEffect(clip, 0.2f);
    }

    void Start()
    {
        if(clip == null)
            clip = Resources.Load<AudioClip>("Sounds/mouseOver");
        EventTrigger trigger = GetComponent<EventTrigger>();
        if(trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();
        UtillHelper.SetEvent(trigger, EventTriggerType.PointerEnter, MouseOver);
    }
}
