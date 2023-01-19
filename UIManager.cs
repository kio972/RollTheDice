using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public GraphicRaycaster raycaster;
    public PointerEventData pointerEventData;

    public MapController mapController;
    public StackUpdater stackUpdater;
    public DescManager descManager;

    public Animator tableGate;
    public EndTurnBtn endTurn;

    public RawImage stackDice;

    public Button[] townButtons;
    public TextMeshProUGUI helpText;

    public bool uimouseOver = false;

    private void MouseOverCheck()
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
            uimouseOver = true;
        else
            uimouseOver = false;
    }

    public void UpdateCharDir()
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        if(results.Count > 0)
        {
            Tile tile = results[0].gameObject.transform.GetComponent<Tile>();
            if (tile != null)
            {
                int dirRow = tile.rowIndex - GameManager.instance.player.curRow;
                int dirCol = tile.colIndex - GameManager.instance.player.curCol;
                GameManager.instance.player.SetDir(dirRow, dirCol);

                if (GameManager.instance.player.curDir == CharDir.W || GameManager.instance.player.curDir == CharDir.D)
                    GameManager.instance.player.charImg.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                else
                    GameManager.instance.player.charImg.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }

    public void Init()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        pointerEventData = new PointerEventData(null);

        mapController = GetComponentInChildren<MapController>();
        if(mapController != null)
        {
            mapController.Init();
            mapController.gameObject.SetActive(false);
        }

        stackUpdater = GetComponentInChildren<StackUpdater>();
        if(stackUpdater != null)
            stackUpdater.Init();

        tableGate = UtillHelper.GetComponent<Animator>("Table/Gate", transform);

        helpText = UtillHelper.GetComponent<TextMeshProUGUI>("HelpText/Text", transform);
        if (helpText != null)
            helpText.gameObject.SetActive(false);

        endTurn = UtillHelper.GetComponent<EndTurnBtn>("EndTurnBtn", transform, true);

        descManager = GetComponentInChildren<DescManager>();
        if(descManager != null)
            descManager.Init();
    }

    private void Update()
    {
        MouseOverCheck();
    }
}
