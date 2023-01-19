using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMoveBtn : MonoBehaviour
{
    Button button;
    public string sceneName;

    private void MoveScene()
    {
        if(sceneName != null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                GameData.SaveData(player);
            }

            GameData.sceneName = sceneName;
            SaveManager.Instance.SaveGameData();
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

    public void Move()
    {
        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if (fade != null)
            fade.FadeO();
        Invoke("MoveScene", 1.0f);
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        if(button != null)
            button.onClick.AddListener(Move);
    }
}
