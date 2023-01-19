using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herb : EnemyController
{
    private SpriteRenderer sprite;
    IEnumerator DieEffect()
    {
        float elapsed = 0;
        while(elapsed < 1)
        {
            elapsed += Time.deltaTime;
            sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 1 - elapsed);
            yield return null;
        }

        elapsed = 1;
        sprite.color = Color.clear;
        Destroy(this.gameObject, 3f);
    }

    public void DestroyHerb()
    {
        // 허브 파괴, 에너미 목록에서 이거 제거
        print("herb Destroy");
        GameManager.instance.enemys.Remove(this);
        if(curHp != 0)
            hpBar.UpdateHealthBar(curHp, 0, maxHp);

        StartCoroutine(DieEffect());
    }

    public override void Behavior()
    {
        EndEnemyTrun();
    }

    public override void TakeDamage(float damage, Controller attacker)
    {
        if (damage > 0)
        {
            if (hpBar != null)
            {
                hpBar.UpdateHealthBar(curHp, curHp - 1, maxHp);
            }

            curHp -= 1;
            if(curHp <= 0 && !isDead)
            {
                // 공격자에게 디버프 / 버프를 줄 수 있도록 만들어야함
                isDead = true;
                attacker.TakeBuff("Prefab/Buff/HerbCollect", 1);
                Invoke("DestroyHerb", 0.1f);
            }
            DamageText.instance.Damage(transform, 1);
        }
    }

    public override void Init()
    {
        enemyName = "약초";
        enemyDesc = "건강해질것 같은 향이 난다.\n채집하려면 두번의 수고가 필요할것같다.";

        maxHp = 2;
        curHp = 2;

        sprite = GetComponentInChildren<SpriteRenderer>();
        base.Init();
    }

}
