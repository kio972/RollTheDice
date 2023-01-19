using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause instance;
    public bool pause = false;

    [SerializeField]
    Transform imgs;
    [SerializeField]
    Button continueBtn;
    [SerializeField]
    Button settingBtn;
    [SerializeField]
    Button exitBtn;

    private bool settingOpen;


    private void Exit()
    {
        Application.Quit();
    }

    private void Setting()
    {
        SettingManager.Instance.Open();
        settingOpen = true;
    }

    private void Continue()
    {
        Time.timeScale = GameSpeedController.SetSpeed(SaveManager.Instance.settingData.gameSpeed);
        imgs.gameObject.SetActive(false);
        pause = false;
    }

    private void PauseExit()
    {
        if(settingOpen)
        {
            settingOpen = false;

            if (SettingManager.Instance.open == true)
            {
                SettingManager.Instance.Close();
                return;
            }
        }

        Continue();
    }

    private void PauseOn()
    {
        Time.timeScale = 0.0f;
        imgs.gameObject.SetActive(true);
        pause = true;
    }

    private void PauseToggle()
    {
        if (pause)
            PauseExit();
        else
            PauseOn();
    }

    // Start is called before the first frame update
    void Awake()
    {
        Pause[] pause = FindObjectsOfType<Pause>();
        if (pause.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            if (continueBtn != null)
                continueBtn.onClick.AddListener(Continue);
            if (settingBtn != null)
                settingBtn.onClick.AddListener(Setting);
            if (exitBtn != null)
                exitBtn.onClick.AddListener(Exit);

            Continue();
            DontDestroyOnLoad(this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseToggle();
    }
}
