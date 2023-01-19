using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Quests curQuest;
    public BoardDesc boardUI;

    Button acceptBtn;

    //Quests[] quests;
    //퀘스트 종류, 클리어한퀘스트

    private bool CheckQuestAvail(int index)
    {
        foreach (int i in GameData.clearedQuest)
        {
            if (i == index)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator ChangeScene(string sceneName, float fadeTime = 2f)
    {
        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if(fade != null)
        {
            fade.gameObject.SetActive(true);
            fade.FadeO(fadeTime);
        }
       
        float elapsed = 0;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        PlayerController player = FindObjectOfType<PlayerController>();
        GameData.SaveData(player);
        SceneManager.LoadSceneAsync(sceneName);
    }

    private void GoToQuest()
    {
        //curQuest에 따라서 이동하게 처리예정
        if(curQuest != null)
        {
            string scene = "Quest" + curQuest.questInfo.questId.ToString();
            SceneMoveBtn btn = acceptBtn.GetComponent<SceneMoveBtn>();
            btn.sceneName = scene;
            btn.Move();
        }
        
    }

    private void SetQuest()
    {
        InstanceQuest();
        Quests quest = GetComponentInChildren<Quests>();
        if(quest != null)
        {
            quest.Init();
            boardUI.gameObject.SetActive(true);
            boardUI.SetQuest(quest);
            curQuest = quest;
        }
    }

    public void InstanceQuest()
    {
        int date = GameData.week;
        while(date > 5)
        {
            date = date - 5;
        }

        int count = 0;
        // 임무 단계에 따라 임무 생성하는 코드
        while(true)
        {
            count++;
            if (count > 10000)
            {
                print("조건에 맞는 퀘스트가 없습니다.");
                break;
            }

            if (DataManger.QuestData == null)
                return;

            int random = Random.Range(0, DataManger.QuestData.Count);

            // 퀘스트가 이미 클리어했거나 조건에맞는지 확인
            if (!CheckQuestAvail(random))
                continue;

            string type = DataManger.QuestData[random]["questType"];

            bool canbreak = false;
            switch(date)
            {
                case 1:
                    if(type == "Normal")
                        canbreak = true;
                    break;
                case 2:
                    if (type == "Normal")
                        canbreak = true;
                    break;
                case 3:
                    if (type == "Elite")
                        canbreak = true;
                    break;
                case 4:
                    if (type == "Normal")
                        canbreak = true;
                    break;
                case 5:
                    if (type == "Boss")
                        canbreak = true;
                    break;
                default:
                    if (type == "Normal")
                        canbreak = true;
                    break;
            }

            if (canbreak)
            {
                Quests quest = Resources.Load<Quests>("Prefab/Quest/Quest" + DataManger.QuestData[random]["questID"]);
                quest = Instantiate(quest, transform);
                break;
            }
        }
    }

    public void Init()
    {
        boardUI = FindObjectOfType<BoardDesc>(true);
        if(boardUI != null)
        {
            boardUI.Init();
            acceptBtn = boardUI.transform.Find("AcceptBtn").GetComponent<Button>();
            acceptBtn.onClick.AddListener(GoToQuest);
            SetQuest();
            boardUI.gameObject.SetActive(false);
        }
    }
}
