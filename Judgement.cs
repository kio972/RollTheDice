using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : SkillFunc
{
    public override void CallAni()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillReady");
        AudioManager.Instance.PlayEffect(clip);

        GameManager.instance.player.ReturnToIdle();
        GameManager.instance.player.Buff();
    }

    public override void Init()
    {
        skillInfo.index = 4;

        base.Init();

        skillInfo.skillDesc =
            "����Ÿ�� : ����\n" +
            "��Ÿ� : 2\n" +
            "���ݹ��� : ����\n" +
            "���� ���ð� : " + skillInfo.coolTime.ToString() + "\n" +
            "������ : " + skillInfo.damage.ToString();

    }

    private void LightEffect()
    {
        foreach(Tile tiles in GameManager.instance.mapController.tiles)
        {
            if(tiles.inRange)
                FxEffectManager.Instance.PlayEffect("Prefab/Battle/Effect/LightSphereBlast", tiles.transform.position);
        }
    }

    public override void Skill()
    {
        LightEffect();
        AudioClip clip = Resources.Load<AudioClip>("Sounds/lightning");
        AudioManager.Instance.PlayEffect(clip);

        base.Skill();

        SkillManager.instance.effect = StartCoroutine(Effect(true));
        GameManager.instance.player.Invoke("ReturnToIdle", 1.0f);

    }
}
