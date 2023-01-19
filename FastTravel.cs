using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravel : MonoBehaviour
{
    private Button tavern;
    private Button shop;
    private Button guild;
    private Button board;

    private void GoTavern()
    {
        StartCoroutine(TownController.instance.ChangeScene(Buildings.Tavern));
    }

    private void GoShop()
    {
        StartCoroutine(TownController.instance.ChangeScene(Buildings.Shop));
    }

    private void GoGuild()
    {
        StartCoroutine(TownController.instance.ChangeScene(Buildings.Guild));
    }

    private void GoBoard()
    {
        StartCoroutine(TownController.instance.ChangeScene(Buildings.Board));
    }

    public void Init()
    {
        tavern = transform.Find("Tavern").GetComponent<Button>();
        tavern.onClick.AddListener(GoTavern);

        shop = transform.Find("Shop").GetComponent<Button>();
        shop.onClick.AddListener(GoShop);

        guild = transform.Find("Guild").GetComponent<Button>();
        guild.onClick.AddListener(GoGuild);

        board = transform.Find("Board").GetComponent<Button>();
        board.onClick.AddListener(GoBoard);
    }
    
}
