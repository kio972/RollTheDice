using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartBless : MonoBehaviour
{
    private SceneMoveBtn moveBtn;

    private Transform playerHpBar;
    private Tutorial tutorial;

    [SerializeField]
    private ParticleSystem effect;

    [SerializeField]
    private Button leftBtn;
    [SerializeField]
    private Button rightBtn;

    private List<int> startRelicIndexs;
    [SerializeField]
    private Transform relicImgGroup;
    [SerializeField]
    private Image curRelicImg;
    [SerializeField]
    private Image nextRelicImg;
    private int curRelicIndex;
    

    Button getBtn;

    public float changeSpeed = 1f;

    Coroutine changing;

    private void CloseTip()
    {
        tutorial.gameObject.SetActive(false);
        moveBtn.gameObject.SetActive(true);
        moveBtn.sceneName = "StartBoard";
    }

    private void NextTip()
    {
        tutorial.tipHealth.gameObject.SetActive(false);
        tutorial.tipRelic.gameObject.SetActive(true);
        tutorial.btn.onClick.RemoveAllListeners();
        tutorial.btn.onClick.AddListener(CloseTip);
    }

    private void PopUpTutorial()
    {
        tutorial.gameObject.SetActive(true);
        tutorial.tipHealth.gameObject.SetActive(true);
        tutorial.tipRelic.gameObject.SetActive(false);
        tutorial.btn.onClick.AddListener(NextTip);
    }

    private void DisableGetRelics()
    {
        leftBtn.gameObject.SetActive(false);
        rightBtn.gameObject.SetActive(false);
        curRelicImg.gameObject.SetActive(false);
        getBtn.gameObject.SetActive(false);
        TextMeshProUGUI helpText = UtillHelper.GetComponent<TextMeshProUGUI>("HelpText", transform);
        helpText.gameObject.SetActive(false);
        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void GetStartRelic()
    {
        PlayerStat.instance.relic.GetRelic(startRelicIndexs[curRelicIndex]);
        if (playerHpBar != null)
            playerHpBar.gameObject.SetActive(true);

        FxEffectManager.Instance.PlayEffect("Prefab/Battle/Effect/SelectEffect", Vector3.zero);
        AddStartSkill();
        DisableGetRelics();

        // Æ©Åä¸®¾ó ¶ç¿ì±â
        Invoke("PopUpTutorial", 1.0f);
    }

    private IEnumerator TextChange(int nextRelicIndex)
    {
        TextMeshProUGUI nameText = UtillHelper.GetComponent<TextMeshProUGUI>("Desc/RelicName", transform);
        TextMeshProUGUI descText = UtillHelper.GetComponent<TextMeshProUGUI>("Desc/RelicDesc", transform);
        float elapsed = 0;
        while(elapsed < 1)
        {
            elapsed += Time.deltaTime * 2f;
            Color color1 = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 1 - elapsed);
            nameText.color = color1;
            Color color2 = new Color(descText.color.r, descText.color.g, descText.color.b, 1 - elapsed);
            descText.color = color2;
            yield return null;
        }

        elapsed = 0;
        nameText.text = DataManger.RelicData[nextRelicIndex]["Name"];
        descText.text = DataManger.RelicData[nextRelicIndex]["Description"];

        while (elapsed < 1)
        {
            elapsed += Time.deltaTime * 2f;
            Color color1 = new Color(nameText.color.r, nameText.color.g, nameText.color.b, elapsed);
            nameText.color = color1;
            Color color2 = new Color(descText.color.r, descText.color.g, descText.color.b, elapsed);
            descText.color = color2;
            yield return null;
        }

        yield return null;
    }

    private void AddStartSkill()
    {
        GameData.playerSkills = new List<int>();
        GameData.playerSkills.Add(0);
        GameData.playerSkills.Add(1);
        GameData.playerSkills.Add(3);
        GameData.quickSlotSkills = new List<int>();
        GameData.quickSlotSkills.Add(0);
        GameData.quickSlotSkills.Add(1);
        GameData.quickSlotSkills.Add(3);
    }

    private void SetRelicImg(int index, bool isNext = false)
    {
        Image img = Resources.Load<Image>("Prefab/Relics/Relic" + index.ToString());
        if (!isNext)
        {
            curRelicImg.sprite = img.sprite;
        }
        else
        {
            nextRelicImg.sprite = img.sprite;
        }
    }

    private void SetStartRelic()
    {
        getBtn = UtillHelper.GetComponent<Button>("StartRelics/Relics/Btn", transform);
        if (getBtn != null)
            getBtn.onClick.AddListener(GetStartRelic);

        startRelicIndexs = new List<int>();
        foreach(int i in SaveManager.Instance.settingData.startRelics)
        {
            startRelicIndexs.Add(i);
        }

        curRelicIndex = startRelicIndexs[0];
        SetRelicImg(startRelicIndexs[0]);
    }

    private IEnumerator Setting(int index, bool left = false)
    {
        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        effect.Play();

        SetRelicImg(index, true);
        StartCoroutine(TextChange(index));

        Vector2 prevPos = new Vector2(relicImgGroup.transform.position.x, relicImgGroup.transform.position.y);
        Vector2 nextPos = prevPos;
        if (left)
        {
            nextPos.x -= 400;
            nextRelicImg.transform.position = nextPos;
            nextPos.x += 800;
        }
        else
        {
            nextPos.x += 400;
            nextRelicImg.transform.position = nextPos;
            nextPos.x -= 800;
        }

        while (true)
        {
            if (left)
                relicImgGroup.transform.Translate(Vector2.right * 5f * changeSpeed);
            else
                relicImgGroup.transform.Translate(Vector2.left * 5f * changeSpeed);

            if (((Vector2)relicImgGroup.transform.position - nextPos).magnitude < 2f)
                break;

            yield return null;
        }

        SetRelicImg(index);
        relicImgGroup.transform.position = prevPos;
        changing = null;
        yield return null;
    }

    private void LeftBtn()
    {
        if (changing != null)
            return;

        curRelicIndex--;
        if (curRelicIndex < 0)
            curRelicIndex = startRelicIndexs.Count - 1;

        changing = StartCoroutine(Setting(startRelicIndexs[curRelicIndex], true));
    }

    private void RightBtn()
    {
        if (changing != null)
            return;

        curRelicIndex++;
        if (curRelicIndex > startRelicIndexs.Count - 1)
            curRelicIndex = 0;

        changing = StartCoroutine(Setting(startRelicIndexs[curRelicIndex]));
    }

    private void Start()
    {
        GameData.playerCurHp = 60;
        GameData.playerMaxHp = 60;
        GameData.week = 1;
        PlayerController player = FindObjectOfType<PlayerController>();
        player.Init();

        SetStartRelic();

        moveBtn = GetComponentInChildren<SceneMoveBtn>(true);
        moveBtn.gameObject.SetActive(false);

        leftBtn.onClick.AddListener(LeftBtn);
        rightBtn.onClick.AddListener(RightBtn);

        tutorial = GetComponentInChildren<Tutorial>(true);
        if (tutorial != null)
            tutorial.gameObject.SetActive(false);

        FadeOut fade = FindObjectOfType<FadeOut>();
        if (fade != null)
            fade.FadeI();

        SaveManager.Instance.gameData.start = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
