using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    private Vector2 screenSize;
    private GameObject[] listPlayer;
    private int[] listId;
    private Vector2 playerPos;
    private List<Vector3> listPosPlayerMove = new List<Vector3>();
    [SerializeField] private BubbleController bubbleController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LineController lineController;
    private Vector2 mousePosMemory;
    private Vector2 sizeBubble;
    [SerializeField] private float speed;



    private void Awake()
    {
        float camH = 2f * Camera.main.orthographicSize;
        float camW = camH * Camera.main.aspect;
        screenSize = new Vector2(camW, camH);
        LeanTween.init(1600);

    }
    void Start()
    {
        // gameHandler = FindObjectOfType<GameHandler>();
        // bubbleController = FindObjectOfType<BubbleController>();
        Level level = gameHandler.getLevel();
        gameHandler.setLevel(level);


        bubbleController.setPosition(level.bubbles);
        sizeBubble = bubbleController.getSize();

        playerController.setPlayer(screenSize);
        listId = playerController.getId();
        listPlayer = playerController.getPlayer();
        playerPos = listPlayer[0].transform.position;

        lineController.setStart(playerPos, screenSize);
        // this.player = playerController.getPlayer();
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

        if (Input.GetMouseButton(0))
        {
            if (mousePosition.y > playerPos.y + 0.5 && playerController.isRun())
            {
                if (mousePosMemory != mousePosition)
                {
                    listId = playerController.getId();
                    lineController.setIdDot(listId[0]);
                    listPosPlayerMove = lineController.DrawPoints(mousePosition);
                    mousePosMemory = mousePosition;
                }
                else
                {
                    lineController.MoveDots();
                }
            }
            else if (playerController.isRotate() && playerController.isRun())
            {
                lineController.DestroyAllDots();
                changPlayer();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (mousePosition.y > playerPos.y + 0.5 && playerController.isRun())
            {
                MovePlayer();
            }

            lineController.DestroyAllDots();

        }
        // if (this.player)
        // {
        //     this.player.transform.position = new Vector3(mousePos.x, mousePos.y, -0.0001f);
        // }

    }
    private void changPlayer()
    {
        playerController.changPlayer();
        listId = playerController.getId();
        listPlayer = playerController.getPlayer();
        // playerController.Tween();
    }
    private void MovePlayer()
    {
        Vector2 lastPos = listPosPlayerMove[listPosPlayerMove.Count - 1];
        Vector2 index = LocationToIndex(lastPos);
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
        playerController.Move(listPosPlayerMove, listDistance);
    }
    private Vector2 LocationToIndex(Vector2 mousePos)
    {
        float x = 0f;
        float y = Mathf.Floor(((screenSize.y) * 0.5f - mousePos.y) / 0.87f);
        if (y % 2 == 0)
        {
            x = Mathf.Floor(mousePos.x + screenSize.x * 0.5f - sizeBubble.x * 0.45f);
            if (x > 10) x = 10;
        }
        else
        {
            x = Mathf.Floor(mousePos.x + screenSize.x * 0.5f);
            if (x > 9) x = 10;
        }
        if (x < 0) x = 0;
        return new Vector2(x, y);
    }
    private Vector2 IndexToLocation(Vector2 index)
    {
        Vector2 pos;
        if (index.y % 2 == 0)
            pos = new Vector2(-screenSize.x * 0.5f + sizeBubble.x * 0.95f + index.x, (screenSize.y - sizeBubble.y) * 0.5f - index.y * 0.87f);
        else
            pos = new Vector2(-screenSize.x * 0.5f + sizeBubble.x / 2 + index.x, (screenSize.y - sizeBubble.y) * 0.5f - index.y * 0.87f);
        return pos;
    }
}
