using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void EventFunc(PointerEventData data);

public static class UtillHelper
{
    public static T GetComponent<T>(string path, Transform parent, bool init = false) where T : Behaviour
    {
        Transform trans = parent.transform.Find(path);
        if (trans != null)
        {
            T temp = trans.GetComponent<T>();
            if(temp != null)
            {
                if (init)
                    temp.transform.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
                return temp;
            }
            else
                return default(T);
        }
        return default(T);
    }

    public static void SetEvent(EventTrigger trigger, EventTriggerType eventType, EventFunc eventFunc)
    {
        EventTrigger.Entry newTriggerEvent = new EventTrigger.Entry();
        newTriggerEvent.eventID = eventType;
        newTriggerEvent.callback.AddListener((data) => { eventFunc((PointerEventData)data); });
        trigger.triggers.Add(newTriggerEvent);
    }
}
