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
            "십자로 베기 공격을 가합니다.\n" +
            "공격타입 : 범위\n" +
            "공격범위 : 십자\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString() + "\n" +
            "데미지 : " + skillInfo.damage.ToString();

    }

    public override void Skill()
    {
        base.Skill();
        GameManager.instance.player.Slash();

        SkillManager.instance.effect = StartCoroutine(Effect(true));
    }
}
