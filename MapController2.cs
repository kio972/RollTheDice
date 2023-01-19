using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapController2 : MonoBehaviour
{
    private int rowSize;
    private int colSize;
    public List<Tile> tiles;

    private List<int[]> path;
    
    public List<int> pathIndexs;
    public float pathCost;

    public Tile curTile;

    public bool isinit = false;

    public int mapRowsize
    {
        get { return rowSize; }
    }
    public int mapColsize
    {
        get { return colSize; }
    }

    TextMeshProUGUI costText;

    public bool TileAvailCheck(int row, int col)
    {
        if (row < 0 || row > mapRowsize -1)
            return false;
        if (col < 0 || col > mapColsize -1)
            return false;

        return true;
    }

    public void ResetGuide()
    {
        foreach (Tile tile in tiles)
        {
            tile.ResetGuide();
        }
        costText.gameObject.SetActive(false);
    }

    private void DirCheck(int[] vec, int tileIndex, bool haveCost)
    {
        if (tiles[tileIndex].guides == null || tiles[tileIndex].guides.Length == 0)
            return;

        SpriteRenderer guide = null;
        if(vec[0] == -1)
        {
            if(vec[1] == -1)
                guide = tiles[tileIndex].guides[0];
            else if(vec[1] == 0)
                guide = tiles[tileIndex].guides[5];
            else if(vec[1] == 1)
                guide = tiles[tileIndex].guides[3];
        }
        else if(vec[0] == 0)
        {
            if (vec[1] == -1)
                guide = tiles[tileIndex].guides[4];
            else if (vec[1] == 1)
                guide = tiles[tileIndex].guides[6];
        }
        else if(vec[0] == 1)
        {
            if (vec[1] == -1)
                guide = tiles[tileIndex].guides[2];
            else if (vec[1] == 0)
                guide = tiles[tileIndex].guides[7];
            else if (vec[1] == 1)
                guide = tiles[tileIndex].guides[1];
        }
        
        if(guide != null)
        {
            guide.gameObject.SetActive(true);
            if (haveCost)
                guide.color = Color.white;
            else
                guide.color = Color.red;
        }
    }

    public void SetGuide(bool haveCost = true)
    {
        if(pathIndexs != null)
        {
            for(int i = 0; i < pathIndexs.Count; i++)
            {
                int curRow = tiles[pathIndexs[i]].rowIndex;
                int curCol = tiles[pathIndexs[i]].colIndex;
                if (i != 0)
                {
                    int prevRow = tiles[pathIndexs[i - 1]].rowIndex;
                    int prevCol = tiles[pathIndexs[i - 1]].colIndex;
                    DirCheck(GetDirVec(curRow, curCol, prevRow, prevCol), pathIndexs[i], haveCost);
                }
                else
                {
                    int prevRow = GameManager.instance.player.curRow;
                    int prevCol = GameManager.instance.player.curCol;
                    int playerIndex = ReturnIndex(prevRow, prevCol);
                    DirCheck(GetDirVec(curRow, curCol, prevRow, prevCol), pathIndexs[i], haveCost);
                    DirCheck(GetDirVec(prevRow, prevCol, curRow, curCol), playerIndex, haveCost);
                }
                
                if (i != pathIndexs.Count - 1)
                {
                    int nextRow = tiles[pathIndexs[i + 1]].rowIndex;
                    int nextCol = tiles[pathIndexs[i + 1]].colIndex;
                    DirCheck(GetDirVec(curRow, curCol, nextRow, nextCol), pathIndexs[i], haveCost);
                }
                else
                {
                    Vector2 rec = Camera.main.WorldToScreenPoint(tiles[pathIndexs[i]].transform.position);
                    costText.gameObject.SetActive(true);
                    costText.transform.position = rec;
                    costText.text = "Cost : " + pathCost.ToString();
                    if(haveCost)
                        costText.color = Color.yellow; 
                    else
                        costText.color = Color.red;
                }
            }
        }
    }

    public void UpdateTiles()
    {
        UpdateOnTargetTiles();
        foreach (Tile tile in tiles)
        {
            tile.gameObject.SetActive(true);

            if(!tile.canMove && tile.onTarget != OnTile.Player)
                tile.gameObject.SetActive(false);
        }
    }


    public void SpawnEnemy(string enemyPrefab, int index)
    {
        EnemyController enemy = Resources.Load<EnemyController>(enemyPrefab);
        enemy = Instantiate(enemy, GameManager.instance.enemySpace);
        enemy.transform.position = tiles[index].transform.position;
        enemy.curRow = tiles[index].rowIndex;
        enemy.curCol = tiles[index].colIndex;
        tiles[index].onTarget = OnTile.Enemy;
        tiles[index].canMove = false;
        enemy.Init();
    }

    public Controller FindIndexTarget(int index)
    {
        switch (tiles[index].onTarget)
        {
            case OnTile.Player:
                Controller target = GameManager.instance.player;
                return target;
            case OnTile.Enemy:
                foreach (EnemyController enemy in GameManager.instance.enemys)
                {
                    int targetIndex = ReturnIndex(enemy.curRow, enemy.curCol);
                    if (targetIndex == index)
                    {
                        return enemy;
                    }
                }
                break;
        }
        return null;
    }

    // 추정거리 = 직선거리
    public float GetHCost(int curNodeIndex, int destNodeIndex)
    {
        int curNodeRow = tiles[curNodeIndex].rowIndex;
        int curNodeCol = tiles[curNodeIndex].colIndex;
        int destNodeRow = tiles[destNodeIndex].rowIndex;
        int destNodeCol = tiles[destNodeIndex].colIndex;
        int rowDist = destNodeRow - curNodeRow;
        int colDist = destNodeCol - curNodeCol;
        float dist = 0;

        while (rowDist != 0 || colDist != 0)
        {
            if (rowDist < 0)
            {
                if (colDist < 0)
                {
                    rowDist -= -1;
                    colDist -= -1;
                    dist += 1.5f;
                }
                else if (colDist > 0)
                {
                    rowDist -= -1;
                    colDist -= 1;
                    dist += 1.5f;
                }
                else
                {
                    for (int i = rowDist; i != 0; i++)
                    {
                        rowDist -= -1;
                        dist++;
                    }
                }

            }
            else if (rowDist > 0)
            {
                if (colDist < 0)
                {
                    rowDist -= 1;
                    colDist -= -1;
                    dist += 1.5f;
                }
                else if (colDist > 0)
                {
                    rowDist -= 1;
                    colDist -= 1;
                    dist += 1.5f;
                }
                else
                {
                    for (int i = rowDist; i != 0; i--)
                    {
                        rowDist -= 1;
                        dist++;
                    }
                }
            }
            else
            {
                if (colDist < 0)
                {
                    colDist -= -1;
                    dist++;
                }
                else if (colDist > 0)
                {
                    colDist -= 1;
                    dist++;
                }
            }
        }

        return dist;
    }

    // 목표거리 -> 현재노드로부터 전노드거리를 전노드가 시작노드일때까지 더함
    public float GetGCost(int curNodeIndex, int startNodeIndex)
    {
        int curNodeRow = tiles[curNodeIndex].rowIndex;
        int curNodeCol = tiles[curNodeIndex].colIndex;
        int prevNodeRow = tiles[curNodeIndex].prevTile.rowIndex;
        int prevNodeCol = tiles[curNodeIndex].prevTile.colIndex;
        int rowDist = Mathf.Abs(prevNodeRow - curNodeRow);
        int colDist = Mathf.Abs(prevNodeCol - curNodeCol);
        float dist = 0;
        if (rowDist == 0)
            dist += colDist;
        else if (colDist == 0)
            dist += rowDist;
        else
            dist += 1.5f;
        int prevIndex = ReturnIndex(prevNodeRow, prevNodeCol);

        while(prevIndex != startNodeIndex)
        {
            curNodeRow = prevNodeRow;
            curNodeCol = prevNodeCol;
            int index = ReturnIndex(curNodeRow, prevNodeCol);
            prevNodeRow = tiles[index].prevTile.rowIndex;
            prevNodeCol = tiles[index].prevTile.colIndex;
            rowDist = Mathf.Abs(prevNodeRow - curNodeRow);
            colDist = Mathf.Abs(prevNodeCol - curNodeCol);
            if (rowDist == 0)
                dist += colDist;
            else if (colDist == 0)
                dist += rowDist;
            else
                dist += 1.5f;
            prevIndex = ReturnIndex(prevNodeRow, prevNodeCol);
        }

        return dist;
    }

    private struct Node
    {
        public int index;
        public float gCost;
        public float HCost;
        public float FCost;
    }

    private void ResetPrevTile()
    {
        foreach (Tile tile in tiles)
        {
            tile.prevTile = null;
        }
    }

    private int[][] GetNeighborNode(int curNodeIndex)
    {
        int[][] neighborNodes = new int[8][];
        neighborNodes[0] = new int[2] { tiles[curNodeIndex].rowIndex - 1, tiles[curNodeIndex].colIndex - 1 };
        neighborNodes[1] = new int[2] { tiles[curNodeIndex].rowIndex - 1, tiles[curNodeIndex].colIndex };
        neighborNodes[2] = new int[2] { tiles[curNodeIndex].rowIndex - 1, tiles[curNodeIndex].colIndex + 1 };
        neighborNodes[3] = new int[2] { tiles[curNodeIndex].rowIndex, tiles[curNodeIndex].colIndex - 1};
        neighborNodes[4] = new int[2] { tiles[curNodeIndex].rowIndex, tiles[curNodeIndex].colIndex + 1};
        neighborNodes[5] = new int[2] { tiles[curNodeIndex].rowIndex + 1, tiles[curNodeIndex].colIndex - 1 };
        neighborNodes[6] = new int[2] { tiles[curNodeIndex].rowIndex + 1, tiles[curNodeIndex].colIndex };
        neighborNodes[7] = new int[2] { tiles[curNodeIndex].rowIndex + 1, tiles[curNodeIndex].colIndex + 1};
        return neighborNodes;
    }

    public bool CheckIndexAvail(int rowIndex, int colIndex)
    {
        if (rowIndex < 0 || colIndex < 0)
            return false;
        if (rowIndex >= mapRowsize || colIndex >= mapColsize)
            return false;
        return true;
    }


    private bool CheckList(int index, List<Node> targtNodes)
    {
        foreach (Node node in targtNodes)
        {
            if (node.index == index)
                return true;
        }
        return false;
    }

    private int FindNodeIndex(int index, List<Node> targetNodes)
    {
        for(int i = 0; i < targetNodes.Count; i++)
        {
            if (targetNodes[i].index == index)
                return i;
        }
        return -1;
    }

    private Node NewNode(int index, int curNodeIndex, int startNodeIndex, int destNodeIndex)
    {
        Node newNode = new Node();
        newNode.index = index;
        tiles[index].prevTile = tiles[curNodeIndex];
        newNode.gCost = GetGCost(index, startNodeIndex);
        newNode.HCost = GetHCost(index, destNodeIndex);
        newNode.FCost = newNode.gCost + newNode.HCost;

        return newNode;
    }

    private void CheckNode(int curNodeIndex, int startNodeIndex, int destNodeIndex, List<Node> openNode, List<Node> closedNode, out List<Node> resultOpenNode)
    {
        int[][] neighborNodes = GetNeighborNode(curNodeIndex);
        for (int i = 0; i < neighborNodes.Length; i++)
        {
            if (!CheckIndexAvail(neighborNodes[i][0], neighborNodes[i][1]))
                continue;

            int index = ReturnIndex(neighborNodes[i][0], neighborNodes[i][1]);
            if (!tiles[index].canMove || index == startNodeIndex || CheckList(index, closedNode))
                continue;

            Tile tempPrevTile = tiles[index].prevTile;
            Node neighborNode = NewNode(index, curNodeIndex, startNodeIndex, destNodeIndex);
            if (CheckList(index, openNode))
            {
                // 이미 리스트에 있다면, 리스트의 현재 G코스트보다 크다면 기존 노드 제거
                if (openNode[FindNodeIndex(index, openNode)].gCost > neighborNode.gCost)
                    openNode.Remove(openNode[FindNodeIndex(index, openNode)]);
                else // 적다면 이전노드로 유지
                {
                    tiles[index].prevTile = tempPrevTile;
                    continue;
                }
            }
            openNode.Add(neighborNode);
        }
        resultOpenNode = openNode;
    }

    private int FindMinFCostNodeIndex(List<Node> openNode)
    {
        int minDistIndex = 0;
        float minFCost = openNode[minDistIndex].FCost;
        for (int i = 0; i < openNode.Count; i++)
        {
            if(openNode[i].FCost < minFCost)
            {
                minFCost = openNode[i].FCost;
                minDistIndex = i;
            }
        }

        return minDistIndex;
    }

    private List<int> FinalPath(int startNodeIndex, int destNodeIndex)
    {
        List<int> pathIndexs = new List<int>();
        Tile tempNode = tiles[destNodeIndex];
        int tempIndex = destNodeIndex;
        while (tempIndex != startNodeIndex)
        {
            if (tempNode.prevTile == null)
                return null;

            pathIndexs.Add(tempIndex);
            pathCost += GetHCost(tempIndex, ReturnIndex(tempNode.prevTile.rowIndex, tempNode.prevTile.colIndex));
            tempNode = tempNode.prevTile;
            tempIndex = ReturnIndex(tempNode.rowIndex, tempNode.colIndex);
        }
        pathIndexs.Reverse();
        return pathIndexs;
    }

    public float FindPath(int startNodeIndex, int destNodeIndex)
    {
        pathIndexs = new List<int>();
        pathCost = 0;
        ResetPrevTile();
        List<Node> openNode = new List<Node>();
        List<Node> closedNode = new List<Node>();
        int curNodeIndex = startNodeIndex;
        while(curNodeIndex != destNodeIndex)
        {
            CheckNode(curNodeIndex, startNodeIndex, destNodeIndex, openNode, closedNode, out openNode);
            if (openNode.Count == 0)
            {
                pathIndexs = null;
                return -1; // 길 없음
            }

            // 오픈노드들 중 가장 F코스트가 낮은 노드를 close노드에 추가하고, open노드에서 제거
            Node minFCostNode = openNode[FindMinFCostNodeIndex(openNode)];
            closedNode.Add(minFCostNode);
            openNode.Remove(minFCostNode);
            curNodeIndex = minFCostNode.index;
        }

        // 길찾기 종료, 도착지로부터 이전노드가 시작노드에 도착할때까지 기록
        pathIndexs = FinalPath(startNodeIndex, destNodeIndex);
        return pathCost;
    }

    public void UpdateOnTargetTiles()
    {
        int playerRow = GameManager.instance.player.curRow;
        int playerCol = GameManager.instance.player.curCol;
        foreach (Tile tile in tiles)
        {
            if (tile.onTarget == OnTile.Empty)
                continue;

            if(tile.rowIndex == playerRow && tile.colIndex == playerCol)
            {
                tile.onTarget = OnTile.Player;
                tile.canMove = false;
            }
            else
            {
                bool isEnemy = false;
                foreach(EnemyController enemy in GameManager.instance.enemys)
                {
                    if(tile.rowIndex == enemy.curRow && tile.colIndex == enemy.curCol)
                        isEnemy = true;
                }
                if (isEnemy)
                {
                    tile.onTarget = OnTile.Enemy;
                    tile.canMove = false;
                }
                else
                {
                    bool isObject = false;
                    foreach(int index in GameManager.instance.objects)
                    {
                        if (ReturnIndex(tile.rowIndex, tile.colIndex) == index)
                        {
                            tile.onTarget = OnTile.Object;
                            isObject = true;
                        }
                    }

                    if(!isObject)
                    {
                        tile.onTarget = OnTile.None;
                        tile.canMove = true;
                    }
                }
            }
        }
    }

    public int[] GetDirVec(int curRow, int curCol, int targetRow, int targetCol)
    {
        int dirRow = targetRow - curRow;
        int dirCol = targetCol - curCol;

        if (dirRow > 0)
            dirRow = 1;
        else if (dirRow < 0)
            dirRow = -1;
        if (dirCol > 0)
            dirCol = 1;
        else if (dirCol < 0)
            dirCol = -1;

        int[] vec = new int[2] { dirRow, dirCol };
        return vec;
    }

    public void ResetTilesColor()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.onTarget == OnTile.Empty)
                continue;

            SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
            sprite.color = Color.white;
        }
    }

    public void DrawRange(List<int[]> rangePos, Color color, int targetRow = 0, int targetCol = 0)
    {
        List<int[]> availIndex = new List<int[]>();
        for (int i = 0; i < rangePos.Count; i++)
        {
            int row = rangePos[i][0] + targetRow;
            int col = rangePos[i][1] + targetCol;
            availIndex.Add(new int[2] { row, col });
        }

        foreach (Tile tile in tiles)
        {
            if (tile.onTarget == OnTile.Empty)
                continue;

            SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
            sprite.color = Color.white;
            tile.inRange = false;
        }

        foreach (int[] index in availIndex)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.onTarget == OnTile.Empty)
                    continue;

                if (tile.rowIndex == index[0] && tile.colIndex == index[1])
                {
                    SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
                    sprite.color = color;
                    tile.inRange = true;
                }
            }
        }
    }

    public void BasicRangeDraw(int range, Color color)
    {
        int rowMin = GameManager.instance.player.curRow - range;
        int rowMax = GameManager.instance.player.curRow + range;
        int colMin = GameManager.instance.player.curCol - range;
        int colMax = GameManager.instance.player.curCol + range;
        foreach (Tile tile in tiles)
        {
            if (tile.onTarget == OnTile.Empty)
                continue;

            SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
            if (tile.rowIndex >= rowMin && tile.rowIndex <= rowMax && tile.colIndex >= colMin && tile.colIndex <= colMax)
            {
                sprite.color = color;
                tile.inRange = true;
            }
            else
            {
                sprite.color = Color.white;
                tile.inRange = false;
            }
        }
    }

    public void DrawAttackRange(SkillFunc skill, Color color,int targetRow = 0, int targetCol = 0)
    {
        // 일단 사거리내 버튼만 보이게
        switch(skill.skillInfo.type)
        {
            case AttackType.Basic:
                int range = skill.skillInfo.range;
                BasicRangeDraw(range, Color.blue);
                break;
            case AttackType.Range:
                {
                    List<int[]> rangePos = skill.RotateSkill();
                    DrawRange(rangePos, color, targetRow, targetCol);
                }
                break;
            case AttackType.Target:
                {
                    // 현재 타일에 적이 있다면 공격가능하게 그려주고, 아니라면 attackinput 풀어버리기(다시 사거리화면으로)
                    int index = GameManager.instance.uiManager.mapController.ReturnIndex(targetRow, targetCol);
                    if (GameManager.instance.uiManager.mapController.tiles[index].onTarget == OnTile.Enemy)
                    {
                        List<int[]> rangePos = skill.RotateSkill();
                        DrawRange(rangePos, color, targetRow, targetCol);
                    }
                    else
                    {
                        GameManager.instance.attackInput = false;
                    }
                }
                break;
            case AttackType.Buff:
                {
                    if(skill.skillInfo.range == 0)
                    {
                        List<int[]> rangePos = new List<int[]>();
                        int[] pos = new int[2] { 0, 0 };
                        rangePos.Add(pos);
                        DrawRange(rangePos, Color.yellow, targetRow, targetCol);
                    }
                    else
                    {
                        // 범위버프기 추가예정
                    }
                }
                break;
        }
    }

    public int ReturnIndex(int row, int col)
    {
        int index = (row * colSize) + col;
        return index;
    }

    public float CheckDist(int rowindex, int colindex, int playerRow, int playerCol)
    {
        //직선거리(코스트) 계산하는 함수
        float cost = 0;

        int rowDist = rowindex - playerRow;
        int colDist = colindex - playerCol;

        path = new List<int[]>();
        while(rowDist != 0 || colDist != 0)
        {
            if (rowDist < 0)
            {
                if (colDist < 0)
                {
                    int[] temp = new int[2] { -1, -1 };
                    rowDist -= -1;
                    colDist -= -1;
                    cost += 1.5f;
                    path.Add(temp);
                }
                else if (colDist > 0)
                {
                    int[] temp = new int[2] { -1, 1 };
                    rowDist -= -1;
                    colDist -= 1;
                    path.Add(temp);
                    cost += 1.5f;
                }
                else
                {
                    // rowDist만 계속 계산
                    int[] temp = new int[2] { -1, 0 };
                    for (int i = rowDist; i != 0; i++)
                    {
                        rowDist -= -1;
                        path.Add(temp);
                        cost++;
                    }
                }

            }
            else if (rowDist > 0)
            {
                if (colDist < 0)
                {
                    int[] temp = new int[2] { 1, -1 };
                    rowDist -= 1;
                    colDist -= -1;
                    cost += 1.5f;
                    path.Add(temp);
                }
                else if (colDist > 0)
                {
                    int[] temp = new int[2] { 1, 1 };
                    rowDist -= 1;
                    colDist -= 1;
                    path.Add(temp);
                    cost += 1.5f;
                }
                else
                {
                    int[] temp = new int[2] { 1, 0 };
                    for (int i = rowDist; i != 0; i--)
                    {
                        rowDist -= 1;
                        path.Add(temp);
                        cost++;
                    }
                }
            }
            else
            {
                if (colDist < 0)
                {
                    int[] temp = new int[2] { 0, -1 };
                    colDist -= -1;
                    cost++;
                    path.Add(temp);
                }
                else if (colDist > 0)
                {
                    int[] temp = new int[2] { 0, 1 };
                    colDist -= 1;
                    path.Add(temp);
                    cost++;
                }
            }
        }
        
        return cost;
    }

    public void MoveToTile()
    {
        if (GameManager.instance.player.stack >= pathCost)
        {
            // 이동처리
            GameManager.instance.moveInput = true;
            GameManager.instance.player.stack -= pathCost;
            ResetGuide();
        }
        else
        {
            //이동불가 출력 (코스트가 부족합니다)
        }
    }

    private void Tile_MouseCheck()
    {
        if (GameManager.instance.uiManager.uimouseOver)
        {
            if (curTile != null)
                curTile.DestroyGuide();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hit = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"));

        if(hit.Length > 0)
        {
            foreach(RaycastHit target in hit)
            {
                Tile tile = target.transform.GetComponentInParent<Tile>();
                if (tile != null)
                {
                    if (tile != curTile)
                    {
                        if (curTile != null)
                            curTile.DestroyGuide();
                        curTile = tile;
                        if (GameManager.instance.moveing == null && !GameManager.instance.skillManager.isAttacking)
                            GameManager.instance.player.RotateChar(tile.rowIndex, tile.colIndex);
                    }

                    if (GameManager.instance.phase == 2 && GameManager.instance.moveing == null && tile.canMove)
                    {
                        if (!tile.guideOn)
                            tile.DrawGuide();

                        if (Input.GetKeyUp(KeyCode.Mouse0))
                            MoveToTile();
                    }
                    else if (GameManager.instance.phase == 3 && !GameManager.instance.skillManager.isAttacking)
                    {
                        if (Input.GetKeyUp(KeyCode.Mouse0) && GameManager.instance.skillManager.currSkill != null && tile.inRange)
                            tile.Attack();
                    }
                }
            }
        }
        else if (curTile != null)
        {
            curTile.DestroyGuide();
            curTile = null;
        }
    }

    public void Init()
    {
        List<Transform> rows = new List<Transform>();
        int count = 0;
        while(true)
        {
            Transform row = transform.Find("Row" + count.ToString());
            if (row != null)
            {
                rows.Add(row);
                count++;
            }
            else
                break;
        }


        for (int i = 0; i < rows.Count; i++)
        {
            count = 0;
            List<Tile> cols = new List<Tile>();
            while(true)
            {
                Transform col = rows[i].transform.Find("Col" + count.ToString());
                if (col != null)
                {
                    Tile tile = col.GetComponent<Tile>();
                    tile.colIndex = count;
                    tile.rowIndex = i;
                    tile.Init();
                    tiles.Add(tile);
                    count++;
                }
                else
                    break;
            }            
        }

        rowSize = rows.Count;
        colSize = count;

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].tileIndex = i;
        }

        costText = FindObjectOfType<TileGuide1>().GetComponentInChildren<TextMeshProUGUI>(true);
        if (costText != null)
            costText.gameObject.SetActive(false);
        isinit = true;
    }

    private void Update()
    {
        if (GameManager.instance == null)
            return;

        if (GameManager.instance.phase == 2 || GameManager.instance.phase == 3)
        {
            Tile_MouseCheck();
        }
    }
}
