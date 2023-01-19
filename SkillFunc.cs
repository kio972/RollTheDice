using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillFunc : MonoBehaviour
{
    public SkillInfo skillInfo;
    public bool isLeran = false;
    public float finalDamage;

    public virtual IEnumerator Effect(bool dontMove = false)
    {
        GameManager.instance.skillManager.isAttacking = true;
        if (CameraController.instance != null)
            CameraController.instance.move = false;

        GameManager.instance.mapController.ResetTilesColor();
        Camera cam = Camera.main;
        Vector3 back = cam.transform.position;
        float elapsed = 0;
        float size = cam.orthographicSize;
        Vector3 pos = GameManager.instance.mapController.tiles[GameManager.instance.mapController.curTile.tileIndex].transform.position;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 targetPos = playerPos + ((pos - playerPos) * 0.3f);
        if (dontMove)
            targetPos = pos;
        targetPos.z = -10;
        cam.orthographicSize = 2.5f;
        cam.transform.position = targetPos;
        targetPos.z = 0;
        while (elapsed < 2f)
        {
            elapsed += Time.deltaTime;
            if(!dontMove)
                GameManager.instance.player.transform.position = Vector3.Lerp(GameManager.instance.player.transform.position, targetPos, Time.deltaTime * 4f);
            yield return null;
        }
        elapsed = 0;
        GameManager.instance.player.EndAttack();

        while (true)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, Time.deltaTime * 3);
            cam.transform.position = Vector3.Lerp(cam.transform.position, back, Time.deltaTime * 3);
            GameManager.instance.player.transform.position = Vector3.Lerp(GameManager.instance.player.transform.position, playerPos, Time.deltaTime * 3);
            if ((cam.transform.position - back).magnitude < 0.1f)
                break;
            yield return null;
        }

        GameManager.instance.player.transform.position = playerPos;
        cam.orthographicSize = size;
        cam.transform.position = back;
        EndSkill();
        SkillManager.instance.effect = null;
        if (CameraController.instance != null)
            CameraController.instance.move = true;
    }

    public virtual void CallAni()
    {

    }

    protected void LoadInfo(int index)
    {
        if (DataManger.SkillData != null)
        {
            skillInfo.index = int.Parse(DataManger.SkillData[index]["Index"]);
            skillInfo.skillName = DataManger.SkillData[index]["Name"];
            skillInfo.cost = float.Parse(DataManger.SkillData[index]["Cost"]);
            skillInfo.coolTime = int.Parse(DataManger.SkillData[index]["CoolTime"]);

            int type = int.Parse(DataManger.SkillData[index]["AttackType"]);
            AttackType attackType = AttackType.Basic;
            switch (type)
            {
                case 0:
                    attackType = AttackType.Basic;
                    break;
                case 1:
                    attackType = AttackType.Range;
                    break;
                case 2:
                    attackType = AttackType.Target;
                    break;
                case 3:
                    attackType = AttackType.Buff;
                    break;
            }
            skillInfo.type = attackType;

            skillInfo.damage = float.Parse(DataManger.SkillData[index]["Damage"]);

            int range = 0;
            if (int.TryParse(DataManger.SkillData[index]["TargetPos"], out range))
            {
                skillInfo.targetRange = range;
            }
            else
            {
                skillInfo.targetPos = new List<int[]>();
                string[] targetPos = DataManger.SkillData[index]["TargetPos"].Split(']');
                for (int i = 0; i < targetPos.Length; i++)
                {
                    string[] str = targetPos[i].Split('|');
                    str[0] = str[0].Replace("[", "");
                    int row = int.Parse(str[0]);
                    int col = int.Parse(str[1]);
                    int[] pos = new int[2] { row, col };
                    skillInfo.targetPos.Add(pos);
                }
            }

            if (int.TryParse(DataManger.SkillData[index]["RangePos"], out range))
            {
                skillInfo.range = range;
            }
            else
            {
                skillInfo.rangePos = new List<int[]>();
                string[] rangePos = DataManger.SkillData[index]["RangePos"].Split(']');
                for (int i = 0; i < rangePos.Length - 1; i++)
                {
                    string[] str = rangePos[i].Split('|');
                    str[0] = str[0].Replace("[", "");
                    int row = int.Parse(str[0]);
                    int col = int.Parse(str[1]);
                    int[] pos = new int[2] { row, col };
                    skillInfo.rangePos.Add(pos);
                }
            }

            int needSkill;
            if (int.TryParse(DataManger.SkillData[index]["NeedSkill"], out needSkill))
            {
                if (needSkill != -1)
                {
                    skillInfo.needSkill = new int[1] { needSkill };
                }
            }
            else
            {
                string[] skills = DataManger.SkillData[index]["NeedSkill"].Split('|');
                skillInfo.needSkill = new int[skills.Length];
                for (int i = 0; i < skills.Length; i++)
                {
                    skillInfo.needSkill[i] = int.Parse(skills[i]);
                }
            }

            


            int point;
            if (int.TryParse(DataManger.SkillData[index]["NeedPoint"], out point))
                skillInfo.needSkillPoint = point;
            if (int.TryParse(DataManger.SkillData[index]["NeedPermanent"], out point))
                skillInfo.needPermanentPoint = point;
            if (int.TryParse(DataManger.SkillData[index]["NeedGold"], out point))
                skillInfo.needGold = point;

            skillInfo.rangeImg = Resources.Load<Sprite>("Img/SkillIcon/SkillRange/" + DataManger.SkillData[index]["RangeImg"]);
        }
    }

    protected void EndSkill()
    {
        GameManager.instance.skillManager.currSkill = null;
        GameManager.instance.mapController.ResetTilesColor();
        GameManager.instance.mapController.UpdateOnTargetTiles();
        GameManager.instance.skillManager.isAttacking = false;
        GameManager.instance.uiManager.descManager.DescOff();
    }

    public List<int[]> RotateSkill()
    {
        List<int[]> range = new List<int[]>();
        switch(GameManager.instance.player.curDir)
        {
            case CharDir.W:
                foreach(int[] basePos in GameManager.instance.skillManager.currSkill.skillInfo.rangePos)
                {
                    int tempRow = basePos[0];
                    int tempCol = basePos[1];
                    range.Add(new int[2] {tempCol, tempRow});
                }
                return range;
            case CharDir.S:
                foreach (int[] basePos in GameManager.instance.skillManager.currSkill.skillInfo.rangePos)
                {
                    int tempRow = basePos[0];
                    int tempCol = basePos[1];
                    range.Add(new int[2] { -tempCol, tempRow });
                }
                return range;
            case CharDir.A:
                foreach (int[] basePos in GameManager.instance.skillManager.currSkill.skillInfo.rangePos)
                {
                    int tempRow = basePos[0];
                    int tempCol = basePos[1];
                    range.Add(new int[2] {tempRow, -tempCol });
                }
                return range;
            case CharDir.D:
                return GameManager.instance.skillManager.currSkill.skillInfo.rangePos;
        }

        return GameManager.instance.skillManager.currSkill.skillInfo.rangePos;
    }

    public virtual void Skill()
    {
        SkillBtn btn = GetComponentInParent<SkillBtn>();
        btn.coolTime = skillInfo.coolTime;
        if(btn.coolTime > 0)
            btn.CoolDown = true;

        GameManager.instance.player.stack -= skillInfo.cost;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();

        List<int> indexs = new List<int>();
        foreach (Tile tile in GameManager.instance.mapController.tiles)
        {
            if (tile.inRange)
            {
                indexs.Add(GameManager.instance.mapController.ReturnIndex(tile.rowIndex, tile.colIndex));
            }
        }

        foreach (int index in indexs)
        {
            foreach (EnemyController enemy in GameManager.instance.enemys)
            {
                int enemyIndex = GameManager.instance.mapController.ReturnIndex(enemy.curRow, enemy.curCol);
                if (enemyIndex == index)
                {
                    finalDamage = skillInfo.damage;
                    SkillManager.instance.targetEnemy = enemy;
                    GameManager.instance.player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);
                    if (!enemy.isDead)
                        enemy.TakeDamage(finalDamage, GameManager.instance.player);
                }
            }
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;
    }

    public virtual void Init()
    {
        LoadInfo(skillInfo.index);

        if (GameData.playerSkills != null)
        {
            foreach (int index in GameData.playerSkills)
            {
                if (skillInfo.index == index)
                    isLeran = true;
            }
        }
    }
}
