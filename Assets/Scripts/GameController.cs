using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameHandler gameHandler;
    private float camW;
    private float camH;

    private GameObject player;
    // private int idPlayer = 0;

    private Vector3 playerPos;

    [SerializeField]
    private Collider2D boundaryLeftCol;

    [SerializeField]
    private Collider2D boundaryRightCol;

    List<RaycastHit2D> listHit = new List<RaycastHit2D>();

    [SerializeField]
    private BubbleController bubbleController;

    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private LineDrawer lineDrawer;


    private void Awake()
    {
        camH = 2f * Camera.main.orthographicSize;
        camW = camH * Camera.main.aspect;
        playerPos = new Vector3(0f, -camH * 0.255f, -0.0001f);

    }
    void Start()
    {
        // gameHandler = FindObjectOfType<GameHandler>();
        // bubbleController = FindObjectOfType<BubbleController>();
        Level level = gameHandler.getLevel();
        gameHandler.setLevel(level);

        // bubbleController.setPosition(level.bubbles);

        // playerController.setPlayer(idPlayer, playerPos);
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
        Vector3 direction = (mousePos - playerPos).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            // lineDrawer.Draw(playerPos, mousePos);

            // RaycastHit2D hit = Physics2D.Raycast(mousePos, playerPos);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, direction);
            // Debug.Log("direction: " +direction);
            Debug.Log(hit.transform.name);
            Debug.Log(hit.point);

            if (hit.collider != null)
            {
                var rayDir = Vector3.Reflect(((Vector3)hit.point - playerPos).normalized, hit.normal);
                // hit.collider.enabled = !hit.collider.enabled;

                RaycastHit2D hit1 = Physics2D.Raycast(hit.point, rayDir);
                if (hit1.collider != null)
                {
                    Debug.DrawLine(playerPos, hit.point, Color.yellow, 5.0f);
                    // Debug.DrawLine(hit.point, hit1.point, Color.red, 5.0f);
                    Debug.DrawRay(hit.point, rayDir, Color.blue, 5.0f);
                    // Debug.Log(hit1.point);
                    Debug.DrawLine(hit.point, hit1.point, Color.red, 5.0f);
                    // Debug.DrawRay(hit.point, rayDir, Color.red, 5.0f);
                }
            }


        }
        // if (this.player)
        // {
        //     this.player.transform.position = new Vector3(mousePos.x, mousePos.y, -0.0001f);
        // }

    }

    public int getHit(Vector3 mousePos, Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, direction);
        if (hit.collider != null)
        {
            listHit.Add(hit);
        }
        return 1;
    }
}
