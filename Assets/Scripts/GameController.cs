using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;

    private int rowDefault;
    private int rowBubbleHide;
    private Vector2 screenSize;
    private GameObject[] listPlayer;
    private int[] listId;
    private Vector2 playerPos;
    private List<List<BubbleMaps>> bubbleMaps = new List<List<BubbleMaps>>();
    private List<Vector3> listPosPlayerMove = new List<Vector3>();
    [SerializeField] private BubbleController bubbleController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LineController lineController;
    private Vector2 mousePosMemory;
    [SerializeField] private float speed;

    private void Awake()
    {
        float camH = 2f * Camera.main.orthographicSize;
        float camW = camH * Camera.main.aspect;
        screenSize = new Vector2(camW, camH);
        // LeanTween.init(1600);

    }
    public void SpawbBubbleMaps()
    {
        for (int i = 0; i <= this.rowDefault + 1; i++)
        {
            int length = i % 2 != 0 || i == 0 ? 11 : 10;
            List<BubbleMaps> temp = new List<BubbleMaps>();
            for (int j = 0; j < length; j++)
            {
                BubbleMaps bubbleMap = new BubbleMaps(new Vector2(i, j), false);
                temp.Add(bubbleMap);
            }
            bubbleMaps.Add(temp);
        }
    }
    void Start()
    {
        Level level = gameHandler.getLevel();
        this.rowDefault = gameHandler.getRowDefault();
        this.rowBubbleHide = this.rowDefault - 11;
        Debug.Log("rowDefault: " + this.rowDefault);
        Debug.Log("rowBubbleHide: " + rowBubbleHide);
        SpawbBubbleMaps();

        bubbleController.SetUp(screenSize, bubbleMaps, rowDefault);
        bubbleMaps = bubbleController.setPosition(level.bubbles);
        // Debug.Log(bubbleController.getMaxRow());

        playerController.SetPlayer(screenSize);
        listId = playerController.getId();
        listPlayer = playerController.getPlayer();
        playerPos = listPlayer[0].transform.position;

        lineController.SetUp(playerPos, screenSize, bubbleMaps);

        // Debug.Log(JsonUtility.ToJson(gameHandler.getLevel()));
    }
    public void nextLevel()
    {
        gameHandler.nextLevel();
        Debug.Log(JsonUtility.ToJson(gameHandler.getLevel()));

    }
    public void previousLevel()
    {
        gameHandler.previousLevel();
        Debug.Log(JsonUtility.ToJson(gameHandler.getLevel()));

    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = new Vector2(mousePos.x, mousePos.y);

        // if (Input.GetMouseButton(0))
        // {
        //     if (HasMouseMoved() && mousePosition.y > playerPos.y + 0.5)
        //     {
        //         listId = playerController.getId();
        //         lineController.setIdDot(listId[0]);
        //         lineController.setBubbleMaps(bubbleMaps);
        //         listPosPlayerMove = lineController.DrawPoints(mousePosition);
        //     }
        //     else if (mousePosition.y < playerPos.y + 0.5)
        //     {
        //         lineController.DestroyAllDots();
        //     }
        //     else lineController.MoveDots();
        // }

        if (Input.GetMouseButtonDown(0))
        {
            // if (mousePosition.y > playerPos.y + 0.5 && playerController.isRun())
            // {
            //     listId = playerController.getId();
            //     lineController.setIdDot(listId[0]);
            //     listPosPlayerMove = lineController.DrawPoints(mousePosition);
            // }
            // else if (playerController.isRotate() && playerController.isRun())
            // {
            //     lineController.DestroyAllDots();
            //     changPlayer();
            // }
            Vector2Int index = LocationToIndex(mousePosition);
            Debug.Log("index: " + index);
            // Debug.Log(bubbleMaps[index.y].Count);
            Debug.Log(JsonUtility.ToJson(bubbleMaps[index.y][index.x]));

            Vector2 loc = IndexToLocation(index);
            Debug.Log(loc);
            Debug.Log(loc.x);
            Debug.Log(loc.y);

        }
        // if (Input.GetMouseButtonUp(0))
        // {
        //     if (mousePosition.y > playerPos.y + 0.5 && playerController.isRun())
        //     {
        //         StartCoroutine(MovePlayer());
        //     }
        //     lineController.DestroyAllDots();
        // }


        // if (this.player)
        // {
        //     this.player.transform.position = new Vector3(mousePos.x, mousePos.y, -0.0001f);
        // }

    }
    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }
    private void changPlayer()
    {
        playerController.ChangePlayer();
        listId = playerController.getId();
        listPlayer = playerController.getPlayer();
        // playerController.Tween();
    }
    private IEnumerator MovePlayer()
    {
        Vector2 lastPos = listPosPlayerMove[listPosPlayerMove.Count - 1];
        Vector2Int index = LocationToIndex(lastPos);
        Debug.Log(index);
        Vector2 location = IndexToLocation(index);

        listPosPlayerMove.RemoveAt(listPosPlayerMove.Count - 1);
        listPosPlayerMove.Add(location);

        Vector2[] listDistance = new Vector2[listPosPlayerMove.Count];
        float total = 0f;
        for (int i = 0; i < listPosPlayerMove.Count; i++)
        {
            float temp = i == 0 ? Vector2.Distance(playerPos, listPosPlayerMove[i]) / speed : Vector2.Distance(listPosPlayerMove[i - 1], listPosPlayerMove[i]) / speed;
            listDistance[i] = new Vector2(temp, total);
            total += temp;
        }
        bubbleMaps[index.y][index.x].bubble = listPlayer[0];
        bubbleMaps[index.y][index.x].location = location;
        bubbleMaps[index.y][index.x].is_exist = true;
        bubbleMaps[index.y][index.x].color = listPlayer[0].tag;
        playerController.Move(listPosPlayerMove, listDistance);
        yield return new WaitForSeconds(total);
        PlayerMoveEnd(index, listPlayer[0].tag);

    }
    private void PlayerMoveEnd(Vector2Int index, string color)
    {
        List<Vector2Int> listBoom = new List<Vector2Int>();
        bubbleMaps[index.y][index.x].is_check = true;
        listBoom.Add(index);
        List<Vector2Int> booms = CheckBoom(index, color);
        listBoom.AddRange(booms);
        while (booms.Count > 0)
        {
            List<Vector2Int> temp = new List<Vector2Int>();
            for (int i = 0; i < booms.Count; i++)
            {
                temp.AddRange(CheckBoom(booms[i], color));
            }
            booms = temp;
            listBoom.AddRange(booms);
        }
        if (listBoom.Count > 1)
        {
            foreach (var item in listBoom)
            {
                RemoveBubble(item);
            }
        }
        TurnOffCheck();
    }
    private void RemoveBubble(Vector2Int index)
    {
        Destroy(bubbleMaps[index.y][index.x].bubble);
        bubbleMaps[index.y][index.x].bubble = null;
        bubbleMaps[index.y][index.x].location = Vector2.zero;
        bubbleMaps[index.y][index.x].is_exist = false;
        bubbleMaps[index.y][index.x].color = null;
        bubbleMaps[index.y][index.x].is_check = false;
        bubbleMaps[index.y][index.x].is_active = false;

    }
    private void TurnOffCheck()
    {
        for (int y = 0; y < bubbleMaps.Count; y++)
        {
            var temp = bubbleMaps[y];
            for (int x = 0; x < temp.Count; x++)
            {
                bubbleMaps[y][x].is_check = false;
            }
        }
    }
    private List<Vector2Int> CheckBoom(Vector2Int index, string color)
    {
        List<Vector2Int> listBoom = new List<Vector2Int>();
        Vector2 location = IndexToLocation(index);
        Vector2Int TL = LocationToIndex(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.25f, location.y + GameDefine.SIZE_BUBBLE.y * 0.6f));
        Vector2Int TR = LocationToIndex(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.25f, location.y + GameDefine.SIZE_BUBBLE.y * 0.6f));
        Vector2Int L = LocationToIndex(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.6f, location.y));
        Vector2Int R = LocationToIndex(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.6f, location.y));
        Vector2Int BL = LocationToIndex(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.25f, location.y - GameDefine.SIZE_BUBBLE.y * 0.6f));
        Vector2Int BR = LocationToIndex(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.25f, location.y - GameDefine.SIZE_BUBBLE.y * 0.6f));
        if (bubbleMaps[TL.y][TL.x].is_exist && !bubbleMaps[TL.y][TL.x].is_check)
        {
            bubbleMaps[TL.y][TL.x].is_check = true;
            if (bubbleMaps[TL.y][TL.x].color == color) listBoom.Add(TL);
            // listBoom.Add(TL);
        }
        if (bubbleMaps[TR.y][TR.x].is_exist && !bubbleMaps[TR.y][TR.x].is_check)
        {
            bubbleMaps[TR.y][TR.x].is_check = true;
            if (bubbleMaps[TR.y][TR.x].color == color) listBoom.Add(TR);
            // listBoom.Add(TR);
        }
        if (bubbleMaps[L.y][L.x].is_exist && !bubbleMaps[L.y][L.x].is_check)
        {
            bubbleMaps[L.y][L.x].is_check = true;
            if (bubbleMaps[L.y][L.x].color == color) listBoom.Add(L);
            // listBoom.Add(L);
        }
        if (bubbleMaps[R.y][R.x].is_exist && !bubbleMaps[R.y][R.x].is_check)
        {
            bubbleMaps[R.y][R.x].is_check = true;
            if (bubbleMaps[R.y][R.x].color == color) listBoom.Add(R);
            // listBoom.Add(R);
        }
        if (bubbleMaps[BL.y][BL.x].is_exist && !bubbleMaps[BL.y][BL.x].is_check)
        {
            bubbleMaps[BL.y][BL.x].is_check = true;
            if (bubbleMaps[BL.y][BL.x].color == color) listBoom.Add(BL);
            //  listBoom.Add(BL);
        }
        if (bubbleMaps[BR.y][BR.x].is_exist && !bubbleMaps[BR.y][BR.x].is_check)
        {
            bubbleMaps[BR.y][BR.x].is_check = true;
            if (bubbleMaps[BR.y][BR.x].color == color) listBoom.Add(BR);
            // listBoom.Add(BR);
        }
        return listBoom;
    }
    private Vector2Int LocationToIndex(Vector2 mousePos)
    {
        int count_Row_0 = 0;
        if (rowBubbleHide >= 1) count_Row_0 = bubbleMaps[rowBubbleHide - 1].Count;
        else count_Row_0 = bubbleMaps[0].Count;



        float posy = rowBubbleHide <= 1 ? mousePos.y + GameDefine.SIZE_TOP_BAR.y : mousePos.y;
        float x = 0;
        float y = Mathf.Floor((screenSize.y * 0.495f - posy) / GameDefine.HEIGHT_ROW);
        if (count_Row_0 == 11)
        {
            if (y % 2 == 0)
            {
                x = Mathf.Floor(mousePos.x + screenSize.x * 0.5f);
                if (x > 10) x = 10;
            }
            else
            {
                x = Mathf.Floor(mousePos.x + screenSize.x * 0.5f - GameDefine.SIZE_BUBBLE.x * 0.45f);
                if (x > 9) x = 9;
            }
        }
        else
        {
            if (y % 2 == 0)
            {
                x = Mathf.Floor(mousePos.x + screenSize.x * 0.5f - GameDefine.SIZE_BUBBLE.x * 0.45f);
                if (x > 9) x = 9;
            }
            else
            {
                x = Mathf.Floor(mousePos.x + screenSize.x * 0.5f);
                if (x > 10) x = 10;
            }
        }

        if (rowBubbleHide == 1) y -= rowBubbleHide;
        else if (rowBubbleHide <= 0) y -= 2;
        if (x < 0) x = 0;
        y += (rowBubbleHide - 1);
        if (y < 0) y = 0;

        return new Vector2Int((int)x, (int)y);
    }
    private Vector2 IndexToLocation(Vector2Int index)
    {

        int count_Row_0 = 0;
        if (rowBubbleHide >= 1) count_Row_0 = bubbleMaps[rowBubbleHide - 1].Count;
        else count_Row_0 = bubbleMaps[0].Count;
        float y = screenSize.y * 0.512f - index.y * GameDefine.HEIGHT_ROW;
        // if (rowBubbleHide <= 1) y -= GameDefine.HEIGHT_ROW*(2-rowBubbleHide);
        // if (rowBubbleHide == 0) y -= GameDefine.HEIGHT_ROW *2;
        // if (rowBubbleHide == 0) y -= GameDefine.HEIGHT_ROW *2;
        Debug.Log("0: " + screenSize.y * 0.512f );
        Debug.Log("2-rowBubbleHide: " + (float)(2-rowBubbleHide));
        Debug.Log(y);
        Vector2 pos;
        if (index.y % 2 == 0)
            pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x * 0.95f + index.x, y);
        else
            pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x / 2 + index.x, y);

        return pos;
    }

}
