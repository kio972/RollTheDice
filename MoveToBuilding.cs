using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MoveToBuilding : MonoBehaviour
{


    private Button tavern;
    private Button shop;
    private Button guild;
    private Button board;

    private Transform tavernTrans;
    private Transform shopTrans;
    private Transform guildTrans;
    private Transform boardTrans;



    IEnumerator MoveToBuildings(Buildings building)
    {
        TownController.instance.sceneChanging = true;
        yield return null;
        PlayerController player = TownController.instance.player;

        TownController.instance.moving = null;
        StartCoroutine(TownController.instance.ChangeScene(building));
    }

    public void GoTavern()
    {
        if (TownController.instance.moving != null)
            StopCoroutine(TownController.instance.moving);
        TownController.instance.moving = StartCoroutine(MoveToBuildings(Buildings.Tavern));
        print("goTavern");
    }

    public void GoShop()
    {
        if (TownController.instance.moving != null)
            StopCoroutine(TownController.instance.moving);
        TownController.instance.moving = StartCoroutine(MoveToBuildings(Buildings.Shop));
        print("goShop");
    }

    public void GoGuild()
    {
        if (TownController.instance.moving != null)
            StopCoroutine(TownController.instance.moving);
        TownController.instance.moving = StartCoroutine(MoveToBuildings(Buildings.Guild));
        print("goGoGuild");
    }

    public void GoBoard()
    {
        if (TownController.instance.moving != null)
            StopCoroutine(TownController.instance.moving);
        TownController.instance.moving = StartCoroutine(MoveToBuildings(Buildings.Board));
        print("goGoBoard");
    }

    public void Init()
    {
        tavern = transform.Find("Tavern").GetComponent<Button>();
        tavern.onClick.AddListener(GoTavern);
        tavernTrans = TownController.instance.transform.Find("TownBuilding/Tavern");
        RectTransform buildingPos = tavern.GetComponent<RectTransform>();
        Vector2 pos = Camera.main.WorldToScreenPoint(tavernTrans.position);
        buildingPos.position = pos + new Vector2(0, 120);

        shop = transform.Find("Shop").GetComponent<Button>();
        shop.onClick.AddListener(GoShop);
        shopTrans = TownController.instance.transform.Find("TownBuilding/Shop");
        buildingPos = shop.GetComponent<RectTransform>();
        pos = Camera.main.WorldToScreenPoint(shopTrans.position);
        buildingPos.position = pos + new Vector2(0, 120);


        guild = transform.Find("Guild").GetComponent<Button>();
        guild.onClick.AddListener(GoGuild);
        guildTrans = TownController.instance.transform.Find("TownBuilding/Guild");
        buildingPos = guild.GetComponent<RectTransform>();
        pos = Camera.main.WorldToScreenPoint(guildTrans.position);
        buildingPos.position = pos + new Vector2(0, 120);


        board = transform.Find("Board").GetComponent<Button>();
        board.onClick.AddListener(GoBoard);
        boardTrans = TownController.instance.transform.Find("TownBuilding/Board");
        buildingPos = board.GetComponent<RectTransform>();
        pos = Camera.main.WorldToScreenPoint(boardTrans.position);
        buildingPos.position = pos + new Vector2(0, 120);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
