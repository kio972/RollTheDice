using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;
    Image playerIllur;
    public HPUpdater hpBar;
    public PlayerRelic relic;
    public ItemPopUp popUp;

    // Start is called before the first frame update
    public void Init(PlayerController player)
    {
        instance = this;

        playerIllur = UtillHelper.GetComponent<Image>("PlayerIllur", transform);
        
        hpBar = GetComponentInChildren<HPUpdater>();
        if (hpBar != null)
            hpBar.Init(player);

        relic = GetComponentInChildren<PlayerRelic>();
        popUp = FindObjectOfType<ItemPopUp>();
        if (popUp != null)
            popUp.Init();
    }

}
