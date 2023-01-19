using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MovingAttack : SkillFunc
{
    public override void Init()
    {
        skillInfo.index = 1;
        skillInfo.cost = 2;
        skillInfo.skillLv = 1;

        skillInfo.targetRange = 0;

        skillInfo.rangePos = new List<int[]>();
        skillInfo.rangePos.Add(new int[2] { 0, 1 });
        skillInfo.rangePos.Add(new int[2] { 0, 2 });
        skillInfo.rangePos.Add(new int[2] { 0, 3 });

        skillInfo.type = AttackType.Range;
        skillInfo.damage = 20;

        skillInfo.skillName = "테스트스킬 5";
        skillInfo.skillDesc =
            "직선으로 세칸이동하며 공격합니다. 도착지가 빈 타일이어야 사용가능\n" +
            "공격타입 : 범위(직선)\n" +
            "공격범위 : 3\n" +
            "데미지 : " + skillInfo.damage.ToString() + "\n" +
            "설명끝";

        base.Init();
    }

    public override void Skill()
    {
        base.Skill();

        EndSkill();
    }
}
