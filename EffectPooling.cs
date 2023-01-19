using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooling : MonoBehaviour
{
    public IEnumerator EffectClip(AudioClip clip, float volume)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();

        float elpasedTime = 0;
        while (elpasedTime < clip.length)
        {
            elpasedTime += Time.deltaTime;
            yield return null;
        }

        audio.Stop();
        AudioManager.effectSounds.Add(this);
        gameObject.SetActive(false);
    }
}
