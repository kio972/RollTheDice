using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBag : MonoBehaviour
{

    public static ItemBag instance;
    public List<Items> itemGroup;
    public List<RectTransform> itemGroupTrans;

    private Animator animator;
    private Button bagBtn;
    private ItemPopUp popUpDesc;

    public ItemOnClickBtn onClickBtn;

    public bool interact;
    public Items curItem;

    public bool ispointerOn = false;
    private bool isOpen = false;

    private Image icon;

    public void OnClickBag()
    {
        animator.SetTrigger("Toggle");
        if(isOpen)
        {
            isOpen = false;
            interact = false;
            icon.sprite = Resources.Load<Sprite>("Img/BagClose");
            PopUpDescExit();
        }
        else
        {
            isOpen = true;
            icon.sprite = Resources.Load<Sprite>("Img/BagOpen");
        }
    }

    public void UpdateBag()
    {
        if (itemGroup == null)
            return;

        for(int i = 0; i < itemGroup.Count; i++)
        {
            itemGroup[i].transform.position = itemGroupTrans[i].transform.position;
        }
    }

    public void UseItemChoice()
    {
        interact = true;
        Vector2 pos = curItem.transform.position;
        if (pos.x > 1500)
            pos.x -= 245;
        else
            pos.x += 245;
        pos.y += 150;
        if (pos.y > 900)
            pos.y = 900;
        popUpDesc.gameObject.SetActive(true);
        popUpDesc.transform.position = pos;

        if (pos.x > 1500)
            pos = pos + new Vector2(45, -150); 
        else
            pos = pos + new Vector2(-45, -150);
        onClickBtn.gameObject.SetActive(true);
        onClickBtn.transform.position = pos;
    }

    public void PopUpDescExit()
    {
        popUpDesc.gameObject.SetActive(false);
        onClickBtn.gameObject.SetActive(false);
    }

    public void UpdateDescPos()
    {
        Vector2 pos = Input.mousePosition;
        if(pos.x > 1500)
            pos.x -= 200;
        else
            pos.x += 200;

        if (pos.y > 900)
            pos.y -= 100;
        else
            pos.y += 100;

        popUpDesc.transform.position = pos;
    }

    public void PopUpDesc(Items item)
    {
        popUpDesc.gameObject.SetActive(true);
        popUpDesc.UpdateInfo(item);
    }

    public void UpdateListPos()
    {
        itemGroupTrans = new List<RectTransform>();
        int count = 0;
        while (true)
        {
            Transform temp = transform.Find("Mask").Find("Bg").Find("Item" + count.ToString());
            if (temp != null)
            {
                RectTransform target = temp.GetComponent<RectTransform>();
                itemGroupTrans.Add(target);
                count++;
            }
            else
                break;
        }
    }

    public void SaveItem()
    {
        GameData.playerItems = new List<int>();
        foreach (Items item in itemGroup)
        {
            GameData.playerItems.Add(item.itemInfo.id);
        }
    }

    public void GetItem(int index)
    {
        if(index >= DataManger.ItemData.Count)
        {
            print("Wrong Index");
            return;
        }

        Items item = Resources.Load<Items>(DataManger.ItemData[index]["Path"]);
        if (item == null)
        {
            print("Wrong Index");
            return;
        }

        foreach(Transform trans in itemGroupTrans)
        {
            Items temp = trans.GetComponentInChildren<Items>();
            if(temp == null)
            {
                item = Instantiate(item, trans);
                item.Init();
                item.haveItem = true;
                break;
            }
        }
        UpdateBag();
    }

    private void LoadItems()
    {
        Items[] items = GetComponentsInChildren<Items>();
        foreach (Items item in items)
            Destroy(item.gameObject);

        if(GameData.playerItems != null && DataManger.ItemData != null)
        {
            for (int i = 0; i < GameData.playerItems.Count; i++)
            {
                int index = GameData.playerItems[i];
                Items item = Resources.Load<Items>(DataManger.ItemData[index]["Path"]);
                item = Instantiate(item, itemGroupTrans[i]);
                item.Init();
                item.haveItem = true;
                itemGroup.Add(item);
            }
        }
    }

    public void Init()
    {
        instance = this;
        itemGroup = new List<Items>();
        UpdateListPos();
        LoadItems();

        popUpDesc = GetComponentInChildren<ItemPopUp>(true);
        if(popUpDesc != null)
        {
            popUpDesc.Init();
            popUpDesc.gameObject.SetActive(false);
        }

        onClickBtn = GetComponentInChildren<ItemOnClickBtn>(true);
        if(onClickBtn != null)
        {
            onClickBtn.Init();
            onClickBtn.gameObject.SetActive(false);
        }

        bagBtn = transform.Find("Icon").GetComponent<Button>();
        bagBtn.onClick.AddListener(OnClickBag);
        animator = transform.Find("Mask").Find("Bg").GetComponent<Animator>();

        icon = UtillHelper.GetComponent<Image>("Icon", transform);
    }

    private void Update()
    {
        if(ispointerOn && !interact)
        {
            UpdateDescPos();
        }
    }
}
