using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regroup : SkillFunc
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
        skillInfo.index = 3;

        base.Init();

        skillInfo.skillDesc =
            "숨을 골라 다음턴이 시작될 때 코스트를 1획득합니다.\n" +
            "스킬타입 : 버프\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString();
    }

    public override void Skill()
    {
        GameManager.instance.player.TakeBuff("Prefab/Buff/ReadyBattle", 2);
        GameManager.instance.player.stack -= skillInfo.cost;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();
        GameManager.instance.player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);

        SkillBtn btn = GetComponentInParent<SkillBtn>();
        btn.CoolDown = true;
        btn.coolTime = skillInfo.coolTime;

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/shout");
        AudioManager.Instance.PlayEffect(clip);

        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
