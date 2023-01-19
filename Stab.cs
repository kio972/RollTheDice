using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : SkillFunc
{
    public override void CallAni()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillReady");
        AudioManager.Instance.PlayEffect(clip);

        GameManager.instance.player.ReturnToIdle();
        GameManager.instance.player.Stab();
    }

    public override void Init()
    {
        skillInfo.index = 1;
        base.Init();

        skillInfo.skillDesc =
            "�� ���� ���� ��� ���� ��ϴ�.\n" +
            "����Ÿ�� : ����(����)\n" +
            "���ݹ��� : 2\n" +
            "���� ���ð� : " + skillInfo.coolTime.ToString() + "\n" +
            "������ : " + skillInfo.damage.ToString() + "\n";

    }

    public override void Skill()
    {
        base.Skill();
        GameManager.instance.player.Stab();
        SkillManager.instance.effect = StartCoroutine(Effect());
        //EndSkill();
    }
}
