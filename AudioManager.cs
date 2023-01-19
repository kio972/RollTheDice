using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource backGround;
    Transform effects;
    public static List<EffectPooling> effectSounds = new List<EffectPooling>();

    public void PlayEffect(AudioClip clip, float volume2 = 1)
    {
        if (clip == null)
            return;

        float finalVolume = SaveManager.Instance.settingData.fxVolume * SaveManager.Instance.settingData.totalVolume * volume2;
        if (effectSounds.Count > 1)
        {
            EffectPooling effect = effectSounds[0];
            effect.gameObject.SetActive(true);
            StartCoroutine(effect.EffectClip(clip, finalVolume));
            effectSounds.Remove(effect);
        }
        else
        {
            EffectPooling effect = Resources.Load<EffectPooling>("Prefab/Effect/EffectClip");
            effect = Instantiate(effect, effects);
            StartCoroutine(effect.EffectClip(clip, finalVolume));
        }
    }

    public void PlayEffect(string audioName, float volume2 = 1)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + audioName);
        PlayEffect(clip, volume2);
    }

    public void StopBG()
    {
        //if (backGround == null)
        //    Init();

        backGround.Stop();
    }

    public void PlayBG(AudioClip clip)
    {
        if (clip == null)
            return;

        //if (backGround == null)
        //    Init();

        backGround.clip = clip;
        backGround.Play();
    }

    protected override void Awake()
    {
        Init();
        base.Awake();
    }

    private void Init()
    {
        effects = transform.Find("Effects");
        if(effects == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(transform);
            gameObject.name = "Effects";
            effects = gameObject.transform;
        }

        backGround = UtillHelper.GetComponent<AudioSource>("BackGround", transform);
        if(backGround == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(transform);
            gameObject.name = "BackGround";
            gameObject.AddComponent<AudioSource>();
            backGround = gameObject.GetComponent<AudioSource>();
        }
    }

    public void Update()
    {
        if (backGround == null)
            Init();

        backGround.volume = SaveManager.Instance.settingData.bgVolume * SaveManager.Instance.settingData.totalVolume;
    }
}
