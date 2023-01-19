using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPotion : Items
{
    public override void Use()
    {
        print("»ç¿ë");
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.maxHp += 5;
            float targetHp = player.curHp + 5;
            if (targetHp >= player.maxHp)
                targetHp = player.maxHp;
            player.playerStat.hpBar.UpdateHealthBar(player.curHp, targetHp, player.maxHp);
            player.curHp = targetHp;

            Abandon();
        }

    }

    public override void Init()
    {
        itemInfo.id = 9;

        base.Init();
    }
}
