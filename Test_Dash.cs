using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Dash : SkillFunc
{

    public override void Init()
    {
        skillInfo.index = 3;
        skillInfo.cost = 3;
        skillInfo.skillLv = 1;

        skillInfo.targetRange = 2;

        skillInfo.rangePos = new List<int[]>();
        skillInfo.rangePos.Add(new int[2] { 0, 0 });


        skillInfo.type = AttackType.Target;
        skillInfo.damage = 10;

        skillInfo.skillName = "테스트스킬 4";
        skillInfo.skillDesc =
            "대상에게 대쉬하여 공격\n" +
            "공격타입 : 타겟\n" +
            "사거리 : 2\n" +
            "데미지 : " + skillInfo.damage.ToString() + "\n" +
            "설명끝";

        base.Init();
    }

    public override void Skill()
    {
        int targetRow = 0;
        int targetCol = 0;
        foreach (Tile tile in GameManager.instance.uiManager.mapController.tiles)
        {
            if (tile.inRange)
            {
                targetRow = tile.rowIndex;
                targetCol = tile.colIndex;
            }
        }

        // 타겟인덱스와 플레이어 인덱스 계산 후 방향(row,col)값 구해서 플레이어 방향 1칸 위치구함
        int[] vec = new int[2];
        vec = GameManager.instance.uiManager.mapController.GetDirVec(targetRow, targetCol, GameManager.instance.player.curRow, GameManager.instance.player.curCol);

        int moveRow = targetRow + vec[0];
        int moveCol = targetCol + vec[1];
        int index = GameManager.instance.uiManager.mapController.ReturnIndex(moveRow, moveCol);
        Vector3 dest = GameManager.instance.uiManager.mapController.tileTransform[index].position;

        
        StartCoroutine(GameManager.instance.player.Move(dest, 2f, true));
        GameManager.instance.player.curRow = moveRow;
        GameManager.instance.player.curCol = moveCol;

        EndSkill();
    }
}
