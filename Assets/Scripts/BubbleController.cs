using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    [Header("List Prefabs bubbles:")]
    [SerializeField] private GameObject[] listBubbles;
    private List<List<BubbleMaps>> bubbleMaps = new List<List<BubbleMaps>>();
    private Vector2 screenSize;
    // private bool move = true;
    private List<Vector2> listPos = new List<Vector2>();
    [SerializeField] private GameObject top_bar;
    private int rowDefault;
    private int rowBubbleHide;
    public void SetUp(Vector2 screenSize, List<List<BubbleMaps>> bubbleMaps, int rowDefault)
    {
        this.screenSize = screenSize;
        this.bubbleMaps = bubbleMaps;
        this.rowDefault = rowDefault;
        this.rowBubbleHide = rowDefault - 12;
    }
    public List<List<BubbleMaps>> setPosition(Bubble_level[] bubbles)
    {
        float constant = screenSize.y * 0.5f - GameDefine.SIZE_TOP_BAR.y + GameDefine.HEIGHT_ROW * 0.5f;

        foreach (Bubble_level bubble in bubbles)
        {
            GameObject bubbleTemplate;
            Vector2 pos;
            if (bubble.id == 100)
            {
                pos = new Vector2(-screenSize.x / 2 + GameDefine.SIZE_BUBBLE.x * 0.95f + bubble.x, screenSize.y * 0.5f - bubble.y * GameDefine.HEIGHT_ROW);
                bubbleMaps[bubble.y][bubble.x].location = pos;
                bubbleMaps[bubble.y][bubble.x].is_exist = true;
                if (bubble.x == 0)
                {
                    GameObject topBar = Instantiate(top_bar, new Vector3(0f, screenSize.y * 0.5f, 0f), Quaternion.identity, this.transform);
                    bubbleMaps[bubble.y][bubble.x].bubble = topBar;
                }
            }
            if (bubble.id < 8)
            {
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

                bubbleMaps[bubble.y][bubble.x].bubble = bubbleCl;
                bubbleMaps[bubble.y][bubble.x].location = pos;
                bubbleMaps[bubble.y][bubble.x].is_exist = true;
                bubbleMaps[bubble.y][bubble.x].color = bubbleCl.tag;
            }
        }

        transform.position = new Vector3(0f, -2f, 0f);
        Move();

        return bubbleMaps;
    }
    public Vector2 getMaxRow()
    {
        Vector2 maxRow = Vector2.zero;
        foreach (var row in bubbleMaps)
        {
            foreach (var item in row)
            {
                if (item.is_exist) maxRow = item.index;
            }
        }
        return maxRow;

    }
    private void Move()
    {
        // float dow = rowBubbleHide <= 3 ? rowBubbleHide : 3 + (rowBubbleHide - 3) * GameDefine.HEIGHT_ROW;
        // Debug.Log(dow);
        if (rowDefault > 11)
        {
            transform.DOMoveY((rowBubbleHide - 1) * GameDefine.HEIGHT_ROW, 1).SetDelay(1);
        }
    }
    public Vector3 getPosition()
    {
        // Debug.Log(transform.position);
        return transform.position;
    }
}
