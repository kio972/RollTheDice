using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : MonoBehaviour
{
    [SerializeField]
    private Bed bed;

    public Transform tavernUi;
    private RectTransform desc;

    private bool popUp = false;
    private float restFee = 30;

    public void UpdateInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Interact"))
            {
                Bed targetbed = hit.transform.GetComponent<Bed>();
                if (targetbed != null)
                {
                    if(targetbed.CanRest)
                    {
                        bed = targetbed;
                        popUp = true;

                        if (Input.GetKeyUp(KeyCode.Mouse0))
                        {
                            if (TownController.instance.player.gold >= restFee)
                            {
                                bed.Rest();
                                popUp = false;
                            }
                            else
                            {
                                TownController.instance.npc.SetNPCName("여관주인", "밥");
                                TownController.instance.npc.SetConv("돈이 부족하지않은가, 무료로 재워줄순 없네.");
                            }
                        }
                    }
                    else if(Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        TownController.instance.npc.SetNPCName("여관주인", "밥");
                        TownController.instance.npc.SetConv("하루종일 잠만 잘건가?");
                    }
                }
            }
        }
        else
        {
            if (bed != null)
            {
                desc.gameObject.SetActive(false);
                popUp = false;
                bed = null;
            }
        }
    }
    public void PopUP()
    {
        desc.gameObject.SetActive(true);
        Vector2 pos = Input.mousePosition;
        desc.transform.position = pos;
    }

    public void Init()
    {
        tavernUi = TownController.instance.canvas.transform.Find("TavernUI");
        desc = tavernUi.Find("Desc").GetComponent<RectTransform>();
        desc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        if (popUp)
        {
            PopUP();
        }
    }
}
