using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    SceneMoveBtn newGame;
    SceneMoveBtn continueGame;
    Button setting;
    Button quitGame;

    public AudioClip backBGM;

    private void ExitGame()
    {
        Application.Quit();
    }

    private void Setting()
    {
        SettingManager.Instance.Open();
    }

    private void NewGame()
    {
        SaveManager.Instance.gameData = new SaveFile();
        SaveManager.Instance.LoadFile();
    }

    void Awake()
    {
        Transform btns = transform.Find("Btns");
        if (btns == null)
            return;

        newGame = UtillHelper.GetComponent<SceneMoveBtn>("NewGame", btns);
        if (newGame != null)
        {
            Button btn = newGame.GetComponent<Button>();
            btn.onClick.AddListener(NewGame);
            newGame.sceneName = "StartScene";
        }
        
        continueGame = UtillHelper.GetComponent<SceneMoveBtn>("Continue", btns);
        continueGame.gameObject.SetActive(false);
        if(File.Exists(Application.persistentDataPath + SaveManager.Instance.saveFileName))
        {
            SaveManager.Instance.LoadGameData();
            continueGame.gameObject.SetActive(true);
            continueGame.sceneName = SaveManager.Instance.gameData.sceneName;
        }
        if(!SaveManager.Instance.gameData.start)
            continueGame.gameObject.SetActive(false);

        setting = UtillHelper.GetComponent<Button>("Setting", btns);
        if(setting != null)
            setting.onClick.AddListener(Setting);

        quitGame = UtillHelper.GetComponent<Button>("Exit", btns);
        if (quitGame != null)
            quitGame.onClick.AddListener(ExitGame);

        FadeOut fade = FindObjectOfType<FadeOut>();
        if (fade != null)
            fade.FadeI();

        AudioManager.Instance.PlayBG(backBGM);
    }

}
