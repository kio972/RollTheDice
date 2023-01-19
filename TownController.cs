using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Buildings
{
    Town,
    Tavern,
    Shop,
    Guild,
    Board,
}

public class TownController : MonoBehaviour
{
    public static TownController instance;

    public Canvas canvas;

    public PlayerController player;

    public NPCController npc;

    public UIManager uiManager;
    public MoveToBuilding moveToBuildings;

    public Coroutine moving;

    private Buildings curScene = Buildings.Town;

    public FastTravel fastTravel;
    
    private SpriteRenderer townBg;

    private Transform town;
    private Tavern tavern;
    private Board board;
    private GuildController guild;
    private ShopController shop;
    private Button goTownBtn;

    public bool sceneChanging = false;

    public TownBuildingFX buildingfx;

    public bool goEnd = false;

    private int clearWeek = 5;

    public void ActiveTown()
    {
        moveToBuildings.gameObject.SetActive(true);
        town.gameObject.SetActive(true);
        goTownBtn.gameObject.SetActive(false);
    }

    public void DeActiveTown()
    {
        moveToBuildings.gameObject.SetActive(false);
        town.gameObject.SetActive(false);
        goTownBtn.gameObject.SetActive(true);
    }

    public void ActiveTavern()
    {
        tavern.gameObject.SetActive(true);
        tavern.tavernUi.gameObject.SetActive(true);
    }

    public void DeActiveTavern()
    {
        tavern.tavernUi.gameObject.SetActive(false);
        tavern.gameObject.SetActive(false);
        npc.gameObject.SetActive(false);
    }

    public void ActiveBoard()
    {
        board.gameObject.SetActive(true);
        board.boardUI.gameObject.SetActive(true);
    }

    public void DeActiveBoard()
    {
        board.boardUI.gameObject.SetActive(false);
        board.gameObject.SetActive(false);
    }

    public void ActiveGuild()
    {
        guild.gameObject.SetActive(true);
        guild.guildUI.gameObject.SetActive(true);
    }

    public void DeActiveGuild()
    {
        guild.skillLearn.gameObject.SetActive(false);
        guild.skillNPCOnClick.gameObject.SetActive(false);
        guild.guildUI.gameObject.SetActive(false);
        guild.gameObject.SetActive(false);
        npc.gameObject.SetActive(false);
    }

    public void ActiveShop()
    {
        shop.gameObject.SetActive(true);
        shop.shopUI.gameObject.SetActive(true);
    }

    public void DeActiveShop()
    {
        shop.shopUI.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        npc.gameObject.SetActive(false);
    }

    public IEnumerator ChangeScene(Buildings building, float time = 1f)
    {
        sceneChanging = true;
        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if(fade != null)
        {
            fade.gameObject.SetActive(true);
            fade.FadeO(time, 0.1f, true);
        }
        
        float elapsed = 0;
        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        switch (curScene)
        {
            case Buildings.Town:
                DeActiveTown();
                break;
            case Buildings.Tavern:
                DeActiveTavern();
                break;
            case Buildings.Shop:
                DeActiveShop();
                break;
            case Buildings.Guild:
                DeActiveGuild();
                break;
            case Buildings.Board:
                DeActiveBoard();
                break;
        }

        curScene = building;
        switch (curScene)
        {
            case Buildings.Town:
                ActiveTown();
                break;
            case Buildings.Tavern:
                ActiveTavern();
                break;
            case Buildings.Shop:
                ActiveShop();
                break;
            case Buildings.Guild:
                ActiveGuild();
                break;
            case Buildings.Board:
                ActiveBoard();
                break;
        }

        yield return null;
        fade.FadeI();
        sceneChanging = false;
    }

    public void MoveCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interact"))
            {
                string building = hit.transform.gameObject.name;
                buildingfx = hit.transform.GetComponent<TownBuildingFX>();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    switch (building)
                    {
                        case "Tavern":
                            moveToBuildings.GoTavern();
                            break;
                        case "Board":
                            moveToBuildings.GoBoard();
                            break;
                        case "Guild":
                            moveToBuildings.GoGuild();
                            break;
                        case "Shop":
                            moveToBuildings.GoShop();
                            break;
                    }
                }
            }
        }
        else if (buildingfx != null)
            buildingfx = null;
    }

    private void GoBackTown()
    {
        StartCoroutine(ChangeScene(Buildings.Town));
    }

    private void SceneChangeOn()
    {
        sceneChanging = false;
    }

    private void OpenScene()
    {
        sceneChanging = true;
        FadeOut fade = FindObjectOfType<FadeOut>();
        if (fade != null)
            fade.FadeI();
    }

    private void GameClearCheck()
    {
        if(GameData.week > clearWeek)
        {
            GameObject temp = new GameObject();
            SceneMoveBtn btn = temp.AddComponent<SceneMoveBtn>(); ;
            goEnd = true;
            btn.sceneName = "EndScene";
            btn.Move();
        }
    }

    public void Init()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
        Transform[] uIs = canvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in uIs)
        {
            t.gameObject.SetActive(true);
        }

        uiManager = canvas.GetComponent<UIManager>();
        uiManager.Init();

        player = FindObjectOfType<PlayerController>();
        if(player != null)
            player.Init();

        npc = FindObjectOfType<NPCController>(true);
        if(npc != null)
        {
            npc.Init();
            npc.gameObject.SetActive(false);
        }


        fastTravel = canvas.GetComponentInChildren<FastTravel>();
        if (fastTravel != null)
        {
            fastTravel.Init();
        }

        moveToBuildings = FindObjectOfType<MoveToBuilding>();
        if(moveToBuildings != null)
        {
            moveToBuildings.Init();
        }

        town = transform.Find("TownBuilding");

        tavern = FindObjectOfType<Tavern>(true);
        if(tavern != null)
        {
            tavern.Init();
            tavern.gameObject.SetActive(false);
        }

        board = FindObjectOfType<Board>(true);
        if(board != null)
        {
            board.Init();
            board.gameObject.SetActive(false);
        }

        guild = FindObjectOfType<GuildController>(true);
        if (guild != null)
        {
            guild.Init();
            guild.gameObject.SetActive(false);
        }

        shop = FindObjectOfType<ShopController>(true);
        if (shop != null)
        {
            shop.Init();
            shop.gameObject.SetActive(false);
        }

        goTownBtn = UtillHelper.GetComponent<Button>("GoTown", canvas.transform);
        if(goTownBtn != null)
        {
            goTownBtn.onClick.AddListener(GoBackTown);
            goTownBtn.gameObject.SetActive(false);
        }

        GameClearCheck();

        if (goEnd)
            return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/BackGround/town");
        AudioManager.Instance.PlayBG(clip);

        Timer timer = FindObjectOfType<Timer>();
        if(timer != null)
            timer.Init();

        Invoke("OpenScene", 0.5f);

        Time.timeScale = GameSpeedController.SetSpeed(SaveManager.Instance.settingData.gameSpeed);
    }

    void Start()
    {
        instance = this;
        Init();
    }

    void Update()
    {
        if (Pause.instance != null && Pause.instance.pause)
            return;
            
        MoveCheck();
    }
}
