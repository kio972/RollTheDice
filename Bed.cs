using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private bool canRest = true;
    public bool CanRest { get { return canRest; } }

    private void Restore()
    {
        PlayerController player = TownController.instance.player;
        float targetHp = player.curHp + player.maxHp * 0.3f;
        if (targetHp >= player.maxHp)
            targetHp = player.maxHp;
        player.playerStat.hpBar.UpdateHealthBar(player.curHp, targetHp, player.maxHp);
        player.curHp = targetHp;
        player.gold -= 30;
        player.statBar.UpdateStat();

        canRest = false;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = Color.clear;

        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if (fade != null)
            fade.FadeI();

        TownController.instance.npc.SetNPCName("여관주인", "밥");
        TownController.instance.npc.SetConv("잘 쉬었나?\n이제 출발하게");
    }

    public void Rest()
    {
        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if (fade != null)
            fade.FadeO();
        TownController.instance.player.statBar.UpdateStat();
        Invoke("Restore", 1.0f);
        // 골드차감, 화면 페이드아웃-인, 체력회복
    }

}
