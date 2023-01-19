using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPotion : Items
{
    public override void Use()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float targetHp = player.curHp + player.maxHp * 0.1f;
            if(targetHp >= player.maxHp)
                targetHp = player.maxHp;
            player.playerStat.hpBar.UpdateHealthBar(player.curHp, targetHp, player.maxHp);
            player.curHp = targetHp;

            Abandon();
        }
    }

    public override void Init()
    {
        itemInfo.id = 0;

        base.Init();
    }
}
