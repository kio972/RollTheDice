using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMan : EnemyController
{
    // 전체 맵에 돌이 있는지 확인
    // 없다면 전체 맵의 랜덤위치 세군데에 돌 낙하, 플레이어가 맞으면 데미지, 안맞으면 해당위치에 돌 생성
    // 맵에 돌이 하나라도 있다면 한턴 기모으기(제자리에서 한턴 쉬며 기모음)
    // 플레이어와 직선방향으로 정렬되어있는지 확인,(+혹은 x방향) 아니라면 가장가까운 직선정렬방향으로 이동
    // 플레이어 방향으로 돌 투척, 경로에 돌이 없다면 플레이어 데미지 입고 한칸 밀려남, 플레이어 원래위치에 돌 생성
    // 경로에 돌이 있다면 해당 타일 돌부딪히는 모션, 돌 파괴
    // 이동양식은 플레이어에게서 멀어질수 있다면 도망, 이미 플레이어와 거리가 최대로 멀다면 이동 쉼
    private List<Tile> stoneTile;
    GameObject stone;

    private int charge = 0;

    Vector3 cameraPos;



    /*GoNextCheck
    while (true)
    {
        if (goNext == true)
        {
            goNext = false; break;
        }
        yield return null;
    }
    */


    private void RockSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/rockImpact");
        AudioManager.Instance.PlayEffect(clip);
    }

    private void ThrowStoneAni()
    {
        if (animator != null)
            animator.SetTrigger("Throw");
    }

    private void SpawnStoneAni()
    {
        if (animator != null)
            animator.SetTrigger("Spawn");
    }

    private void ChargeAni()
    {
        if (animator != null)
            animator.SetTrigger("Charge");
    }

    IEnumerator Charge()
    {
        CameraController.instance.SetCameraPos(transform.position);
        float cameraWait = 0;
        while(cameraWait < 1)
        {
            cameraWait += Time.deltaTime;
            yield return null;
        }

        ChargeAni();
        while (true)
        {
            if (goNext == true)
            {
                goNext = false;
                break;
            }
            yield return null;
        }
        TakeBuff("Prefab/Buff/ReadyAttack", 4);
        yield return null;
        //motion = null;
        Invoke("EndEnemyTrun", 1f);
    }

    private void SpawnStone(int index)
    {
        GameObject spawnstone = Instantiate(stone, GameManager.instance.mapController.tiles[index].transform);
        spawnstone.name = "Stone";
        GameManager.instance.mapController.tiles[index].onTarget = OnTile.Object; // 해당위치 돌생성
        GameManager.instance.mapController.tiles[index].canMove = false;
        GameManager.instance.objects.Add(index);
    }
    private void BreakStone(int index)
    {
        RockSound();
        GameManager.instance.mapController.tiles[index].onTarget = OnTile.None;
        GameManager.instance.objects.Remove(index);
        Transform targetstone = GameManager.instance.mapController.tiles[index].transform.Find("Stone");
        Animator animator = targetstone.GetComponentInChildren<Animator>();
        animator.SetTrigger("Break");
        GameManager.instance.mapController.tiles[index].canMove = true;
        Destroy(targetstone.gameObject, 0.5f);
    }

    private int GetImpactPoint()
    {
        //몬스터 ~ 플레이어까지의 경로의 타일 인덱스를 구하고, 인덱스중 빈타일이 아닐경우 임팩트인덱스로 리턴
        int playerRow = GameManager.instance.player.curRow;
        int playerCol = GameManager.instance.player.curCol;
        List<int> attackIndex = new List<int>();
        if(curRow == playerRow)
        {
            if (curCol > playerCol)
            {
                int i = 1;
                while(curCol - i != playerCol)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow, curCol - i));
                    i++;
                }
            }
            else
            {
                int i = 1;
                while (curCol + i != playerCol)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow, curCol + i));
                    i++;
                }
            }
        }
        else if(curCol == playerCol)
        {
            if(curRow > playerRow)
            {
                int i = 1;
                while(curRow - i != playerRow)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow - i, curCol));
                    i++;
                }
            }
            else
            {
                int i = 1;
                while (curRow + i != playerRow)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol));
                    i++;
                }
            }
        }
        else if (curRow - playerRow == curCol - playerCol)
        {
            if(curRow > playerRow)
            {
                int i = 1;
                while (curCol - i != playerCol)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow -i, curCol - i));
                    i++;
                }
            }
            else
            {
                int i = 1;
                while (curCol + i != playerCol)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol + i));
                    i++;
                }
            }
        }
        else
        {
            if (curRow > playerRow)
            {
                int i = 1;
                while (curRow - i != playerRow)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow - i, curCol + i));
                    i++;
                }
            }
            else
            {
                int i = 1;
                while (curRow + i != playerRow)
                {
                    attackIndex.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol - i));
                    i++;
                }
            }
        }
        attackIndex.Add(GameManager.instance.mapController.ReturnIndex(playerRow, playerCol));

        int impactIndex = 0;
        foreach (int index in attackIndex)
        {
            if(GameManager.instance.mapController.tiles[index].onTarget != OnTile.None)
            {
                impactIndex = index;
                break;
            }
        }

        return impactIndex;
    }

    IEnumerator ThrowStoneMotion(int impactIndex)
    {
        ThrowStoneAni();
        while (true)
        {
            if (goNext == true)
            {
                goNext = false;
                break;
            }
            yield return null;
        }

        GameObject temp = new GameObject("Stone");
        GameObject throwStone = Instantiate(stone, temp.transform);
        temp.transform.position = transform.position;
        Vector3 impactPos = GameManager.instance.mapController.tiles[impactIndex].transform.position;
        while (true)
        {
            temp.transform.Translate((impactPos - temp.transform.position).normalized * Time.deltaTime * 10);
            if ((impactPos - temp.transform.position).magnitude <= 0.5f)
            {
                Animator animator = throwStone.GetComponentInChildren<Animator>();
                animator.SetTrigger("Break");
                Destroy(temp.gameObject, 0.5f);
                break;
            }
            yield return null;
        }
        yield return null;
        motion = null;
    }

    IEnumerator ThrowStone()
    {
        CameraController.instance.SetCameraPos(cameraPos);
        float cameraWait = 0;
        while (cameraWait < 1)
        {
            cameraWait += Time.deltaTime;
            yield return null;
        }

        int playerRow = GameManager.instance.player.curRow;
        int playerCol = GameManager.instance.player.curCol;
        // 플레이어와 직선이 되는 위치로 우선 이동해야함
        // 이동 가능한 타일을 모두 구하고, 그중 가장 가까운 타일로 이동하게
        List<int[]> invailPos = new List<int[]>();
        foreach (Tile tile in GameManager.instance.mapController.tiles)
        {
            if(tile.rowIndex == playerRow)
            {
                int[] tilePos = new int[2] { tile.rowIndex, tile.colIndex };
                invailPos.Add(tilePos);
            }
            else if(tile.colIndex == playerCol)
            {
                int[] tilePos = new int[2] { tile.rowIndex, tile.colIndex };
                invailPos.Add(tilePos);
            }
            else if(tile.rowIndex - playerRow == tile.colIndex - playerCol)
            {
                int[] tilePos = new int[2] { tile.rowIndex, tile.colIndex };
                invailPos.Add(tilePos);
            }
            else if(tile.rowIndex - playerRow == playerCol - tile.colIndex)
            {
                int[] tilePos = new int[2] { tile.rowIndex, tile.colIndex };
                invailPos.Add(tilePos);
            }
        }

        int[] destPos = new int[2];
        int minIndex = 0;
        float minDist = 100;
        for(int i = 0; i < invailPos.Count; i++)
        {
            float dist = 0;
            int rowDist = Mathf.Abs(curRow - invailPos[i][0]);
            int colDist = Mathf.Abs(curCol - invailPos[i][1]);
            if (rowDist == colDist)
                dist = rowDist * 1.5f;
            else
            {
                if (rowDist > colDist)
                    dist += Mathf.Abs(colDist * 1.5f);
                else
                    dist += Mathf.Abs(rowDist * 1.5f);
                dist += Mathf.Abs(rowDist - colDist);
            }

            int curIndex = GameManager.instance.mapController.ReturnIndex(invailPos[i][0], invailPos[i][1]);
            if (dist < minDist && GameManager.instance.mapController.tiles[curIndex].canMove || dist == 0)
            {
                minDist = dist;
                minIndex = i;
            }
        }
        destPos = invailPos[minIndex];
        int destPosIndex = GameManager.instance.mapController.ReturnIndex(destPos[0], destPos[1]);
        int startPosIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
        float cost = GameManager.instance.mapController.CheckDist(destPos[0], destPos[1], curRow, curCol);
        if(cost != 0)
        {
            GameManager.instance.mapController.FindPath(startPosIndex, destPosIndex);
            GameManager.instance.moveing = StartCoroutine(MoveCharacter());
            if (GameManager.instance.moveing != null)
                MoveAni();
            while (true)
            {
                if (GameManager.instance.moveing == null)
                {
                    ReturnToIdle();
                    break;
                }
                yield return null;
            }
        }

        CameraController.instance.SetCameraPos(cameraPos);
        cameraWait = 0;
        while (cameraWait < 1)
        {
            cameraWait += Time.deltaTime;
            yield return null;
        }

        int impactIndex = GetImpactPoint();
        RotateChar(GameManager.instance.mapController.tiles[impactIndex].rowIndex, GameManager.instance.mapController.tiles[impactIndex].colIndex);
        motion = StartCoroutine(ThrowStoneMotion(impactIndex));

        RemoveBuff(4);

        while (motion != null)
        {
            yield return null;
        }

        switch(GameManager.instance.mapController.tiles[impactIndex].onTarget)
        {
            case OnTile.Object:
                // 돌부딪히며 깨지는 모션 출력
                BreakStone(impactIndex);
                break;
            case OnTile.Player:
                int[] vec = new int[2];
                vec = GameManager.instance.mapController.GetDirVec(curRow, curCol, playerRow, playerCol);
                int moveRow = GameManager.instance.player.curRow + vec[0];
                int moveCol = GameManager.instance.player.curCol + vec[1];
                int moveIndex = GameManager.instance.mapController.ReturnIndex(moveRow, moveCol);
                bool canKnockback = true;

                if (moveRow > GameManager.instance.mapController.mapRowsize-1 || moveRow < 0)
                    canKnockback = false;
                if (moveCol > GameManager.instance.mapController.mapColsize-1 || moveCol < 0)
                    canKnockback = false;
                if (canKnockback && !GameManager.instance.mapController.tiles[moveIndex].canMove)
                    canKnockback = false;

                if (canKnockback)
                {
                    GameManager.instance.player.TakeDamage(20, this);
                    RockSound();
                    StartCoroutine(GameManager.instance.player.Move(GameManager.instance.mapController.tiles[moveIndex].transform.position, 2f, true));
                    //StartCoroutine(GameManager.instance.player.Move(GameManager.instance.uiManager.mapController.tileTransform[moveIndex].transform.position, 2f, true));
                    int playerIndex = GameManager.instance.mapController.ReturnIndex(playerRow, playerCol);
                    GameManager.instance.player.curRow = moveRow;
                    GameManager.instance.player.curCol = moveCol;

                    SpawnStone(playerIndex);
                }
                else
                {
                    //넉백당할곳이 없으면 큰데미지, 플레이어로부터 몬스터방향1칸에 돌소환(몹위치면 소환안함)
                    GameManager.instance.player.TakeDamage(30, this);
                    RockSound();
                    int stoneRow = GameManager.instance.player.curRow - vec[0];
                    int stoneCol = GameManager.instance.player.curCol - vec[1];
                    int stoneIndex = GameManager.instance.mapController.ReturnIndex(stoneRow, stoneCol);
                    if (stoneIndex != GameManager.instance.mapController.ReturnIndex(curRow, curCol))
                        SpawnStone(stoneIndex);
                }
                break;
        }
        // 돌이 맞으면 돌깨짐
        // 플레이어나 적이 맞으면 데미지, 한칸밀려나며 돌떨구기

        Invoke("EndEnemyTrun", 1f);
    }

    IEnumerator SpawnStoneMotion(List<int> indexs)
    {
        SpawnStoneAni();
        while(true)
        {
            if (goNext == true)
            {
                goNext = false;
                break;
            }
            yield return null;
        }

        for(int i =0; i< indexs.Count; i++)
        {
            GameObject spawnstone = Instantiate(stone, GameManager.instance.mapController.tiles[indexs[i]].transform);
            spawnstone.name = "Stone";
            Animator animator = spawnstone.GetComponentInChildren<Animator>();
            animator.SetTrigger("Fall");
        }

        float elpased = 0;
        while(true)
        {
            elpased += Time.deltaTime;
            if (elpased >= 0.3f)
                break;

            yield return null;
        }
        motion = null;
    }

    IEnumerator SpawnStone()
    {
        CameraController.instance.SetCameraPos(cameraPos);
        float cameraWait = 0;
        while (cameraWait < 1)
        {
            cameraWait += Time.deltaTime;
            yield return null;
        }

        int[] randomRows = new int[3];
        int[] randomCols = new int[3];
        List<int> randomIndex = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomRow = Random.Range(1, GameManager.instance.mapController.mapRowsize -1);
            int randomCol = Random.Range(1, GameManager.instance.mapController.mapColsize -1);
            
            int index = GameManager.instance.mapController.ReturnIndex(randomRow, randomCol);
            // 랜덤인덱스에 인덱스값이 있다면 반복
            do
            {
                // 현재 몬스터의 위치가 아닌 좌표값을 구함
                do
                {
                    randomRow = Random.Range(1, GameManager.instance.mapController.mapRowsize - 1);
                    randomCol = Random.Range(1, GameManager.instance.mapController.mapColsize - 1);
                } while (randomRow == curRow && randomCol == curCol);
                index = GameManager.instance.mapController.ReturnIndex(randomRow, randomCol);
            } while (randomIndex.Contains(index));

            randomIndex.Add(index);
        }
        
        motion = StartCoroutine(SpawnStoneMotion(randomIndex));
        while(motion != null)
        {
            yield return null;
        }
        // 돌 이미지 몬스터에게 생성, 해당 타일에 떨어지는 모션
        // 이펙트가 끝날때 아래 출력
        for (int i = 0; i < randomIndex.Count; i++)
        {
            if (GameManager.instance.mapController.tiles[randomIndex[i]].onTarget == OnTile.None)
            {
                GameManager.instance.mapController.tiles[randomIndex[i]].onTarget = OnTile.Object;
                GameManager.instance.mapController.tiles[randomIndex[i]].canMove = false;
                GameManager.instance.objects.Add(randomIndex[i]);
                
            }
            else if (GameManager.instance.mapController.tiles[randomIndex[i]].onTarget == OnTile.Player)
            {
                BreakStone(randomIndex[i]);
                //플레이어에게 데미지 입히고, 그냥 바위 즉시깨짐
                GameManager.instance.player.TakeDamage(10, this);
            }
            else if (GameManager.instance.mapController.tiles[randomIndex[i]].onTarget == OnTile.Object)
            {
                BreakStone(randomIndex[i]);
                // 바위 부딪히며 깨지는 모션
            }
        }
        
        Invoke("EndEnemyTrun", 1f);
    }

    private void CheckStone()
    {
        
    }

    public override void Behavior()
    {
        if(GameManager.instance.turn == 1)
        {
            StartCoroutine(SpawnStone());
        }
        else
        {
            // 맵에 오브젝트있는지 확인하고 없으면 spawnstone
            bool isStone = false;
            foreach (Tile tile in GameManager.instance.mapController.tiles)
            {
                if (tile.onTarget == OnTile.Object)
                    isStone = true;
            }

            if (isStone)
            {
                if(charge == 0)
                {
                    StartCoroutine(Charge());
                    charge++;
                }
                else
                {
                    StartCoroutine(ThrowStone());
                    charge = 0;
                }
            }
            else
                StartCoroutine(SpawnStone());
        }
    }

    public override void Init()
    {
        stone = Resources.Load<GameObject>("Prefab/Object/Stone");
        enemyName = "스톤맨";
        enemyDesc = "바위를 던져 공격하는 골렘. \n녀석의 바위를 피할수 있는 방법은 없을까?";
        stoneTile = new List<Tile>();

        curHp = 120;
        maxHp = 120;
        isBoss = true;
        base.Init();

        cameraPos = Camera.main.transform.position;
    }

}
