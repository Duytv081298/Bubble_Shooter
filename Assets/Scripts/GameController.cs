using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int rowDefault;
    private int rowBubbleHide;
    private Vector2 screenSize;
    private GameObject[] listPlayer;
    private int[] listId;
    private Vector2 playerPos;
    private List<List<BubbleMaps>> bubbleMaps = new List<List<BubbleMaps>>();
    private List<Vector3> listPosPlayerMove = new List<Vector3>();
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private BubbleController bubbleController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LineController lineController;
    [SerializeField] private Event EventController;

    private void Awake()
    {
        float camH = 2f * Camera.main.orthographicSize;
        float camW = camH * Camera.main.aspect;
        screenSize = new Vector2(camW, camH);
        // LeanTween.init(1600);

    }
    public void SpawbBubbleMaps()
    {
        for (int i = 0; i <= this.rowDefault + 3; i++)
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
        // StartCoroutine();
        Init();
        // Debug.Log(JsonUtility.ToJson(gameHandler.getLevel()));
    }
    private void Init()

    {
        Level level = gameHandler.getLevel();
        this.rowDefault = gameHandler.getRowDefault();
        this.rowBubbleHide = this.rowDefault - 12;
        Debug.Log("rowDefault: " + this.rowDefault);
        Debug.Log("rowBubbleHide: " + rowBubbleHide);
        SpawbBubbleMaps();

        bubbleController.SetUp(screenSize, bubbleMaps, rowDefault);
        bubbleController.setPosition(level.bubbles);

        playerController.SetUp(bubbleController.GetListIdPlayer());
        playerController.SetPlayer(screenSize);
        bubbleMaps = bubbleController.getBubbleMaps();
        listId = playerController.getId();
        listPlayer = playerController.getPlayer();
        playerPos = listPlayer[0].transform.position;

        lineController.SetUp(playerPos, screenSize, bubbleMaps, rowBubbleHide);
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
        if (bubbleController.isPlay())
        {
            if (Input.GetMouseButton(0))
            {
                if (HasMouseMoved() && mousePosition.y > playerPos.y + 0.5)
                    DrawLine(mousePosition);
                else if (mousePosition.y < playerPos.y + 0.5)
                {
                    RemoveLine();
                }
                else lineController.MoveDots();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (mousePosition.y > playerPos.y + 0.5 && playerController.isRun())
                    DrawLine(mousePosition);
                else if (playerController.isRotate() && playerController.isRun())
                {
                    RemoveLine();
                    changPlayer();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (mousePosition.y > playerPos.y + 0.5 && playerController.isRun())
                {
                    StartCoroutine(MovePlayer());
                }
                RemoveLine();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                var index = bubbleController.LocationToIndex(mousePosition);
                Debug.Log("index: " + index);
            }
        }


    }

    public void Test()
    {
        Debug.Log("kích hoạt FG");
    }
    void DrawLine(Vector2 mousePosition)
    {
        listId = playerController.getId();
        rowBubbleHide = bubbleController.getRowBubbleHide();
        lineController.SetStart(listId[0], bubbleMaps, rowBubbleHide);
        listPosPlayerMove = lineController.DrawPoints(mousePosition);
    }
    void RemoveLine()
    {
        lineController.DestroyAllDots();
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
        listPosPlayerMove = bubbleController.StandardizePosition(listPosPlayerMove, listPlayer[0]);

        yield return StartCoroutine(playerController.Move(listPosPlayerMove));
        StartCoroutine(PlayerMoveEnd());
    }
    private IEnumerator PlayerMoveEnd()
    {
        bubbleController.setPlay(false);
        yield return StartCoroutine(bubbleController.MergedIntoPlayer());
        playerController.SetUp(bubbleController.GetListIdPlayer());
        // StartCoroutine(RemoBubbleDisconnect());
    }
    // private IEnumerator RemoBubbleDisconnect()
    // {
    //     List<Vector2Int> listDisconnect = bubbleController.GetBubbleDisconnect();
    //     Debug.Log(listDisconnect.Count);
    //     yield return StartCoroutine(bubbleController.RemoveBubbleDisconnect(listDisconnect));
    // }

}
