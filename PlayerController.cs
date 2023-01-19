using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharDir
{
    W,
    S,
    A,
    D,
}

public class PlayerController : Controller
{
    public float stack = 0;

    public int gold = 0;
    public int skillPoint = 0;
    public int permanentSkillPoint = 0;

    public PlayerStat playerStat;
    public PlayerStatBar statBar;
    public List<int> learnedSkills;
    public ItemBag bag;

    LayerMask layer;

    private Animator animator;

    public override void RotateChar(int rowIndex, int colIndex)
    {
        int dirRow = rowIndex - curRow;
        int dirCol = colIndex - curCol;
        SetDir(dirRow, dirCol);

        if (curDir == CharDir.W || curDir == CharDir.D)
            charImg.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            charImg.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void Run()
    {
        if (animator != null)
            animator.SetTrigger("Move");
    }

    public void EndAttack()
    {
        if (animator != null)
            animator.SetTrigger("EndAttack");
    }

    public void SetTrigger(string trigger)
    {
        if (animator != null)
            animator.SetTrigger(trigger);
    }

    public void Buff()
    {
        if (animator != null)
            animator.SetTrigger("Buff");
    }

    public void Slash()
    {
        if (animator != null)
            animator.SetTrigger("Slash");
    }

    public void Stab()
    {
        if (animator != null)
            animator.SetTrigger("Stab");
    }

    private void Die()
    {
        animator.SetTrigger("Die");
    }

    private void DamagedReturn()
    {
        animator.SetTrigger("HurtExit");
    }

    public void Damaged(float time = 0.2f)
    {
        if(animator != null)
        {
            CancelInvoke();
            animator.SetTrigger("Hurt");
            Invoke("DamagedReturn", time);
        }
    }

    public void ReturnToIdle()
    {
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }

    public override void TakeDamage(float damage, Controller attacker)
    {
        if (damage > 0)
        {
            Thorne thorne = tokenSpace.GetComponentInChildren<Thorne>();
            if(thorne != null && attacker != null)
            {
                attacker.TakeDamage(thorne.stack, null);
            }
            
            if (playerStat.hpBar != null)
            {
                playerStat.hpBar.UpdateHealthBar(curHp, curHp - damage, maxHp);
                AudioClip clip = Resources.Load<AudioClip>("Sounds/impact");
                AudioManager.Instance.PlayEffect(clip);
            }
            
            curHp -= damage;
            if (curHp <= 0)
                Die();
            else
                Damaged();
        }
        DamageText.instance.Damage(transform, damage);
    }

    public void CheckMouseInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerController player = hit.transform.GetComponentInParent<PlayerController>();
                if (player != null)
                {
                    GameManager.instance.uiManager.mapController.gameObject.SetActive(true);
                }
            }
        }
    }

    public override void Init()
    {
        curHp = GameData.playerCurHp;
        maxHp = GameData.playerMaxHp;
        gold = GameData.playerGold;
        skillPoint = GameData.playerSkillPoint;
        permanentSkillPoint = GameData.playerPermanentPoint;

        layer = LayerMask.NameToLayer("Player");
        statBar = FindObjectOfType<PlayerStatBar>();
        if(statBar != null)
            statBar.Init();
        learnedSkills = new List<int>();

        playerStat = FindObjectOfType<PlayerStat>();
        if(playerStat != null)
        {
            playerStat.Init(this);
        }

        charImg = transform.Find("PlayerImg");

        curRow = 0;
        curCol = 0;

        animator = GetComponentInChildren<Animator>();

        if (GameManager.instance != null)
        {
            tokenSpace = GameManager.instance.uiManager.transform.Find("PlayerTokenSpace").GetComponentInChildren<TokenSpace>();
            tokenSpace.Target = this;
        }

        bag = FindObjectOfType<ItemBag>();
        if (bag != null)
            bag.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
