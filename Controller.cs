using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform charImg;

    public int curRow;
    public int curCol;

    public float curHp;
    public float maxHp;

    protected bool isMoved = false;

    public Coroutine move;

    public CharDir curDir = CharDir.D;
    public TokenSpace tokenSpace;

    IEnumerator ColorChange(Color color, float time)
    {
        float curTime = 0;
        float r = 1;
        float g = 1;
        float b = 1;
        SpriteRenderer[] mySprites = GetComponentsInChildren<SpriteRenderer>();
        while(curTime != time)
        {
            curTime += Time.deltaTime;
            if (curTime > time)
                curTime = time;
            r = Mathf.Lerp(r, color.r, curTime / (time / 2));
            g = Mathf.Lerp(g, color.g, curTime / (time / 2));
            b = Mathf.Lerp(b, color.b, curTime / (time / 2));
            foreach(SpriteRenderer sprite in mySprites)
                sprite.color = new Color(r, g, b);
            yield return null;
        }

        if (color != Color.white)
            StartCoroutine(ColorChange(Color.white, time));

        yield return null;
    }

    protected void DamageEffect(Color color, float time)
    {
        if (charImg != null)
            StartCoroutine(ColorChange(color, time));
    }

    public virtual void RotateChar(int rowIndex, int colIndex)
    {
        int dirRow = rowIndex - curRow;
        int dirCol = colIndex - curCol;
        SetDir(dirRow, dirCol);

        if (curDir == CharDir.W || curDir == CharDir.D)
            charImg.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            charImg.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public virtual void RemoveBuff(int id, int stack = 1)
    {
        if (tokenSpace == null)
            return;

        Buff[] buffs = tokenSpace.GetComponentsInChildren<Buff>(true);
        foreach (Buff buff in buffs)
        {
            if (buff.id == id)
            {
                buff.stack -= stack;
                if(buff.stack <= 0)
                {
                    Destroy(buff.gameObject);
                }
                else
                {
                    buff.SetStack();
                }
                return;
            }
        }
    }

    public virtual void TakeBuff(string buffPath, int id, int stack = 1)
    {
        Buff[] buffs = tokenSpace.GetComponentsInChildren<Buff>();
        foreach(Buff buff in buffs)
        {
            if(buff.id == id)
            {
                buff.stack += stack;
                buff.SetStack();
                return;
            }
        }

        Buff newBuff = Resources.Load<Buff>(buffPath);
        newBuff = Instantiate(newBuff, tokenSpace.transform);
        newBuff.Init(this, stack);
    }

    public virtual void TakeDamage(float damage, Controller attacker)
    {
        
    }

    public void SetDir(int dirRow, int dirCol)
    {
        if (dirRow == 0)
        {
            if (dirCol > 0)
            {
                curDir = CharDir.D;
            }
            else if (dirCol < 0)
            {
                curDir = CharDir.A;
            }
        }

        if (dirCol == 0)
        {
            if (dirRow > 0)
            {
                curDir = CharDir.W;
            }
            else if (dirRow < 0)
            {
                curDir = CharDir.S;
            }
        }

        if (dirCol == dirRow)
        {
            if (dirRow > 0)
            {
                curDir = CharDir.D;
            }
            if (dirRow < 0)
            {
                curDir = CharDir.A;
            }
        }

        if (dirCol == -dirRow)
        {
            if (dirRow > 0)
            {
                curDir = CharDir.W;
            }
            if (dirRow < 0)
            {
                curDir = CharDir.S;
            }
        }
    }

    public IEnumerator Move(Vector3 nextPos, float speed = 1f, bool directCall = false)
    {
        while (!isMoved)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * 2f * speed);

            if ((transform.position - nextPos).magnitude < 0.05f)
            {
                isMoved = true;
                move = null;
                break;
            }

            yield return null;
        }

        if (directCall)
            isMoved = false;
    }
    
    public virtual IEnumerator MoveCharacter()
    {
        yield return null;
        CameraController.instance.CameraFollow(transform);

        SendMessage("Run", SendMessageOptions.DontRequireReceiver);
        List<int> pathIndex = GameManager.instance.mapController.pathIndexs;
        // path만큼 이동
        for (int i = 0; i < pathIndex.Count; i++)
        {
            int nextRow = GameManager.instance.mapController.tiles[pathIndex[i]].rowIndex;
            int nextCol = GameManager.instance.mapController.tiles[pathIndex[i]].colIndex;

            int index = GameManager.instance.mapController.ReturnIndex(nextRow, nextCol);
            //Vector3 nextPos = GameManager.instance.uiManager.mapController.tileTransform[index].position;
            Vector3 nextPos = GameManager.instance.mapController.tiles[index].transform.position;
            while (true)
            {
                if (move != null)
                    StopCoroutine(move);
                move = StartCoroutine(Move(nextPos));
                RotateChar(nextRow, nextCol);

                if (isMoved)
                {
                    isMoved = false;
                    break;
                }
                yield return null;
            }
            curRow = nextRow;
            curCol = nextCol;
            isMoved = false;
            GameManager.instance.mapController.UpdateOnTargetTiles();
        }

        SendMessage("Run", SendMessageOptions.DontRequireReceiver);
        GameManager.instance.moveing = null;
    }

    public virtual void Init()
    {

    }
}
