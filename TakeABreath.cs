using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeABreath : SkillFunc
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
        skillInfo.index = 5;

        base.Init();

        skillInfo.skillDesc =
            "��� ��ų���� ���� ���ð��� 1�� ���δ�.\n" +
            "��ųŸ�� : ����\n" +
            "���� ���ð� : " + skillInfo.coolTime.ToString();
    }

    public override void Skill()
    {
        SkillManager.instance.CoolDown();

        GameManager.instance.player.stack -= skillInfo.cost;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();
        SkillBtn btn = GetComponentInParent<SkillBtn>();
        btn.CoolDown = true;
        btn.coolTime = skillInfo.coolTime;

        GameManager.instance.player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/shout");
        AudioManager.Instance.PlayEffect(clip);

        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
