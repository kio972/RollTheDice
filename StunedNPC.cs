using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunedNPC : EnemyController
{
    private SpriteRenderer sprite;

    protected override void PointerOver()
    {
        if (GameManager.instance.battleEnd || GameManager.instance.phase == 0)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "NPC")
            {
                mouseOver = true;
                EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();

                if (DescManager.instance != null)
                    DescManager.instance.DrawEnemy(enemy);
            }
        }
        else if (mouseOver)
        {
            mouseOver = false;

            if (DescManager.instance != null)
            {
                if (GameManager.instance.skillManager.currSkill != null)
                {
                    DescManager.instance.DrawSkill(GameManager.instance.skillManager.currSkill.skillInfo);
                }
                else
                    DescManager.instance.DescOff();
            }
        }
    }

    IEnumerator DieEffect()
    {
        float elapsed = 0;
        while (elapsed < 1)
        {
            elapsed += Time.deltaTime;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1 - elapsed);
            yield return null;
        }

        elapsed = 1;
        sprite.color = Color.clear;
        GameManager.instance.enemys.Remove(this);
        Destroy(gameObject, 0.1f);
    }

    public void DestroyNPC()
    {
        GameManager.instance.enemys.Remove(this);
        StartCoroutine(DieEffect());
    }

    public override void TakeDamage(float damage, Controller attacker)
    {
        base.TakeDamage(damage, attacker);

        if (isDead)
            DestroyNPC();
    }

    public override void Behavior()
    {
        EndEnemyTrun();
    }

    public override void Init()
    {
        enemyName = "???";
        enemyDesc = "기절하여 쓰러져있다.\n가만히 둔다면 무방비로 죽게될것같다.";

        maxHp = 45;
        curHp = 45;

        sprite = GetComponentInChildren<SpriteRenderer>();
        base.Init();
    }

}
