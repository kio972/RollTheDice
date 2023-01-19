using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSlash : SkillFunc
{
    public override void CallAni()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillReady");
        AudioManager.Instance.PlayEffect(clip);

        GameManager.instance.player.ReturnToIdle();
        GameManager.instance.player.Slash();
    }

    public override void Init()
    {
        skillInfo.index = 2;

        base.Init();

        skillInfo.skillDesc =
            "���ڷ� ���� ������ ���մϴ�.\n" +
            "����Ÿ�� : ����\n" +
            "���ݹ��� : ����\n" +
            "���� ���ð� : " + skillInfo.coolTime.ToString() + "\n" +
            "������ : " + skillInfo.damage.ToString();

    }

    public override void Skill()
    {
        base.Skill();
        GameManager.instance.player.Slash();

        SkillManager.instance.effect = StartCoroutine(Effect(true));
    }
}
