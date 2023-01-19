using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapEditor : MonoBehaviour
{

    public float xDist;
    public float yDist;

    Transform mapSizer;
    TextMeshProUGUI mapSize;
    Button bakeBtn;
    public int rowSize = 0;
    public int colSize = 0;
    List<Tile> tiles = new List<Tile>();

    Button compliteBtn;

    [SerializeField]
    LayerMask layer;

    Transform terrain;
    Tile tile;
    Transform curTilePos;

    [SerializeField]
    private Tile_Type curTile;

    Button noneBtn;
    Button snowBtn;
    Button grassBtn;
    Button sandBtn;
    Button blueBtn;
    Button redBtn;
    Button btn;

    private void Complite()
    {
        if (tiles == null)
            return;
        if (tiles.Count == 0)
            return;

        foreach (Tile tile in tiles)
        {
            Transform guide = tile.transform.Find("MapTile/Guide");
            guide.gameObject.SetActive(true);

            if (tile.onTarget == OnTile.Empty)
            {
                SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
                sprite.color = Color.clear;
            }
        }

        GameObject map = GameObject.Find("Map");
        map.AddComponent<MapController2>();
    }

    private void SetCurTile()
    {
        Transform temp = curTilePos.Find("Now");
        Destroy(temp.gameObject);
        
        switch (curTile)
        {
            case Tile_Type.None:
                btn = Instantiate(noneBtn, curTilePos);
                break;
            case Tile_Type.Grass:
                btn = Instantiate(grassBtn, curTilePos);
                break;
            case Tile_Type.Snow:
                btn = Instantiate(snowBtn, curTilePos);
                break;
            case Tile_Type.Sand:
                btn = Instantiate(sandBtn, curTilePos);
                break;
            case Tile_Type.Red:
                btn = Instantiate(redBtn, curTilePos);
                break;
            case Tile_Type.Blue:
                btn = Instantiate(blueBtn, curTilePos);
                break;
        }
        btn.name = "Now";
        Button tempBtn = btn.GetComponent<Button>();
        Destroy(tempBtn);
    }

    private void SetNone()
    {
        curTile = Tile_Type.None;
        SetCurTile();
    }

    private void SetSnow()
    {
        curTile = Tile_Type.Snow;
        SetCurTile();
    }

    private void SetGrass()
    {
        curTile = Tile_Type.Grass;
        SetCurTile();
    }

    private void SetSand()
    {
        curTile = Tile_Type.Sand;
        SetCurTile();
    }

    private void SetRed()
    {
        curTile = Tile_Type.Red;
        SetCurTile();
    }

    private void SetBlue()
    {
        curTile = Tile_Type.Blue;
        SetCurTile();
    }

    private void DrawTerrain()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, layer))
        {
            Tile tile = hit.transform.GetComponentInParent<Tile>();
            if(tile != null && tile.type != curTile)
            {

                SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
                switch (curTile)
                {
                    case Tile_Type.None:
                        tile.canMove = false;
                        tile.onTarget = OnTile.Empty;
                        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
                        break;
                    case Tile_Type.Grass:
                        tile.canMove = true;
                        sprite.color = Color.white;
                        sprite.sprite = Resources.Load<Sprite>("Img/Tile/grass");
                        break;
                    case Tile_Type.Snow:
                        tile.canMove = true;
                        sprite.color = Color.white;
                        sprite.sprite = Resources.Load<Sprite>("Img/Tile/snow");
                        break;
                    case Tile_Type.Sand:
                        tile.canMove = true;
                        sprite.color = Color.white;
                        sprite.sprite = Resources.Load<Sprite>("Img/Tile/sand");
                        break;
                    case Tile_Type.Red:
                        tile.canMove = true;
                        sprite.color = Color.white;
                        sprite.sprite = Resources.Load<Sprite>("Img/Tile/redTile");
                        break;
                    case Tile_Type.Blue:
                        tile.canMove = true;
                        sprite.color = Color.white;
                        sprite.sprite = Resources.Load<Sprite>("Img/Tile/blueTile");
                        break;
                }

                tile.type = curTile;
            }
        }
    }

    private void MapSize_Bake()
    {
        if(rowSize != 0 && colSize != 0)
        {
            GameObject map = GameObject.Find("Map");
            if(map != null)
                Destroy(map);
            
            map = new GameObject("Map");

            tiles = new List<Tile>();
            for (int i = 0; i < rowSize; i++)
            {
                GameObject temp = new GameObject();
                temp.name = "Row" + i.ToString();
                temp.transform.parent = map.transform;

                for (int j = 0; j < colSize; j++)
                {
                    Tile tileTemp = Instantiate(tile, map.transform);
                    tileTemp.name = "Col" + j.ToString();
                    tileTemp.transform.position = new Vector3(xDist * j + xDist * i, -yDist * j + yDist * i, 0);
                    tileTemp.transform.parent = temp.transform;
                    SpriteRenderer order = tileTemp.GetComponentInChildren<SpriteRenderer>();
                    order.sortingOrder = j - i - 20;
                    tileTemp.type = Tile_Type.Grass;
                    Transform guide = tileTemp.transform.Find("MapTile/Guide");
                    guide.gameObject.SetActive(false);
                    tiles.Add(tileTemp);
                }
            }
            int index = (int)(rowSize / 2) * colSize + (int)(colSize / 2);
            Camera.main.transform.position = new Vector3(tiles[index].transform.position.x, tiles[index].transform.position.y, -10);
        }
        else
        {
            print("MapSize cannot be 0");
        }
    }

    private void CameraMove()
    {
        if (Input.GetKey(KeyCode.A))
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - Time.deltaTime * 3f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        if (Input.GetKey(KeyCode.D))
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Time.deltaTime * 3f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        if (Input.GetKey(KeyCode.W))
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Time.deltaTime * 3f, Camera.main.transform.position.z);
        if (Input.GetKey(KeyCode.S))
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - Time.deltaTime * 3f, Camera.main.transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        mapSizer = transform.Find("MapSizer");
        if(mapSizer != null)
        {
            mapSize = UtillHelper.GetComponent<TextMeshProUGUI>("InputZone", mapSizer);
            bakeBtn = UtillHelper.GetComponent<Button>("BakeBtn", mapSizer);
            bakeBtn.onClick.AddListener(MapSize_Bake);
            compliteBtn = UtillHelper.GetComponent<Button>("CompliteBtn", mapSizer);
            compliteBtn.onClick.AddListener(Complite);
        }

        terrain = transform.Find("Terrain");
        if (terrain != null)
        {
            curTilePos = terrain.Find("CurTile");
            noneBtn = UtillHelper.GetComponent<Button>("Tiles/None", terrain);
            noneBtn.onClick.AddListener(SetNone);
            grassBtn = UtillHelper.GetComponent<Button>("Tiles/Grass", terrain);
            grassBtn.onClick.AddListener(SetGrass);
            snowBtn = UtillHelper.GetComponent<Button>("Tiles/Snow", terrain);
            snowBtn.onClick.AddListener(SetSnow);
            sandBtn = UtillHelper.GetComponent<Button>("Tiles/Sand", terrain);
            sandBtn.onClick.AddListener(SetSand);
            redBtn = UtillHelper.GetComponent<Button>("Tiles/Red", terrain);
            redBtn.onClick.AddListener(SetRed);
            blueBtn = UtillHelper.GetComponent<Button>("Tiles/Blue", terrain);
            blueBtn.onClick.AddListener(SetBlue);
        }

        tile = Resources.Load<Tile>("Prefab/MapTile");

        
    }

    // Update is called once per frame
    void Update()
    {
        mapSize.text = rowSize.ToString() + " x " + colSize.ToString();
        CameraMove();

        if (Input.GetKey(KeyCode.Mouse0))
            DrawTerrain();
    }
}
