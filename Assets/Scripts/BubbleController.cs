using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    [Header("List Prefabs bubbles:")]
    [SerializeField] private GameObject[] listBubbles;
    private List<List<BubbleMaps>> bubbleMaps = new List<List<BubbleMaps>>();
    public List<List<BubbleMaps>> getBubbleMaps()
    {
        return this.bubbleMaps;
    }
    private Vector2 screenSize;
    private List<Vector2> listPos = new List<Vector2>();
    [SerializeField] private GameObject top_bar;
    private int rowDefault;
    private int rowBubbleHide;

    public int getRowBubbleHide()
    {
        return this.rowBubbleHide;
    }
    private GameObject player;
    private Vector2Int indexPlayerEnd;
    [SerializeField] private EffectsController effectsController;

    [Range(0.1f, 1f)]
    [SerializeField] private float thrust_force;
    private bool play = false;
    public bool isPlay()
    {
        return this.play;
    }
    public void setPlay(bool play)
    {
        this.play = play;
    }
    public void SetUp(Vector2 screenSize, List<List<BubbleMaps>> bubbleMaps, int rowDefault)
    {
        this.screenSize = screenSize;
        this.bubbleMaps = bubbleMaps;
        this.rowDefault = rowDefault;
        this.rowBubbleHide = rowDefault - 12;

    }
    public void setPosition(Bubble_level[] bubbles)
    {
        float constant = screenSize.y * 0.5f - GameDefine.SIZE_TOP_BAR.y + GameDefine.HEIGHT_ROW * 0.5f;

        foreach (Bubble_level bubble in bubbles)
        {
            GameObject bubbleTemplate;
            Vector2 pos;
            if (bubble.id == 100)
            {
                var bubbleM = bubbleMaps[bubble.y][bubble.x];
                pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x * 0.95f + bubble.x, screenSize.y * 0.5f - bubble.y * GameDefine.HEIGHT_ROW);
                bubbleM.location = pos;
                bubbleM.setExist(true);
                bubbleM.id = bubble.id;
                if (bubble.x == 0)
                {
                    GameObject topBar = Instantiate(top_bar, new Vector3(0f, screenSize.y * 0.5f, 0f), Quaternion.identity, this.transform);
                    bubbleM.setBubble(topBar);
                }
            }
            if (bubble.id < 8)
            {
                var bubbleM = bubbleMaps[bubble.y][bubble.x];
                float y = constant - bubble.y * GameDefine.HEIGHT_ROW;
                bubbleTemplate = listBubbles[bubble.id];
                if (bubble.y % 2 == 0)
                    pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x * 0.95f + bubble.x, y);
                else
                    pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x / 2 + bubble.x, y);
                listPos.Add(pos);

                GameObject bubbleCl = Instantiate(bubbleTemplate, pos, Quaternion.identity, this.transform);
                Collider2D col = bubbleCl.GetComponent<Collider2D>();
                col.enabled = true;
                bubbleM.setBubble(bubbleCl);
                bubbleM.location = pos;
                bubbleM.setExist(true);
                bubbleM.color = bubbleCl.tag;
                bubbleM.id = bubble.id;
            }
        }
        transform.position = new Vector3(0f, -2f, 0f);
        Move((rowBubbleHide - 1), true);
    }
    public Vector2 getMaxRow()
    {
        Vector2 maxRow = Vector2.zero;
        foreach (var row in bubbleMaps)
        {
            foreach (var item in row)
            {
                if (item.isExist()) maxRow = item.index;
            }
        }
        return maxRow;
    }
    private void Move(int num, bool is_play)
    {
        // float dow = rowBubbleHide <= 3 ? rowBubbleHide : 3 + (rowBubbleHide - 3) * GameDefine.HEIGHT_ROW;
        // Debug.Log(dow);
        if (rowDefault > 11)
        {
            transform.DOMoveY(num * GameDefine.HEIGHT_ROW, 1).SetDelay(1)
            .OnComplete(() =>
            {
                this.play = is_play;
                effectsController.StartAnimationDino_Owl();
            });
        }
    }
    public Vector3 getPosition()
    {
        // Debug.Log(transform.position);
        return transform.position;
    }
    public Vector3 GetLocation(Vector2 location)
    {
        return transform.InverseTransformPoint(location);
    }
    public Vector3 GetWorldLocation(Vector2 location)
    {
        return transform.TransformPoint(location);
    }
    public List<Vector2Int> GetlistBoom(Vector2Int index, string color)
    {
        List<Vector2Int> listBoom = new List<Vector2Int>();
        bubbleMaps[index.y][index.x].setCheck(true);
        // bubbleMaps[index.y][index.x].setExist(false);
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
        return listBoom;
    }
    private void logArray(List<Vector2Int> list)
    {
        var str = "";
        foreach (var item in list)
        {
            str += item;
        }
        Debug.Log(str);
    }
    private List<Vector2Int> CheckBoom(Vector2Int index, string color)
    {
        List<Vector2Int> listBoom = new List<Vector2Int>();
        Vector2 location = IndexToLocation(index);

        Vector2Int TL = LocationToIndex(GetWorldLocation(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.25f, location.y + GameDefine.SIZE_BUBBLE.y * 0.6f)));
        Vector2Int TR = LocationToIndex(GetWorldLocation(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.25f, location.y + GameDefine.SIZE_BUBBLE.y * 0.6f)));
        Vector2Int L = LocationToIndex(GetWorldLocation(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.6f, location.y)));
        Vector2Int R = LocationToIndex(GetWorldLocation(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.6f, location.y)));
        Vector2Int BL = LocationToIndex(GetWorldLocation(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.25f, location.y - GameDefine.SIZE_BUBBLE.y * 0.6f)));
        Vector2Int BR = LocationToIndex(GetWorldLocation(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.25f, location.y - GameDefine.SIZE_BUBBLE.y * 0.6f)));

        if (check(TL, color)) listBoom.Add(TL);
        if (check(TR, color)) listBoom.Add(TR);
        if (check(L, color)) listBoom.Add(L);
        if (check(R, color)) listBoom.Add(R);
        if (check(BL, color)) listBoom.Add(BL);
        if (check(BR, color)) listBoom.Add(BR);
        return listBoom;
    }
    private bool check(Vector2Int index, string color)
    {
        bool result = false;
        var bubbleM = bubbleMaps[index.y][index.x];
        if (bubbleM.isExist() && !bubbleM.isCheck())
        {
            bubbleM.setCheck(true);
            if (bubbleM.color == color)
            {
                result = true;
                // bubbleM.setExist(false);
            }
        }
        return result;
    }
    public List<Vector3> StandardizePosition(List<Vector3> listPosPlayerMove, GameObject player)
    {
        this.player = player;
        Vector2 lastPos = listPosPlayerMove[listPosPlayerMove.Count - 1];
        this.indexPlayerEnd = LocationToIndex(lastPos);
        Vector2 location = IndexToLocation(this.indexPlayerEnd);
        Vector2 newLoc = GetWorldLocation(location);

        listPosPlayerMove.RemoveAt(listPosPlayerMove.Count - 1);
        listPosPlayerMove.Add(newLoc);
        return listPosPlayerMove;
    }
    public IEnumerator MergedIntoPlayer()
    {
        bubbleMaps[this.indexPlayerEnd.y][this.indexPlayerEnd.x].setBubble(player);
        bubbleMaps[this.indexPlayerEnd.y][this.indexPlayerEnd.x].setExist(true);
        bubbleMaps[this.indexPlayerEnd.y][this.indexPlayerEnd.x].color = player.tag;
        var idColor = ColorToID(player.tag);
        player.transform.SetParent(transform);
        Collider2D col = player.GetComponent<Collider2D>();
        col.enabled = true;
        // StartCoroutine(ActiveVibrate());
        ActiveVibrate();
        List<Vector2Int> listBoom = GetlistBoom(this.indexPlayerEnd, player.tag);

        if (listBoom.Count > 2)
        {
            foreach (var index in listBoom)
            {
                RemoveBubble(index);
                // Debug.Log("index: " + index + "__loc: " + GetWorldLocation(IndexToLocation(index)));
                effectsController.PlayEffect(idColor, GetWorldLocation(IndexToLocation(index)));
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (listBoom.Count > 9)
        {

            effectsController.SetAnimation("Idle3");
        }

        TurnOffCheck();
        List<Vector2Int> listDisconnect = GetBubbleDisconnect();
        StartCoroutine(RemoveBubbleDisconnect(listDisconnect));
    }
    private void RemoveBubble(Vector2Int index)
    {
        // Debug.Log(index);
        var bubbleM = bubbleMaps[index.y][index.x];
        bubbleM.getBubble().transform.DOKill();
        Destroy(bubbleM.getBubble());
        bubbleM.Destroy();
    }
    public void ActiveVibrate()
    {
        // Debug.Log(1111111);
        List<List<Vector2Int>> listVibrate = GetlistVibrate(this.indexPlayerEnd);
        // Debug.Log(listVibrate.Count);
        for (int i = 0; i < listVibrate.Count; i++)
        {
            // logArray(listVibrate[i]);
            RunOneTurnVibrate(listVibrate[i], i);
        }
    }
    private void RunOneTurnVibrate(List<Vector2Int> listIndex, int turn)
    {
        var locationPlayer = GetWorldLocation(IndexToLocation(this.indexPlayerEnd));
        foreach (var index in listIndex)
        {
            var location = GetWorldLocation(IndexToLocation(index));
            Vector2 direction = (location - locationPlayer).normalized;
            Vector2 punch = direction * thrust_force;
            var bubbleM = bubbleMaps[index.y][index.x];
            var bubble = bubbleM.getBubble();
            bubble.transform.DOPunchPosition(punch, 1f, 3, 1f);
        }
    }
    public List<List<Vector2Int>> GetlistVibrate(Vector2Int index)
    {
        List<List<Vector2Int>> listVibrate = new List<List<Vector2Int>>();
        bubbleMaps[index.y][index.x].setCollision(true);
        // listVibrate.Add(index);
        List<Vector2Int> vibrates = CheckVibrate(index);

        listVibrate.Add(vibrates);
        for (int i = 0; i < 2; i++)
        {
            List<Vector2Int> temp = new List<Vector2Int>();
            foreach (var item in vibrates)
            {
                temp.AddRange(CheckVibrate(item));
            }
            vibrates = temp;
            listVibrate.Add(vibrates);
        }
        return listVibrate;
    }
    private List<Vector2Int> CheckVibrate(Vector2Int index)
    {
        List<Vector2Int> listCollision = new List<Vector2Int>();
        Vector2 location = IndexToLocation(index);

        Vector2Int TL = LocationToIndex(GetWorldLocation(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.25f, location.y + GameDefine.SIZE_BUBBLE.y * 0.6f)));
        Vector2Int TR = LocationToIndex(GetWorldLocation(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.25f, location.y + GameDefine.SIZE_BUBBLE.y * 0.6f)));
        Vector2Int L = LocationToIndex(GetWorldLocation(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.6f, location.y)));
        Vector2Int R = LocationToIndex(GetWorldLocation(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.6f, location.y)));
        Vector2Int BL = LocationToIndex(GetWorldLocation(new Vector2(location.x - GameDefine.SIZE_BUBBLE.x * 0.25f, location.y - GameDefine.SIZE_BUBBLE.y * 0.6f)));
        Vector2Int BR = LocationToIndex(GetWorldLocation(new Vector2(location.x + GameDefine.SIZE_BUBBLE.x * 0.25f, location.y - GameDefine.SIZE_BUBBLE.y * 0.6f)));

        // Debug.Log("TL: " + TL + "  TR: " + TR + "  L: " + L + "  R: " + R + "  BL: " + BL + "  BR:" + BR);
        if (bubbleMaps[TL.y][TL.x].isExist() && !bubbleMaps[TL.y][TL.x].isCollision())
        {
            bubbleMaps[TL.y][TL.x].setCollision(true);
            listCollision.Add(TL);
        }
        if (bubbleMaps[TR.y][TR.x].isExist() && !bubbleMaps[TR.y][TR.x].isCollision())
        {
            bubbleMaps[TR.y][TR.x].setCollision(true);
            listCollision.Add(TR);
        }
        if (bubbleMaps[L.y][L.x].isExist() && !bubbleMaps[L.y][L.x].isCollision())
        {
            bubbleMaps[L.y][L.x].setCollision(true);
            listCollision.Add(L);
        }
        if (bubbleMaps[R.y][R.x].isExist() && !bubbleMaps[R.y][R.x].isCollision())
        {
            bubbleMaps[R.y][R.x].setCollision(true);
            listCollision.Add(R);
        }
        if (bubbleMaps[BL.y][BL.x].isExist() && !bubbleMaps[BL.y][BL.x].isCollision())
        {
            bubbleMaps[BL.y][BL.x].setCollision(true);
            listCollision.Add(BL);
        }
        if (bubbleMaps[BR.y][BR.x].isExist() && !bubbleMaps[BR.y][BR.x].isCollision())
        {
            bubbleMaps[BR.y][BR.x].setCollision(true);
            listCollision.Add(BR);
        }
        return listCollision;
    }
    private void TurnOffCheck()
    {
        for (int y = 0; y < bubbleMaps.Count; y++)
        {
            var temp = bubbleMaps[y];
            for (int x = 0; x < temp.Count; x++)
            {
                bubbleMaps[y][x].setCheck(false);
                bubbleMaps[y][x].setConnection(false);
                bubbleMaps[y][x].setCollision(false);
            }
        }
    }
    public List<int> GetListIdPlayer()
    {
        List<int> listID = new List<int>();
        foreach (var row in bubbleMaps)
            foreach (var bubble in row)
                if (bubble.isExist() && bubble.id >= 0 && bubble.id < 8 && !listID.Contains(bubble.id)) listID.Add(bubble.id);
        return listID;
    }
    public List<Vector2Int> GetBubbleDisconnect()
    {
        Vector2Int index = Vector2Int.zero;
        List<Vector2Int> listConnection = new List<Vector2Int>();
        List<Vector2Int> listDisconnect = new List<Vector2Int>();
        List<Vector2Int> connections = new List<Vector2Int>();
        for (int i = 0; i < bubbleMaps[0].Count; i++)
        {
            var bubble = bubbleMaps[0][i];
            bubble.setConnection(true);
            listConnection.Add(new Vector2Int(i, 0));
        }
        for (int i = 0; i < bubbleMaps[1].Count; i++)
        {
            var bubble = bubbleMaps[1][i];
            if (bubble.isExist())
            {
                bubble.setConnection(true);
                connections.Add(new Vector2Int(i, 1));
            }
        }
        while (connections.Count > 0)
        {
            List<Vector2Int> temp = new List<Vector2Int>();
            for (int i = 0; i < connections.Count; i++)
            {
                temp.AddRange(CheckConnection(connections[i]));
            }
            connections = temp;
            listConnection.AddRange(connections);
        }
        for (int y = 0; y < bubbleMaps.Count; y++)
        {
            var temp = bubbleMaps[y];
            for (int x = 0; x < temp.Count; x++)
            {
                var bubble = bubbleMaps[y][x];
                if (bubble.isExist() && !bubble.isConnection()) listDisconnect.Add(new Vector2Int((int)bubble.index.y, (int)bubble.index.x));
            }
        }
        TurnOffCheck();
        return listDisconnect;
    }
    public IEnumerator RemoveBubbleDisconnect(List<Vector2Int> listDisconnect)
    {
        // logArray(listDisconnect);
        for (int i = 0; i < listDisconnect.Count; i++)
        {
            var bubbleM = bubbleMaps[listDisconnect[i].y][listDisconnect[i].x];
            var idColor = ColorToID(bubbleM.color);
            var bubble = bubbleM.getBubble();
            var loc = GetWorldLocation(IndexToLocation(listDisconnect[i]));
            Vector2 loc1 = new Vector2(Random.Range(loc.x - 1f, loc.x + 1f), Random.Range(-3f, -4.5f));
            bubble.transform.DOMove(loc1, Random.Range(0.3f, 0.7f))
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                effectsController.PlayEffect(idColor, loc1);
                bubbleM.getBubble().transform.DOKill();
                Destroy(bubble);
                bubbleM.Destroy();
            });
        }
        yield return new WaitForSeconds(0.7f);
        // effectsController.SetAnimation("Idle3");

        Vector2 maxIndex = getMaxRow();
        int newRowBubbleHide = (int)maxIndex.x - 11;
        if (newRowBubbleHide == this.rowBubbleHide)
            this.play = true;
        else
        {
            transform.DOMoveY((newRowBubbleHide - 1) * GameDefine.HEIGHT_ROW, 0.5f)
           .OnComplete(() => { this.play = true; });
            this.rowBubbleHide = newRowBubbleHide;
        }
        Debug.Log(CheckWin());
    }
    private List<Vector2Int> CheckConnection(Vector2Int index)
    {
        List<Vector2Int> listConnection = new List<Vector2Int>();
        Vector2Int R = index;
        Vector2Int L = index;
        Vector2Int BR = index;
        Vector2Int BL = index;
        if (index.y % 2 != 0)
        {
            if (index.x == 0)
            {
                R = new Vector2Int(index.x + 1, index.y);
                BR = new Vector2Int(index.x, index.y + 1);

            }
            else if (index.x == 10)
            {
                BL = new Vector2Int(index.x - 1, index.y + 1);
                L = new Vector2Int(index.x - 1, index.y);
            }
            else
            {
                R = new Vector2Int(index.x + 1, index.y);
                BR = new Vector2Int(index.x, index.y + 1);
                BL = new Vector2Int(index.x - 1, index.y + 1);
                L = new Vector2Int(index.x - 1, index.y);
            }
        }
        else
        {
            if (index.x == 0)
            {
                R = new Vector2Int(index.x + 1, index.y);
                BR = new Vector2Int(index.x + 1, index.y + 1);
                BL = new Vector2Int(index.x, index.y + 1);

            }
            else if (index.x == 9)
            {
                BR = new Vector2Int(index.x + 1, index.y + 1);
                BL = new Vector2Int(index.x, index.y + 1);
                L = new Vector2Int(index.x - 1, index.y);
            }
            else
            {
                R = new Vector2Int(index.x + 1, index.y);
                BR = new Vector2Int(index.x + 1, index.y + 1);
                BL = new Vector2Int(index.x, index.y + 1);
                L = new Vector2Int(index.x - 1, index.y);
            }
        }
        if (bubbleMaps[L.y][L.x].isExist() && !bubbleMaps[L.y][L.x].isConnection())
        {
            bubbleMaps[L.y][L.x].setConnection(true);
            listConnection.Add(L);
        }
        if (bubbleMaps[R.y][R.x].isExist() && !bubbleMaps[R.y][R.x].isConnection())
        {
            bubbleMaps[R.y][R.x].setConnection(true);
            listConnection.Add(R);
        }
        if (bubbleMaps[BL.y][BL.x].isExist() && !bubbleMaps[BL.y][BL.x].isConnection())
        {
            bubbleMaps[BL.y][BL.x].setConnection(true);
            listConnection.Add(BL);
        }
        if (bubbleMaps[BR.y][BR.x].isExist() && !bubbleMaps[BR.y][BR.x].isConnection())
        {
            bubbleMaps[BR.y][BR.x].setConnection(true);
            listConnection.Add(BR);
        }
        return listConnection;
    }
    public Vector2Int LocationToIndex(Vector2 mousePos)
    {
        int count_Row_0 = 0;
        if (rowBubbleHide >= 1) count_Row_0 = bubbleMaps[rowBubbleHide].Count;
        else count_Row_0 = bubbleMaps[1].Count;

        float constant = screenSize.y * 0.5f - GameDefine.SIZE_TOP_BAR.y * 0.8f;
        if (rowBubbleHide > 0) constant = screenSize.y * 0.5f - GameDefine.SIZE_TOP_BAR.y * 0.8f;
        else if (rowBubbleHide == 0) constant -= GameDefine.HEIGHT_ROW;
        else constant -= 2;

        float x = 0;
        float y = Mathf.Floor((constant - mousePos.y) / GameDefine.HEIGHT_ROW);
        if (count_Row_0 == 11)
        {
            if (rowBubbleHide == 0)
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
            else
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

        if (x < 0) x = 0;
        // if (y < 0) y = 0;
        y += 1;
        // Debug.Log("y: "+ y);
        // Debug.Log(rowBubbleHide);
        if (rowBubbleHide > 0) y += (rowBubbleHide - 1);
        return new Vector2Int((int)x, (int)y);
    }
    public Vector2 IndexToLocation(Vector2Int index)
    {

        float constant = screenSize.y * 0.5f - GameDefine.SIZE_TOP_BAR.y + GameDefine.HEIGHT_ROW * 0.5f;
        float y = constant - index.y * GameDefine.HEIGHT_ROW;
        Vector2 pos;
        if (index.y % 2 == 0)
            pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x * 0.95f + index.x, y);
        else
            pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x / 2 + index.x, y);

        return pos;
    }
    public int ColorToID(string color)
    {
        int id = -1;
        switch (color)
        {
            case "B_Blue":
                id = 0;
                break;
            case "B_Cyan":
                id = 1;
                break;
            case "B_Green":
                id = 2;
                break;
            case "B_Pink":
                id = 3;
                break;
            case "B_Purple":
                id = 4;
                break;
            case "B_Red":
                id = 5;
                break;
            case "B_Yellow":
                id = 6;
                break;
        }
        return id;
    }

    public bool CheckWin()
    {
        for (int i = 1; i < bubbleMaps.Count; i++)
        {
            foreach (var item in bubbleMaps[i])
            {
                if (item.isExist()) return false;
            }
        }
        return true;
    }
}
