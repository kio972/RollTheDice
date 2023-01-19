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
            "공격타입 : 지점\n" +
            "사거리 : 2\n" +
            "공격범위 : 십자\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString() + "\n" +
            "데미지 : " + skillInfo.damage.ToString();

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
