using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceZone : EffectZone
{
    private int prevPlayerIndex = -1;
    private int count = 3;
    public int startTurn = -1;

    private void EffectCheck()
    {
        if (GameManager.instance.turn == startTurn + count + 1)
            gameObject.SetActive(false);
        if (GameManager.instance.mapController.tiles[tileIndex].onTarget == OnTile.Empty)
            gameObject.SetActive(false);

        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        if (prevPlayerIndex != playerIndex)
        {
            if(playerIndex == tileIndex)
            {
                IceImmune iceImmune = GameManager.instance.player.tokenSpace.GetComponentInChildren<IceImmune>();
                if(iceImmune == null)
                    GameManager.instance.player.TakeBuff("Prefab/Buff/Ice", 9);
            }

            prevPlayerIndex = playerIndex;
        }
    }

    public override void Init()
    {
        Tile tile = GetComponentInParent<Tile>();
        tileIndex = tile.tileIndex;
        effectId = 2;
        startTurn = GameManager.instance.turn;
    }

    // Update is called once per frame
    void Update()
    {
        EffectCheck();
    }
}
