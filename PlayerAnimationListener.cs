using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{
    private void FootR()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/footR");
        AudioManager.Instance.PlayEffect(clip);
    }

    private void FootL()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/footL");
        AudioManager.Instance.PlayEffect(clip);
    }

    private void Swing()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/swind");
        AudioManager.Instance.PlayEffect(clip);
    }

    private void Impact()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/impact");
        AudioManager.Instance.PlayEffect(clip);
    }
}
