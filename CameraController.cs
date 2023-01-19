using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField]
    private float minWidth = -1.2f;
    [SerializeField]
    private float maxWidth = 1.2f;
    [SerializeField]
    private float minHeight = -1f;
    [SerializeField]
    private float maxHeight = 1f;

    bool init = false;

    public bool move = true;

    public Coroutine following;

    //해상도 받아와야함
    private float isizeX = 1920;
    private float isizeY = 1080;
    private MousePointer mouseP;

    private float minSpeed = 1;
    private float maxSpeed = 3;
    private float speed = 1;
    private float mouseAccel = 2;

    private float cameraSize;
    private float wheelSpeed = 2f;
    private void ZoomInOut()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
        Camera.main.orthographicSize += scroll;

        if (Camera.main.orthographicSize <= 3)
            Camera.main.orthographicSize = 3;
        else if(Camera.main.orthographicSize > 6.5f)
            Camera.main.orthographicSize = 6.5f;
            
    }

    private Vector3 ReturnMaxRangePos(Vector3 fixedPos)
    {
        if (fixedPos.y > maxHeight)
            fixedPos.y = maxHeight;
        else if (fixedPos.y < minHeight)
            fixedPos.y = minHeight;

        if (fixedPos.x > maxWidth)
            fixedPos.x = maxWidth;
        else if (fixedPos.x < minWidth)
            fixedPos.x = minWidth;

        return fixedPos;
    }

    public void SetMaxSize()
    {
        List<Tile> tiles = GameManager.instance.mapController.tiles;
        minWidth = tiles[0].transform.position.x;
        maxWidth = tiles[tiles.Count - 1].transform.position.x;
        minHeight = tiles[GameManager.instance.mapController.mapColsize - 1].transform.position.y;
        maxHeight = tiles[(GameManager.instance.mapController.mapRowsize - 1) * GameManager.instance.mapController.mapColsize].transform.position.y - 1;
    }

    private void Init()
    {
        if(!init && GameManager.instance.mapController != null)
        {
            instance = this;
            SetMaxSize();

            init = true;

            mouseP = FindObjectOfType<MousePointer>();
        }
    }

    IEnumerator MoveToPos(Vector3 pos, float speed = 1.0f)
    {
        move = false;
        
        Vector3 nextPos = new Vector3(pos.x, pos.y, transform.position.z);
        nextPos = ReturnMaxRangePos(nextPos);

        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * speed);

            if ((transform.position - nextPos).magnitude < 0.1f)
                break;

            yield return null;
        }

        move = true;
        following = null;
        yield return null;
    }

    public void SetCameraPos(Vector3 pos, float speed = 1.0f)
    {
        if (following != null)
            StopCoroutine(following);

        following = StartCoroutine(MoveToPos(pos, speed));
    }

    public void CameraFollow(Transform targetPos)
    {
        if(following != null)
            StopCoroutine(following);

        following = StartCoroutine(FollowTarget(targetPos));
    }

    private IEnumerator FollowTarget(Transform targetPos)
    {
        move = false;
        
        while (GameManager.instance.moveing != null)
        {
            Vector3 pos = new Vector3(targetPos.position.x, targetPos.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
            yield return null;
        }

        Vector3 nextPos = transform.position;
        if (nextPos.y > maxHeight)
            nextPos.y = maxHeight;
        else if (nextPos.y < minHeight)
            nextPos.y = minHeight;

        if (nextPos.x > maxWidth)
            nextPos.x = maxWidth;
        else if (nextPos.x < minWidth)
            nextPos.x = minWidth;

        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime);

            if ((transform.position - nextPos).magnitude < 0.1f)
                break;

            yield return null;
        }

        move = true;
        following = null;
        yield return null;
    }

    public bool KeyMove()
    {
        Vector3 targetPos = transform.position;
        if (Input.GetKey(KeyCode.W))
            targetPos.y += 0.1f * minSpeed; 
        if (Input.GetKey(KeyCode.S))
            targetPos.y -= 0.1f * minSpeed;
        if (Input.GetKey(KeyCode.A))
            targetPos.x -= 0.1f * minSpeed;
        if (Input.GetKey(KeyCode.D))
            targetPos.x += 0.1f * minSpeed;

        if (transform.position != targetPos)
        {
            transform.position = ReturnMaxRangePos(targetPos);
            return true;
        }

        return false;
    }

    public void CameraMove()
    {
        if(mouseP != null)
        {
            Vector3 targetPos = transform.position;

            isizeX = Screen.width;
            isizeY = Screen.height;

            if(mouseP.PointerLocation.x < 5)
                targetPos.x -= 0.1f * speed;
            else if(mouseP.PointerLocation.x > isizeX - 5)
                targetPos.x += 0.1f * speed;

            if (mouseP.PointerLocation.y < 5)
                targetPos.y -= 0.1f * speed;
            else if(mouseP.PointerLocation.y > isizeY - 5)
                targetPos.y += 0.1f * speed;

            if (transform.position != targetPos)
            {
                speed += Time.deltaTime * mouseAccel;
                if (speed > maxSpeed)
                    speed = maxSpeed;
                transform.position = ReturnMaxRangePos(targetPos);
            }
            else
                speed = minSpeed;
        }
    }

    private bool WheelMove()
    {
        if(Input.GetKey(KeyCode.Mouse2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 targetPos = transform.position + new Vector3(-mouseX, -mouseY);
            transform.position = ReturnMaxRangePos(targetPos);
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        if(init && move)
        {
            if (GameManager.instance.phase == 1 || GameManager.instance.phase == 2 || GameManager.instance.phase == 3)
            {
                if (GameManager.instance.battleEnd)
                    return;

                if(!KeyMove())
                {
                    if(!WheelMove())
                        CameraMove();
                }
            }

            ZoomInOut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Init();
    }
}
