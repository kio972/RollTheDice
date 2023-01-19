using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elixir : Items
{
    public override void Use()
    {
        print("»ç¿ë");
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.playerStat.hpBar.UpdateHealthBar(player.curHp, player.maxHp, player.maxHp);
            player.curHp = player.maxHp;

            Abandon();
        }
    }

    public override void Init()
    {
        itemInfo.id = 3;

        base.Init();
    }

    private void Start()
    {
        Init();
    }
}
