using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 5;
        buffInfo.name = "�Ź���";
        buffInfo.description = "�� ���� ���۵ɶ� ��ø�� �ൿ���� 1 �Ҵ´�.";
    }

    protected override void Effect()
    {
        // �� ���۽� ���ð���ؼ� ������
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            GameManager.instance.player.stack -= stack;
            GameManager.instance.uiManager.stackUpdater.UpdateStack();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
