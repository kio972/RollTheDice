using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighPotion : Items
{
    public override void Use()
    {
        print("»ç¿ë");
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float targetHp = player.curHp + player.maxHp * 0.4f;
            if(targetHp >= player.maxHp)
                targetHp = player.maxHp;
            player.playerStat.hpBar.UpdateHealthBar(player.curHp, targetHp, player.maxHp);
            player.curHp = targetHp;

            Abandon();
        }
    }

    public override void Init()
    {
        itemInfo.id = 2;

        base.Init();
    }

    private void Start()
    {
        Init();
    }
}
