using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagListener : MonoBehaviour
{
    void BagOpen()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/bagOpen");
        AudioManager.Instance.PlayEffect(clip);
    }

    void BagClose()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/bagClose");
        AudioManager.Instance.PlayEffect(clip);
    }
}
