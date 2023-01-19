using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPotion : Items
{
    public override void Use()
    {
        print("»ç¿ë");
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float targetHp = player.curHp + player.maxHp * 0.25f;
            if(targetHp >= player.maxHp)
                targetHp = player.maxHp;
            player.playerStat.hpBar.UpdateHealthBar(player.curHp, targetHp, player.maxHp);
            player.curHp = targetHp;

            Abandon();
        }
    }

    public override void Init()
    {
        itemInfo.id = 1;

        base.Init();
    }

    private void Start()
    {
        Init();
    }
}
