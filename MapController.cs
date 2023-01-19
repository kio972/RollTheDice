using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    private int rowSize;
    private int colSize;
    public List<Tile> tiles;
    public List<Transform> tileTransform;

    private List<int[]> path;
    
    public List<int> pathIndexs;
    public float pathCost;

    public Tile curTile;

    public int mapRowsize
    {
        get { return rowSize; }
    }
    public int mapColsize
    {
        get { return colSize; }
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
        enemy.transform.position = tileTransform[index].position;
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

    public void FindPath(int startNodeIndex, int destNodeIndex)
    {
        pathIndexs = new List<int>();
        pathCost = 0;
        List<Node> openNode = new List<Node>();
        List<Node> closeNode = new List<Node>();

        foreach (Tile tile in tiles)
        {
            tile.prevTile = null;
        }

        int curNodeIndex = startNodeIndex;
        int bugCount = 0;
        while(curNodeIndex != destNodeIndex)
        {
            // 인접노드를 계산하고, 해당노드의 G, H, F코스트와 index기록하고 현재 오픈노드에 없다면 추가
            for (int i = 0; i < 8; i++)
            {
                bugCount++;
                if(bugCount > 10000)
                {
                    pathIndexs = null;
                    print("No path");
                    return;
                }

                int neighborRow = 0;
                int neighborCol = 0;
                switch (i)
                {
                    case 0:
                        neighborRow = tiles[curNodeIndex].rowIndex - 1;
                        neighborCol = tiles[curNodeIndex].colIndex - 1;
                        break;
                    case 1:
                        neighborRow = tiles[curNodeIndex].rowIndex - 1;
                        neighborCol = tiles[curNodeIndex].colIndex;
                        break;
                    case 2:
                        neighborRow = tiles[curNodeIndex].rowIndex - 1;
                        neighborCol = tiles[curNodeIndex].colIndex + 1;
                        break;
                    case 3:
                        neighborRow = tiles[curNodeIndex].rowIndex;
                        neighborCol = tiles[curNodeIndex].colIndex - 1;
                        break;
                    case 4:
                        neighborRow = tiles[curNodeIndex].rowIndex;
                        neighborCol = tiles[curNodeIndex].colIndex + 1;
                        break;
                    case 5:
                        neighborRow = tiles[curNodeIndex].rowIndex + 1;
                        neighborCol = tiles[curNodeIndex].colIndex - 1;
                        break;
                    case 6:
                        neighborRow = tiles[curNodeIndex].rowIndex + 1;
                        neighborCol = tiles[curNodeIndex].colIndex;
                        break;
                    case 7:
                        neighborRow = tiles[curNodeIndex].rowIndex + 1;
                        neighborCol = tiles[curNodeIndex].colIndex + 1;
                        break;
                }

                if (neighborRow >= 0 && neighborRow < rowSize && neighborCol >= 0 && neighborCol < colSize)
                {
                    int index = ReturnIndex(neighborRow, neighborCol);
                    if (tiles[index].canMove && index != startNodeIndex)
                    {
                        Node neighborNode = new Node();
                        neighborNode.index = index;

                        // close list 안에 있는지 확인
                        bool isList = false;
                        foreach (Node nodes in closeNode)
                        {
                            if (nodes.index == index)
                            {
                                isList = true;
                                break;
                            }
                        }

                        // close list 안에 없다면 open list안에 있는지 확인
                        if(!isList)
                        {
                            Node temp = new Node();
                            foreach (Node nodes in openNode)
                            {
                                if (nodes.index == index)
                                {
                                    temp = nodes;
                                    isList = true;
                                    break;
                                }
                            }

                            if (!isList)
                            {
                                // 이웃 타일이 오픈노드 리스트에 없다면, 추가하고 이전타일을 기록
                                tiles[index].prevTile = tiles[curNodeIndex];
                                neighborNode.gCost = GetGCost(index, startNodeIndex);
                                neighborNode.HCost = GetHCost(index, destNodeIndex);
                                neighborNode.FCost = neighborNode.gCost + neighborNode.HCost;
                                openNode.Add(neighborNode);
                            }
                            else
                            {
                                // 이미 리스트에 있다면, 리스트의 현재 G코스트보다 크다면 업데이트(제거 후 등록) 현재 prevTile을 일단기록
                                Tile tempPrevTile = tiles[index].prevTile;

                                tiles[index].prevTile = tiles[curNodeIndex];
                                neighborNode.gCost = GetGCost(index, startNodeIndex);
                                neighborNode.HCost = GetHCost(index, destNodeIndex);
                                neighborNode.FCost = neighborNode.gCost + neighborNode.HCost;

                                if (temp.gCost > neighborNode.gCost)
                                {
                                    openNode.Remove(temp);
                                    tiles[index].prevTile = tiles[curNodeIndex];
                                    openNode.Add(neighborNode);
                                }
                                else
                                {
                                    tiles[index].prevTile = tempPrevTile;
                                }
                            }
                        }
                    }
                }
            }

            // 오픈노드들 중 가장 F코스트가 낮은 노드를 close노드에 추가하고, open노드에서 제거
            int minDistIndex = 0;
            float minCost = 10000;
            foreach (Node nodes in openNode)
            {
                if (nodes.FCost < minCost)
                {
                    minCost = nodes.FCost;
                    minDistIndex = nodes.index;
                }
            }
            Node nextNode = new Node();
            foreach (Node nodes in openNode)
            {
                if (nodes.index == minDistIndex)
                {
                    nextNode = nodes;
                    break;
                }
            }
            openNode.Remove(nextNode);
            closeNode.Add(nextNode);
            curNodeIndex = nextNode.index;
        }

        
        // 길찾기 종료, 도착지로부터 이전노드가 시작노드에 도착할때까지 기록
        Tile tempNode = tiles[destNodeIndex];
        int tempIndex = destNodeIndex;
        while (tempIndex != startNodeIndex)
        {
            if(tempNode.prevTile == null)
            {
                print("No path");
                return;
            }
            pathIndexs.Add(tempIndex);
            pathCost += GetHCost(tempIndex, ReturnIndex(tempNode.prevTile.rowIndex, tempNode.prevTile.colIndex));
            tempNode = tempNode.prevTile;
            tempIndex = ReturnIndex(tempNode.rowIndex, tempNode.colIndex);
        }
        pathIndexs.Reverse();
    }

    public void UpdateOnTargetTiles()
    {
        int playerRow = GameManager.instance.player.curRow;
        int playerCol = GameManager.instance.player.curCol;
        foreach (Tile tile in tiles)
        {
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
                    tile.onTarget = OnTile.None;
                    tile.canMove = true;
                    foreach(int index in GameManager.instance.objects)
                    {
                        if (ReturnIndex(tile.rowIndex, tile.colIndex) == index)
                        {
                            tile.onTarget = OnTile.Object;
                            tile.canMove = false;
                        }
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

    public void ResetTilesColor(bool alpha = false)
    {
        foreach (Tile tile in tiles)
        {
            if(alpha)
                tile.image.color = Color.clear;
            else
                tile.image.color = tile.defaultColor;
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
            tile.gameObject.SetActive(true);
            tile.image.color = Color.clear;
            tile.inRange = false;
        }

        foreach (int[] index in availIndex)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.rowIndex == index[0] && tile.colIndex == index[1])
                {
                    tile.image.color = color;
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
            tile.gameObject.SetActive(true);

            if (tile.rowIndex >= rowMin && tile.rowIndex <= rowMax && tile.colIndex >= colMin && tile.colIndex <= colMax)
            {
                tile.image.color = color;
                tile.inRange = true;
            }
            else
            {
                tile.image.color = Color.clear;
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

    public void FindTileTransform()
    {
        List<Transform> rows = new List<Transform>();
        int count = 0;
        while (true)
        {
            Transform map = GameObject.Find("Map").GetComponent<Transform>();
            Transform row = map.transform.Find("Row" + count.ToString());
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
            while (true)
            {
                Transform col = rows[i].transform.Find("Col" + count.ToString());
                if (col != null)
                {
                    tileTransform.Add(col);
                    count++;
                }
                else
                    break;
            }
        }
    }

    public void Init()
    {
        instance = this;

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

        foreach(Tile tile in tiles)
        {
            tile.tileIndex = ReturnIndex(tile.rowIndex, tile.colIndex);
        }

        FindTileTransform();
    }
}
