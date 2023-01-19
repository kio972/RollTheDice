using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    static SettingManager instance;
    public static SettingManager Instance
    {
        get
        {
            if (!instance)
            {
                SettingManager manger = Resources.Load<SettingManager>("Prefab/SettingManager");
                manger = Instantiate(manger);
                manger.Init();
                instance = manger;
                DontDestroyOnLoad(manger);
            }
            return instance;
        }
    }

    VolumeController volumeController;
    GameSpeedController gameSpeedController;
    Transform settingZone;
    Button closeBtn;
    public bool open = true;

    public void Open()
    {
        open = true;
        settingZone.gameObject.SetActive(true);
    }

    public void Close()
    {
        open = false;
        settingZone.gameObject.SetActive(false);
    }

    void Init()
    {
        volumeController = GetComponentInChildren<VolumeController>();
        volumeController.Init();

        gameSpeedController = GetComponentInChildren<GameSpeedController>();
        gameSpeedController.Init();

        settingZone = transform.Find("Setting");

        closeBtn = UtillHelper.GetComponent<Button>("Close", settingZone.transform);
        closeBtn.onClick.AddListener(Close);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
