using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillFunc
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
        skillInfo.index = 0;
        base.Init();

        skillInfo.skillDesc =
            "�ֺ� �ݰ��� �� �Ѹ��� �����մϴ�.\n" +
            "����Ÿ�� : �Ϲ���\n" +
            "���ݹ��� : 1\n" +
            "���� ���ð� : " + skillInfo.coolTime.ToString() + "\n" +
            "������ : " + skillInfo.damage.ToString();
    }

    public override void Skill()
    {
        base.Skill();
        GameManager.instance.player.Slash();
        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
