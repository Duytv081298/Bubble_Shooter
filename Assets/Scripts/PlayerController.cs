using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class PlayerController : MonoBehaviour
{

    [Header("List Prefabs bubbles:")]
    [SerializeField] private GameObject[] listBubbles;
    [SerializeField] private PlayerHalo PlayerHalo;

    private GameObject[] listPlayer = new GameObject[2];

    private Vector3 playerPos;
    private Vector3 temPos;

    private Vector2 screenSize;
    private GameObject player;
    private int[] listId = new int[2];
    [SerializeField] private float speed;
    private float totalTimeMove = 0f;

    [SerializeField] private Transform playerMain;
    [SerializeField] private Transform playerExtra;
    List<Vector3> listPosPlayerMove = new List<Vector3>();

    private List<int> ListIdUse;

    private bool rotate = true;
    private bool run = true;
    public bool isRotate()
    {
        return this.rotate;
    }
    public bool isRun()
    {
        return this.run;
    }
    public int[] getId()
    {
        return this.listId;
    }
    public GameObject[] getPlayer()
    {
        return this.listPlayer;
    }

    public void SetUp(List<int> ListIdUse)
    {
        this.ListIdUse = ListIdUse;
    }
    public void SetPlayer(Vector2 screenSize)
    {
        this.screenSize = screenSize;
        listId[0] = this.ListIdUse[Random.Range(0, this.ListIdUse.Count)];
        listId[1] = this.ListIdUse[Random.Range(0, this.ListIdUse.Count)];
        playerPos = new Vector3(0f, -screenSize.y * 0.255f, -0.0001f);
        temPos = new Vector3(0.78f, -screenSize.y * 0.337f, -0.0001f);
        this.listPlayer[0] = Instantiate(listBubbles[listId[0]], playerPos, Quaternion.identity, playerMain);
        this.listPlayer[1] = Instantiate(listBubbles[listId[1]], temPos, Quaternion.identity, playerExtra);

        player = this.listPlayer[0];
        PlayerHalo.SpawbHola(listId[0], playerPos.x, playerPos.y);
    }
    public void MoveEnd()
    {
        playerExtra.DORotate(new Vector3(0f, 0f, 128f), GameDefine.TIME_CHANGE_PLAYER, RotateMode.LocalAxisAdd)
        .OnComplete(() =>
        {
            System.Array.Reverse(listPlayer);
            System.Array.Reverse(listId);

            listPlayer[0].transform.SetParent(playerMain);

            PlayerHalo.DestroyHalo();
            PlayerHalo.SpawbHola(listId[0], playerPos.x, playerPos.y);

            listId[1] = this.ListIdUse[Random.Range(0, this.ListIdUse.Count)];
            this.listPlayer[1] = Instantiate(listBubbles[listId[1]], temPos, Quaternion.identity, playerExtra);

            player = this.listPlayer[0];
            run = true;
        });
    }
    public void ChangePlayer()
    {
        rotate = false;
        playerMain.DORotate(new Vector3(0f, 0f, 230f), GameDefine.TIME_CHANGE_PLAYER, RotateMode.LocalAxisAdd);
        playerExtra.DORotate(new Vector3(0f, 0f, 128f), GameDefine.TIME_CHANGE_PLAYER, RotateMode.LocalAxisAdd)
        .OnComplete(CompleteChangePlayer);
        System.Array.Reverse(listPlayer);
        System.Array.Reverse(listId);
    }
    private void CompleteChangePlayer()
    {
        listPlayer[0].transform.SetParent(playerMain);
        listPlayer[1].transform.SetParent(playerExtra);
        listPlayer[0].transform.position = playerPos;
        listPlayer[1].transform.position = temPos;
        PlayerHalo.DestroyHalo();
        PlayerHalo.SpawbHola(listId[0], playerPos.x, playerPos.y);

        player = this.listPlayer[0];
        rotate = true;
    }
    public IEnumerator Move(List<Vector3> listPos)
    {
        Vector2[] timeMove = GetTimeMove(listPos);
        run = false;
        listPosPlayerMove = listPos;
        for (int i = 0; i < listPos.Count; i++)
        {
            if (i == listPos.Count - 1)
                LeanTween.move(player, listPos[i], timeMove[i].x)
                       .setDelay(timeMove[i].y)
                       .setEase(LeanTweenType.linear)
                       .setOnComplete(MoveEnd);
            else
                LeanTween.move(player, listPos[i], timeMove[i].x)
                       .setDelay(timeMove[i].y)
                       .setEase(LeanTweenType.linear);
        }
        // Sequence sequence = DOTween.Sequence();
        // for (int i = 0; i < listPos.Count; i++)
        // {
        //     sequence.Append(player.transform.DOMove(listPos[i], time[i].x));
        // }
        // sequence.AppendCallback(MoveEnd);
        yield return new WaitForSeconds(totalTimeMove);
    }
    public Vector2[] GetTimeMove(List<Vector3> listPos)
    {
        Vector2[] listTime = new Vector2[listPos.Count];
        float totalTime = 0f;
        for (int i = 0; i < listPos.Count; i++)
        {
            float temp = i == 0 ? Vector2.Distance(playerPos, listPos[i]) / speed : Vector2.Distance(listPos[i - 1], listPos[i]) / speed;
            listTime[i] = new Vector2(temp, totalTime);
            totalTime += temp;
        }
        totalTimeMove = totalTime;
        return listTime;
    }
}
