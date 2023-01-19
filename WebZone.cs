using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebZone : EffectZone
{
    private int prevPlayerIndex = -1;
    
    private void EffectCheck()
    {
        if (GameManager.instance.mapController.tiles[tileIndex].onTarget == OnTile.Empty)
            gameObject.SetActive(false);

        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        if (prevPlayerIndex != playerIndex)
        {
            if(playerIndex == tileIndex)
            {
                GameManager.instance.player.TakeBuff("Prefab/Buff/Web", 5);
                gameObject.SetActive(false);
            }

            prevPlayerIndex = playerIndex;
        }
    }

    public override void Init()
    {
        Tile tile = GetComponentInParent<Tile>();
        tileIndex = tile.tileIndex;
        effectId = 3;
    }

    // Update is called once per frame
    void Update()
    {
        EffectCheck();
    }
}
