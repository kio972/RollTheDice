using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    GraphicRaycaster raycaster;
    PointerEventData data;

    private Transform scoreZone;
    private ScoreList tapDance;
    private ScoreList speedGamer;
    private ScoreList monsterKill;
    private ScoreList clearNormal;
    private ScoreList clearElite;
    private ScoreList clearBoss;
    private ScoreList untouchable;
    private ScoreList collecter;

    private TextMeshProUGUI totalPoint;

    private Transform popUp;
    private TextMeshProUGUI popUpText;

    private Button titleBtn;

    private void MoveScene()
    {
        SceneManager.LoadSceneAsync("Title");
    }

    private void ReturnToTitle()
    {
        SaveManager.Instance.RemoveGameData();
        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if (fade != null)
            fade.FadeO();
        Invoke("MoveScene", 1.0f);
    }

    private void MouseCheck()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        
        data.position = Input.mousePosition;
        raycaster.Raycast(data, results);
        if (results.Count > 0)
        {
            ScoreList score = results[0].gameObject.transform.GetComponent<ScoreList>();
            if(score != null)
            {
                popUp.gameObject.SetActive(true);
                popUp.position = Input.mousePosition;
                popUpText.text = score.description;
                return;
            }
        }

        popUp.gameObject.SetActive(false);
    }

    private void SetScore()
    {
        int total = 0;
        int tapDancePoint = GameData.tapDanceCount * 10;
        tapDance.text.text = "탭댄스(" + GameData.tapDanceCount.ToString() + ") : " + tapDancePoint.ToString();
        total += tapDancePoint;
        if(tapDancePoint <= 0)
            tapDance.gameObject.SetActive(false);

        speedGamer.text.text = "스피드게이머 : " + GameData.speedGamer.ToString();
        total += GameData.speedGamer;
        if (GameData.speedGamer <= 0)
            speedGamer.gameObject.SetActive(false);

        int monsterKillPoint = GameData.monsterKillCount * 3;
        monsterKill.text.text = "몬스터 처치(" + GameData.monsterKillCount.ToString() + ") : " + monsterKillPoint.ToString();
        total += monsterKillPoint;
        if (monsterKillPoint <= 0)
            monsterKill.gameObject.SetActive(false);

        int clearNormalPoint = GameData.clearNormalCount * 5;
        clearNormal.text.text = "일반임무(" + GameData.clearNormalCount.ToString() + ") : " + clearNormalPoint.ToString();
        total += clearNormalPoint;
        if (clearNormalPoint <= 0)
            clearNormal.gameObject.SetActive(false);

        int clearElitePoint = GameData.clearEliteCount * 10;
        clearElite.text.text = "중급임무(" + GameData.clearEliteCount.ToString() + ") : " + clearElitePoint.ToString();
        total += clearElitePoint;
        if (clearElitePoint <= 0)
            clearElite.gameObject.SetActive(false);

        int clearBossPoint = GameData.clearBossCount * 30;
        clearBoss.text.text = "보스 처치(" + GameData.clearBossCount.ToString() + ") : " + clearBossPoint.ToString();
        total += clearBossPoint;
        if (clearBossPoint <= 0)
            clearBoss.gameObject.SetActive(false);

        int untouchablePoint = GameData.untouchableCount * 30;
        untouchable.text.text = "언터쳐블(" + GameData.untouchableCount.ToString() + ") : " + untouchablePoint.ToString();
        total += untouchablePoint;
        if (untouchablePoint <= 0)
            untouchable.gameObject.SetActive(false);

        if (GameData.playerRelics == null)
            GameData.playerRelics = new List<int>();
        int collecterPoint = GameData.playerRelics.Count * 10;
        collecter.text.text = "수집가(" + GameData.playerRelics.Count.ToString() + ") : " + collecterPoint.ToString();
        total += collecterPoint;
        if (collecterPoint <= 0)
            collecter.gameObject.SetActive(false);

        totalPoint.text = "Total : " + total.ToString() + "점";
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreZone = transform.Find("ScoreZone");
        tapDance = UtillHelper.GetComponent<ScoreList>("TapDance", scoreZone, true);
        tapDance.description = "일반 임무에서 피격당하지 않고 클리어";
        speedGamer = UtillHelper.GetComponent<ScoreList>("SpeedGamer", scoreZone, true);
        speedGamer.description = "임무 3턴이내 클리어(5점), 이후 턴당 -1점";
        monsterKill = UtillHelper.GetComponent<ScoreList>("MonsterKill", scoreZone, true);
        monsterKill.description = "처치한 몬스터";
        clearNormal = UtillHelper.GetComponent<ScoreList>("ClearNormal", scoreZone, true);
        clearNormal.description = "완료한 일반 임무";
        clearElite = UtillHelper.GetComponent<ScoreList>("ClearElite", scoreZone, true);
        clearElite.description = "완료한 중급 임무";
        clearBoss = UtillHelper.GetComponent<ScoreList>("ClearBoss", scoreZone, true);
        clearBoss.description = "보스 처치 회수";
        untouchable = UtillHelper.GetComponent<ScoreList>("Untouchable", scoreZone, true);
        untouchable.description = "보스에게 피격당하지 않고 클리어";
        collecter = UtillHelper.GetComponent<ScoreList>("Collecter", scoreZone, true);
        collecter.description = "획득한 유물의 수";

        totalPoint = UtillHelper.GetComponent<TextMeshProUGUI>("Total", transform);

        SetScore();

        popUp = transform.Find("PopUp");
        popUpText = popUp.GetComponentInChildren<TextMeshProUGUI>();

        raycaster = GetComponentInParent<GraphicRaycaster>();
        data = new PointerEventData(null);

        FadeOut fade = FindObjectOfType<FadeOut>();
        if (fade != null)
            fade.FadeI();

        titleBtn = FindObjectOfType<Button>();
        if (titleBtn != null)
            titleBtn.onClick.AddListener(ReturnToTitle);
    }

    // Update is called once per frame
    void Update()
    {
        MouseCheck();
    }
}
