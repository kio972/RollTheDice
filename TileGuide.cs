using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileGuide : MonoBehaviour
{
    public static TileGuide instance;

    Image row;
    Image col;
    Image ver;
    Image hor;

    Image guide;

    TextMeshProUGUI costText;

    List<Image> guideLine;
    public void DestroyGuide()
    {
        foreach (Image image in guideLine)
        {
            Destroy(image.gameObject);
        }
        guideLine = new List<Image>();
        costText.gameObject.SetActive(false);
    }

    public void DrawGuide(bool haveCost = true)
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.mapController.pathIndexs != null)
            {
                if (GameManager.instance.mapController.pathIndexs.Count > 0)
                {
                    List<int> indexs = new List<int>();
                    foreach (int i in GameManager.instance.mapController.pathIndexs)
                    {
                        indexs.Add(i);
                    }

                    int playerRow = GameManager.instance.player.curRow;
                    int playerCol = GameManager.instance.player.curCol;
                    int firstRow = GameManager.instance.mapController.tiles[indexs[0]].rowIndex;
                    int firstCol = GameManager.instance.mapController.tiles[indexs[0]].colIndex;
                    int[] dir = new int[2];
                    dir = GameManager.instance.mapController.GetDirVec(playerRow, playerCol, firstRow, firstCol);

                    Transform guideTrans = transform.Find("GuideLine");

                    if (dir[0] == 0)
                    {
                        if (dir[1] != 0)
                        {
                            guide = Instantiate(col, guideTrans);
                        }
                    }
                    else
                    {
                        if (dir[1] == 0)
                        {
                            guide = Instantiate(row, guideTrans);
                        }
                        else if (dir[0] == dir[1])
                        {
                            guide = Instantiate(hor, guideTrans);
                        }
                        else if (dir[0] == -dir[1])
                        {
                            guide = Instantiate(ver, guideTrans);
                        }
                    }

                    int playerIndex = GameManager.instance.mapController.ReturnIndex(playerRow, playerCol);
                    RectTransform firstRect = guide.GetComponent<RectTransform>();
                    RectTransform playerRect = GameManager.instance.mapController.tiles[playerIndex].GetComponent<RectTransform>();
                    RectTransform firstTarget = GameManager.instance.mapController.tiles[indexs[0]].GetComponent<RectTransform>();
                    Vector2 firstVec = (firstTarget.position - playerRect.position) * 0.5f + playerRect.position;
                    firstRect.position = firstVec;
                    guideLine.Add(guide);

                    for (int i = 0; i < indexs.Count; i++)
                    {
                        if (i == indexs.Count - 1)
                        {
                            RectTransform rec = GameManager.instance.uiManager.mapController.tiles[indexs[i]].GetComponent<RectTransform>();
                            costText.gameObject.SetActive(true);
                            costText.transform.position = rec.position;
                            break;
                        }
                        // indexs[i] 와 indexs[i+1]을 비교하여 방향에 맞는 길 출력

                        int curRow = GameManager.instance.uiManager.mapController.tiles[indexs[i]].rowIndex;
                        int curCol = GameManager.instance.uiManager.mapController.tiles[indexs[i]].colIndex;
                        int nextRow = GameManager.instance.uiManager.mapController.tiles[indexs[i + 1]].rowIndex;
                        int nextCol = GameManager.instance.uiManager.mapController.tiles[indexs[i + 1]].colIndex;
                        int[] vec = new int[2];
                        vec = GameManager.instance.uiManager.mapController.GetDirVec(curRow, curCol, nextRow, nextCol);

                        
                        if (vec[0] == 0)
                        {
                            if (vec[1] != 0)
                            {
                                guide = Instantiate(col, guideTrans);
                            }
                        }
                        else
                        {
                            if (vec[1] == 0)
                            {
                                guide = Instantiate(row, guideTrans);
                            }
                            else if (vec[0] == vec[1])
                            {
                                guide = Instantiate(hor, guideTrans);
                            }
                            else if (vec[0] == -vec[1])
                            {
                                guide = Instantiate(ver, guideTrans);
                            }
                        }

                        RectTransform rect = guide.GetComponent<RectTransform>();
                        RectTransform targetRect = GameManager.instance.uiManager.mapController.tiles[indexs[i]].GetComponent<RectTransform>();
                        RectTransform targetRect2 = GameManager.instance.uiManager.mapController.tiles[indexs[i+1]].GetComponent<RectTransform>();
                        Vector2 tagetVec = (targetRect2.position - targetRect.position) * 0.5f + targetRect.position;
                        rect.position = tagetVec;
                        guideLine.Add(guide);
                    }
                    
                    
                    if(!haveCost)
                    {
                        foreach(Image image in guideLine)
                        {
                            image.color = Color.gray;
                        }
                        costText.color = Color.red;
                    }
                    else
                    {
                        costText.color = Color.yellow;
                    }
                    costText.text = "Cost : " + GameManager.instance.uiManager.mapController.pathCost.ToString();
                }
            }
        }
    }

    private void Start()
    {
        row = Resources.Load<Image>("Prefab/Battle/Guide/Row");
        col = Resources.Load<Image>("Prefab/Battle/Guide/Col");
        ver = Resources.Load<Image>("Prefab/Battle/Guide/Ver");
        hor = Resources.Load<Image>("Prefab/Battle/Guide/Hor");
        guideLine = new List<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>(true);
        if(costText != null)
            costText.gameObject.SetActive(false);
        instance = this;
    }


}
