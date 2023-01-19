using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : Controller
{
    protected float maxAggroRange = 10;

    protected Animator animator;

    public string enemyName;
    public string enemyDesc;

    protected float baseDamage;

    protected Coroutine motion;

    LayerMask layer;
    protected HPUpdater hpBar;
    protected Transform enemyTokenSpace;

    protected bool mouseOver;

    public bool isBoss;
    public bool isDead;

    protected bool goNext = false;

    protected void GoNext()
    {
        goNext = true;
    }

    private void OnDisable()
    {
        if (tokenSpace != null)
            tokenSpace.gameObject.SetActive(false);
        if (hpBar != null && !isBoss)
            hpBar.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (tokenSpace != null)
            tokenSpace.gameObject.SetActive(true);
        if (hpBar != null && !isBoss)
            hpBar.gameObject.SetActive(true);
    }

    public override void RotateChar(int rowIndex, int colIndex)
    {
        int dirRow = rowIndex - curRow;
        int dirCol = colIndex - curCol;
        SetDir(dirRow, dirCol);

        if (curDir == CharDir.W || curDir == CharDir.D)
            charImg.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else
            charImg.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    protected virtual void PointerOver()
    {
        if (GameManager.instance.battleEnd || GameManager.instance.phase == 0)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.tag == "Enemy")
            {
                mouseOver = true;
                EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();
                
                if (DescManager.instance != null)
                    DescManager.instance.DrawEnemy(enemy);
            }
        }
        else if(mouseOver)
        {
            mouseOver = false;

            if (DescManager.instance != null)
            {
                if (GameManager.instance.skillManager.currSkill != null)
                {
                    DescManager.instance.DrawSkill(GameManager.instance.skillManager.currSkill.skillInfo);
                }
                else
                    DescManager.instance.DescOff();
            }
        }
    }

    protected void MoveAni()
    {
        if (animator != null)
            animator.SetTrigger("Move");
    }

    protected void ReturnToIdle()
    {
        if (animator != null)
            animator.SetTrigger("Idle");
    }

    protected void AttackAni()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    protected void DieAni()
    {
        if (animator != null)
            animator.SetTrigger("Die");
    }

    protected void HurtAni()
    {
        if (animator != null)
            animator.SetTrigger("Hurt");
    }

    protected bool DieCheck()
    {
        if (curHp <= 0)
        {
            curHp = 0;
            isDead = true;
            return true;
        }
        return false;
    }
    
    
    protected HPUpdater InstanceHealthBar()
    {
        if (isBoss)
        {
            Transform bossStat = GameManager.instance.uiManager.transform.Find("BossStat");
            if (bossStat == null)
            {
                bossStat = Resources.Load<Transform>("Prefab/UI/BossStat");
                bossStat = Instantiate(bossStat, GameManager.instance.uiManager.transform);
                bossStat.gameObject.name = "BossStat";
            }

            Transform helathBarZone = bossStat.transform.Find("HealthBar");
            HPUpdater hp = Resources.Load<HPUpdater>("Prefab/UI/HPBar");

            Vector3 pos = new Vector3(-300, 55, 0);
            hp = Instantiate(hp, helathBarZone);
            hp.transform.position = helathBarZone.position + pos;
            hp.gameObject.name = enemyName;
            return hp;
        }
        else
        {
            Transform enemyStat = GameManager.instance.uiManager.transform.Find("EnemyStat");
            HPUpdater hp = Resources.Load<HPUpdater>("Prefab/UI/MiniHpBar");
            hp = Instantiate(hp, enemyStat);
            hp.gameObject.name = enemyName;
            return hp;
        }
    }

    public override void TakeBuff(string buffPath, int index, int stack = 1)
    {
        if (tokenSpace == null)
        {
            TokenSpace token = Resources.Load<TokenSpace>("Prefab/UI/Token");
            tokenSpace = Instantiate(token, enemyTokenSpace);
            tokenSpace.name = name;
            tokenSpace.Target = this;
        }

        base.TakeBuff(buffPath, index, stack);
    }

    public override void TakeDamage(float damage, Controller attacker)
    {
        if (damage > 0)
        {
            if(tokenSpace != null)
            {
                Thorne thorne = tokenSpace.GetComponentInChildren<Thorne>();
                if (thorne != null && attacker != null)
                {
                    attacker.TakeDamage(thorne.stack, null);
                }
            }

            if (hpBar != null)
            {
                hpBar.UpdateHealthBar(curHp, curHp - damage, maxHp);
                AudioClip clip = Resources.Load<AudioClip>("Sounds/impact");
                AudioManager.Instance.PlayEffect(clip);
            }
                curHp -= damage;

            //DamageEffect(Color.red, 0.5f);
            if (DieCheck())
                DieAni();
            else
                HurtAni();
        }
        DamageText.instance.Damage(transform, damage);
    }

    protected void EndEnemyTrun()
    {
        GameManager.instance.enemyAttack = false;
    }

    protected bool RangeAttack(List<int> targetIndexs, float damage, string effectAd = null)
    {
        bool ishit = false;
        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        if (targetIndexs.Contains(playerIndex))
        {
            ishit = true;
            GameManager.instance.player.TakeDamage(damage, this);
        }

        if (effectAd != null)
        {
            foreach (int targetIndex in targetIndexs)
            {
                FxEffectManager.Instance.PlayEffect(effectAd, GameManager.instance.mapController.tiles[targetIndex].transform.position);
            }
        }

        return ishit;
    }

    protected List<int> BasicRange(int curRow, int curCol, int range)
    {
        List<int> targetIndexs = new List<int>();
        for (int i = -range; i < range + 1; i++) // row 담당
        {
            for (int j = -range; j < range + 1; j++) // col 담당
            {
                if (GameManager.instance.mapController.TileAvailCheck(curRow + i, curCol + j))
                    targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol + j));
            }
        }
        targetIndexs.Remove(GameManager.instance.mapController.ReturnIndex(curRow, curCol));

        return targetIndexs;
    }

    protected List<int> DiagCrossRange(int curRow, int curCol, int range)
    {
        List<int> targetIndexs = new List<int>();
        for (int i = 1; i < range + 1; i++)
        {
            if (GameManager.instance.mapController.TileAvailCheck(curRow + i, curCol + i))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol + i));
            if (GameManager.instance.mapController.TileAvailCheck(curRow + i, curCol - i))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol - i));
            if (GameManager.instance.mapController.TileAvailCheck(curRow - i, curCol + i))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow - i, curCol + i));
            if (GameManager.instance.mapController.TileAvailCheck(curRow - i, curCol - i))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow - i, curCol - i));
        }
        return targetIndexs;
    }

    protected List<int> CrossRange(int curRow, int curCol, int range)
    {
        List<int> targetIndexs = new List<int>();
        for (int i = 1; i < range + 1; i++)
        {
            if (GameManager.instance.mapController.TileAvailCheck(curRow + i, curCol))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol));
            if (GameManager.instance.mapController.TileAvailCheck(curRow - i, curCol))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow - i, curCol));
            if (GameManager.instance.mapController.TileAvailCheck(curRow, curCol + i))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow, curCol + i));
            if (GameManager.instance.mapController.TileAvailCheck(curRow, curCol - i))
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(curRow, curCol - i));
        }
        return targetIndexs;
    }

    public virtual void Behavior()
    {

    }

    public override void Init()
    {
        layer = LayerMask.NameToLayer("Enemy");
        enemyTokenSpace = GameManager.instance.uiManager.transform.Find("EnemyTokenSpace");
        hpBar = InstanceHealthBar();
        if (hpBar != null)
            hpBar.Init(this);
        animator = GetComponentInChildren<Animator>();
    }

    protected void Update()
    {
        if(GameManager.instance.phase == 2 || GameManager.instance.phase == 3)
        {
            PointerOver();
        }

        if(!isBoss && hpBar != null)
        {
            hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

}
