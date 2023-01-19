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
            "한 점에 힘을 모아 힘껏 찌릅니다.\n" +
            "공격타입 : 범위(직선)\n" +
            "공격범위 : 2\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString() + "\n" +
            "데미지 : " + skillInfo.damage.ToString() + "\n";

    }

    public override void Skill()
    {
        base.Skill();
        GameManager.instance.player.Stab();
        SkillManager.instance.effect = StartCoroutine(Effect());
        //EndSkill();
    }
}
