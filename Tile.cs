using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Tile_Type
{
    None,
    Grass,
    Snow,
    Sand,
    Red,
    Blue,
}

public enum OnTile
{
    None,
    Player,
    Enemy,
    Object,
    Empty,
}

public class Tile : MonoBehaviour
{
    public Tile_Type type;

    public int rowIndex;
    public int colIndex;
    public int tileIndex;

    // a* 길찾기용 타일
    public Tile prevTile;


    private Button btn;

    public Color defaultColor;
    public Image image;

    public bool inRange = false;
    public bool canMove = true;

    public OnTile onTarget = OnTile.None;

    public bool guideOn = false;

    public SpriteRenderer[] guides;
    // 가이드
    // 0 left
    // 1 right
    // 2 up
    // 3 down
    // 4 a
    // 5 s
    // 6 d
    // 7 w

    public void TileSideEffect(string effectAddress, int effectId)
    {
        EffectZone effect = transform.GetComponentInChildren<EffectZone>(true);
        if(effect != null && effect.EffectId == effectId)
        {
            if (!effect.gameObject.activeSelf)
                effect.gameObject.SetActive(true);
            effect.Init();
            return;
        }

        effect = Resources.Load<EffectZone>(effectAddress);
        if (effect == null)
            return;
        
        effect = Instantiate(effect, transform);
        effect.Init();
    }

    public void ChangeTile(Tile_Type type)
    {
        this.type = type;
        Transform temp = transform.Find("MapTile");
        SpriteRenderer sprite = temp.GetComponent<SpriteRenderer>();
        switch (type)
        {
            case Tile_Type.None:
                canMove = false;
                onTarget = OnTile.Empty;
                sprite.color = Color.clear;
                break;
            case Tile_Type.Grass:
                canMove = true;
                onTarget = OnTile.None;
                sprite.color = Color.white;
                sprite.sprite = Resources.Load<Sprite>("Img/Tile/grass");
                break;
            case Tile_Type.Snow:
                canMove = true;
                onTarget = OnTile.None;
                sprite.color = Color.white;
                sprite.sprite = Resources.Load<Sprite>("Img/Tile/snow");
                break;
            case Tile_Type.Sand:
                canMove = true;
                onTarget = OnTile.None;
                sprite.color = Color.white;
                sprite.sprite = Resources.Load<Sprite>("Img/Tile/sand");
                break;
        }
        GameManager.instance.mapController.UpdateOnTargetTiles();
    }    

    public void ResetGuide()
    {
        if(guides != null)
        {
            foreach(SpriteRenderer guide in guides)
            {
                guide.gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        if (!GameManager.instance.attackInput)
        {
            GameManager.instance.attackInput = true;
            GameManager.instance.attackTargetRow = rowIndex;
            GameManager.instance.attackTargetCol = colIndex;
        }
        else
        {
            // 현재 공격스킬의 범위를 받아서 해당타일 위에 적이 있는지 확인하고 있으면 데미지
            print("공격처리");
            GameManager.instance.mapController.curTile = this;
            if (GameManager.instance.skillManager.currSkill.skillInfo.type == AttackType.Basic)
            {
                foreach (Tile tile in GameManager.instance.mapController.tiles)
                {
                    tile.inRange = false;
                }

                inRange = true;
            }

            GameManager.instance.skillManager.currSkill.Skill();
        }
    }

    public void DestroyGuide()
    {
        if (guideOn)
        {
            if (GameManager.instance.mapController.pathIndexs != null)
            {
                if (GameManager.instance.mapController.pathIndexs.Count > 0)
                {
                    GameManager.instance.mapController.ResetGuide();
                    //TileGuide1.instance.DestroyGuide();
                    if (MousePointer.instance != null && SkillManager.instance.currSkill == null)
                        MousePointer.instance.mouseType = MouseType.Normal;
                }
            }
            guideOn = false;
        }
    }

    public void DrawGuide()
    {
        if (GameManager.instance.battleEnd)
            return;

        int playerRow = GameManager.instance.player.curRow;
        int playerCol = GameManager.instance.player.curCol;
        int playerIndex = GameManager.instance.mapController.ReturnIndex(playerRow, playerCol);
        GameManager.instance.mapController.FindPath(playerIndex, tileIndex);

        if (GameManager.instance.mapController.pathIndexs != null && GameManager.instance.mapController.pathIndexs.Count > 0)
        {
            bool haveCost = false;
            if (GameManager.instance.player.stack >= GameManager.instance.mapController.pathCost)
            {
                haveCost = true;
                if (MousePointer.instance != null)
                    MousePointer.instance.mouseType = MouseType.Move;
            }
            else if (MousePointer.instance != null)
                MousePointer.instance.mouseType = MouseType.NoPath;

            //TileGuide1.instance.DrawGuide(haveCost);
            GameManager.instance.mapController.SetGuide(haveCost);
            GameManager.instance.uiManager.UpdateCharDir();
            guideOn = true;
        }
    }

    private void OnClickButton()
    {
        if (GameManager.instance.phase == 2 && GameManager.instance.moveing == null && canMove)
        {
            int playerRow = GameManager.instance.player.curRow;
            int playerCol = GameManager.instance.player.curCol;
            int playerIndex = GameManager.instance.uiManager.mapController.ReturnIndex(playerRow, playerCol);
            GameManager.instance.uiManager.mapController.FindPath(playerIndex, tileIndex);
            if (GameManager.instance.uiManager.mapController.pathIndexs != null)
            {
                float cost = GameManager.instance.uiManager.mapController.pathCost;
                if (GameManager.instance.player.stack >= cost)
                {
                    // 이동처리
                    GameManager.instance.moveInput = true;
                    GameManager.instance.player.stack -= cost;
                    TileGuide.instance.DestroyGuide();
                }
                else
                {
                    //이동불가 출력 (코스트가 부족합니다)
                }
            }
            else
            {
                // 도착지까지의 길이 없습니다
            }
            
        }
        else if (GameManager.instance.phase == 3 && inRange)
        {
            if (!GameManager.instance.attackInput)
            {
                GameManager.instance.attackInput = true;
                GameManager.instance.attackTargetRow = rowIndex;
                GameManager.instance.attackTargetCol = colIndex;
            }
            else
            {
                // 현재 공격스킬의 범위를 받아서 해당타일 위에 적이 있는지 확인하고 있으면 데미지
                print("공격처리");
                MapController.instance.curTile = this;
                if (GameManager.instance.skillManager.currSkill.skillInfo.type == AttackType.Basic)
                {
                    foreach(Tile tile in MapController.instance.tiles)
                    {
                        tile.inRange = false;
                    }

                    inRange = true;
                }
                
                GameManager.instance.skillManager.currSkill.Skill();
            }
        }
    }

    private void OnPointerEnter(PointerEventData data)
    {
        if (GameManager.instance.phase == 2 && GameManager.instance.moveing == null && canMove)
        {
            int playerRow = GameManager.instance.player.curRow;
            int playerCol = GameManager.instance.player.curCol;
            int playerIndex = GameManager.instance.uiManager.mapController.ReturnIndex(playerRow, playerCol);
            GameManager.instance.uiManager.mapController.FindPath(playerIndex, tileIndex);

            if (GameManager.instance.uiManager.mapController.pathIndexs != null)
            {
                bool haveCost = false;
                if (GameManager.instance.player.stack >= GameManager.instance.uiManager.mapController.pathCost)
                {
                    haveCost = true;
                    if (MousePointer.instance != null)
                        MousePointer.instance.mouseType = MouseType.Move;
                }
                else if (MousePointer.instance != null)
                    MousePointer.instance.mouseType = MouseType.NoPath;
                    
                TileGuide.instance.DrawGuide(haveCost);
                GameManager.instance.uiManager.UpdateCharDir();
            }
        }

    }

    private void OnPointerExit(PointerEventData data)
    {
        if (GameManager.instance.uiManager.mapController.pathIndexs != null)
        {
            if (GameManager.instance.uiManager.mapController.pathIndexs.Count > 0)
            {
                TileGuide.instance.DestroyGuide();
                if (MousePointer.instance != null && SkillManager.instance.currSkill == null)
                    MousePointer.instance.mouseType = MouseType.Normal;
            }
        }
    }

    public void Init()
    {
        btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(OnClickButton);

            image = GetComponent<Image>();
            defaultColor = image.color;

            EventTrigger trigger = GetComponent<EventTrigger>();

            UtillHelper.SetEvent(trigger, EventTriggerType.PointerEnter, OnPointerEnter);
            UtillHelper.SetEvent(trigger, EventTriggerType.PointerExit, OnPointerExit);
        }

        //GetGuide();
        ResetGuide();
    }

}
