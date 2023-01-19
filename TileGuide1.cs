using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileGuide1 : MonoBehaviour
{
    public static TileGuide1 instance;

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
                    int playerRow = GameManager.instance.player.curRow;
                    int playerCol = GameManager.instance.player.curCol;
                    int playerIndex = GameManager.instance.mapController.ReturnIndex(playerRow, playerCol);

                    List<int> indexs = new List<int>();

                    indexs.Add(playerIndex);

                    foreach (int i in GameManager.instance.mapController.pathIndexs)
                    {
                        indexs.Add(i);
                    }

                    Transform guideTrans = transform.Find("GuideLine");

                    for (int i = 0; i < indexs.Count; i++)
                    {
                        if (i == indexs.Count - 1)
                        {
                            Vector2 rec = Camera.main.WorldToScreenPoint(GameManager.instance.mapController.tiles[indexs[i]].transform.position);
                            costText.gameObject.SetActive(true);
                            costText.transform.position = rec;
                            break;
                        }
                        // indexs[i] 와 indexs[i+1]을 비교하여 방향에 맞는 길 출력

                        int curRow = GameManager.instance.mapController.tiles[indexs[i]].rowIndex;
                        int curCol = GameManager.instance.mapController.tiles[indexs[i]].colIndex;
                        int nextRow = GameManager.instance.mapController.tiles[indexs[i + 1]].rowIndex;
                        int nextCol = GameManager.instance.mapController.tiles[indexs[i + 1]].colIndex;
                        int[] vec = new int[2];
                        vec = GameManager.instance.mapController.GetDirVec(curRow, curCol, nextRow, nextCol);

                        
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
                        
                        Vector2 targetVec = Camera.main.WorldToScreenPoint(GameManager.instance.mapController.tiles[indexs[i]].transform.position);
                        Vector2 targetVec2 = Camera.main.WorldToScreenPoint(GameManager.instance.mapController.tiles[indexs[i+1]].transform.position);
                        Vector2 finalVec = (targetVec2 - targetVec) * 0.5f + targetVec;
                        RectTransform rect = guide.GetComponent<RectTransform>();
                        rect.position = finalVec;
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
                    costText.text = "Cost : " + GameManager.instance.mapController.pathCost.ToString();
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
