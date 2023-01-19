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

        skillInfo.skillName = "�׽�Ʈ��ų 5";
        skillInfo.skillDesc =
            "�������� ��ĭ�̵��ϸ� �����մϴ�. �������� �� Ÿ���̾�� ��밡��\n" +
            "����Ÿ�� : ����(����)\n" +
            "���ݹ��� : 3\n" +
            "������ : " + skillInfo.damage.ToString() + "\n" +
            "����";

        base.Init();
    }

    public override void Skill()
    {
        base.Skill();

        EndSkill();
    }
}
