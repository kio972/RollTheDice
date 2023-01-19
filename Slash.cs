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
            "주변 반경의 적 한명을 공격합니다.\n" +
            "공격타입 : 일반형\n" +
            "공격범위 : 1\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString() + "\n" +
            "데미지 : " + skillInfo.damage.ToString();
    }

    public override void Skill()
    {
        base.Skill();
        GameManager.instance.player.Slash();
        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
