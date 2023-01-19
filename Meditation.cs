using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meditation : SkillFunc
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
        skillInfo.index = 8;

        skillInfo.skillDesc =
            "공격 턴을 즉시 종료하고 내 행동력을 두배로 늘린다.\n" +
            "스킬타입 : 버프\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString();

        base.Init();
    }

    private void DoubleStack()
    {
        GameManager.instance.player.stack *= 2;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();
        GameManager.instance.endInput = true;
    }

    public override void Skill()
    {
        GameManager.instance.player.stack -= skillInfo.cost;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();
        SkillBtn btn = GetComponentInParent<SkillBtn>();
        btn.CoolDown = true;
        btn.coolTime = skillInfo.coolTime;

        Invoke("DoubleStack", 1f);
        GameManager.instance.player.SetTrigger("Buff1");

        GameManager.instance.player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;

        AudioManager.Instance.PlayEffect("charge");

        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
