using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseType
{
    Normal,
    Interact,
    Enemy,
    Boss,
    Attack,
    Buy,
    NoPath,
    Move,
}

public class MousePointer : MonoBehaviour
{
    public static MousePointer instance;
    
    private Transform pointerGroup;
    public Vector3 PointerLocation
    {
        get { return pointerGroup.transform.position; }
    }

    private Transform normal;
    private Transform interact;
    private Transform enemy;
    private Transform boss;
    private Transform attack;
    private Transform buy;
    private Transform noPath;
    private Transform move;

    public MouseType mouseType;
    public MouseType curMouse;

    private bool needChange;

    protected virtual void PointerOverCheck()
    {
        if(GameManager.instance != null)
        {
            if (GameManager.instance.battleEnd || GameManager.instance.phase == 0)
            {
                if (curMouse == MouseType.Boss || curMouse == MouseType.Attack || curMouse == MouseType.Enemy)
                {
                    mouseType = MouseType.Normal;
                }
                return;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(!needChange)
            {
                AudioClip clip = Resources.Load<AudioClip>("Sounds/mouseOver");
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    AudioManager.Instance.PlayEffect(clip, 0.2f);
                    needChange = true;
                    EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();
                    if (GameManager.instance != null && GameManager.instance.skillManager.currSkill == null)
                    {
                        if (enemy.isBoss)
                            mouseType = MouseType.Boss;
                        else
                            mouseType = MouseType.Enemy;
                    }
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interact"))
                {
                    AudioManager.Instance.PlayEffect(clip);
                    needChange = true;
                    mouseType = MouseType.Interact;
                }
            }
        }
        else if (needChange)
        {
            needChange = false;

            if (GameManager.instance != null && GameManager.instance.skillManager.currSkill != null)
                return;

            mouseType = MouseType.Normal;
        }
    }


    private void SetActiveMouse(MouseType type, bool isTrue)
    {
        switch (type)
        {
            case MouseType.Normal:
                normal.gameObject.SetActive(isTrue);
                break;
            case MouseType.Interact:
                interact.gameObject.SetActive(isTrue);
                break;
            case MouseType.Enemy:
                enemy.gameObject.SetActive(isTrue);
                break;
            case MouseType.Boss:
                boss.gameObject.SetActive(isTrue);
                break;
            case MouseType.Attack:
                attack.gameObject.SetActive(isTrue);
                break;
            case MouseType.Buy:
                buy.gameObject.SetActive(isTrue);
                break;
            case MouseType.Move:
                move.gameObject.SetActive(isTrue);
                break;
            case MouseType.NoPath:
                noPath.gameObject.SetActive(isTrue);
                break;
        }
        Cursor.visible = false;
    }

    private void UpdateMouse()
    {
        pointerGroup.transform.position = Input.mousePosition;

        if(mouseType != curMouse)
        {
            SetActiveMouse(curMouse, false);
            SetActiveMouse(mouseType, true);
            curMouse = mouseType;
        }
    }

    private void Awake()
    {
        MousePointer[] pointers = FindObjectsOfType<MousePointer>();

        if (pointers.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Transform[] temp = GetComponentsInChildren<Transform>();
        foreach(Transform t in temp)
            t.gameObject.SetActive(true);

        pointerGroup = transform.Find("PointerGroup");

        normal = pointerGroup.transform.Find("Normal");

        interact = pointerGroup.transform.Find("Interact");
        interact.gameObject.SetActive(false);

        enemy = pointerGroup.transform.Find("Enemy");
        enemy.gameObject.SetActive(false);

        boss = pointerGroup.transform.Find("Boss");
        boss.gameObject.SetActive(false);

        attack = pointerGroup.transform.Find("Attack");
        attack.gameObject.SetActive(false);

        buy = pointerGroup.transform.Find("Buy");
        buy.gameObject.SetActive(false);

        noPath = pointerGroup.transform.Find("NoPath");
        noPath.gameObject.SetActive(false);

        move = pointerGroup.transform.Find("Move");
        move.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouse();

        if (Pause.instance != null && Pause.instance.pause)
            return;

        PointerOverCheck();
    }
}
