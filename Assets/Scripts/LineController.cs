using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private Vector2 screenSize;
    private int rowBubbleHide;
    Vector2 start;

    [Range(0.1f, 20f)]
    public float Delta;
    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();
    List<Vector3> posWall = new List<Vector3>();
    private List<List<BubbleMaps>> bubbleMaps = new List<List<BubbleMaps>>();
    [SerializeField] private GameObject[] listDot;
    private int idDot;
    private bool isMOveDot = true;
    public void SetUp(Vector2 start, Vector2 screenSize, List<List<BubbleMaps>> bubbleMaps, int rowBubbleHide)
    {
        this.start = start;
        this.screenSize = screenSize;
        this.rowBubbleHide = rowBubbleHide;
        this.bubbleMaps = bubbleMaps;
    }
    public void SetStart(int idDot, List<List<BubbleMaps>> bubbleMaps, int rowBubbleHide)
    {

        this.idDot = idDot;
        this.bubbleMaps = bubbleMaps;
        this.rowBubbleHide = rowBubbleHide;
    }
    public List<Vector3> DrawPoints(Vector2 mousePosition)
    {
        DestroyAllDots();

        Vector2 end;
        Vector2 direction = (mousePosition - start).normalized;


        RaycastHit2D ray = Physics2D.Raycast(start, direction);

        Vector3 rayPos = ray.point;
        end = new Vector2(rayPos.x, rayPos.y);


        if (ray.collider != null)
        {
            if (ray.transform.gameObject.layer == LayerMask.NameToLayer("Bubble"))
            {
                posWall.Add(AddPositions(start, end, direction, true));
                // Debug.Log(ray.collider.name);
            }
            else if (ray.collider.tag == "Top")
            {
                posWall.Add(AddPositions(start, end, direction, true));
            }
            else if (ray.collider.tag == "Wall")
            {
                posWall.Add(AddPositions(start, end, direction, true));
                while (ray.collider != null && ray.collider.tag == "Wall")
                {
                    Vector3 newRayPos = ray.point;
                    Vector2 newStart = new Vector2(newRayPos.x, newRayPos.y);
                    direction = Vector2.Reflect(direction, Vector2.right);
                    Vector2 rightTol = new Vector2(-.0001f, 0);
                    Vector2 leftTol = new Vector2(.0001f, 0);
                    if (ray.collider.name == "wallRight")
                        ray = Physics2D.Raycast(newStart + rightTol, direction);
                    else
                        ray = Physics2D.Raycast(newStart + leftTol, direction);
                    rayPos = ray.point;
                    end = new Vector2(rayPos.x, rayPos.y);

                    if (end != Vector2.zero)
                    {
                        posWall.Add(AddPositions(newStart, end, direction, false));

                    }
                }
            }

        }
        DrawLine();
        posWall[posWall.Count - 1] = positions[positions.Count - 1];
        return posWall;
    }
    public Vector2 AddPositions(Vector2 start, Vector2 end, Vector2 direction, bool drawFirst)
    {
        Vector2 point = start;
        if (end != Vector2.zero)
        {
            float dist = Vector2.Distance(start, end);
            while ((end - start).magnitude > (point - start).magnitude)
            {
                point += (direction * Delta);
                if (point.x >= -screenSize.x / 2 && point.x <= screenSize.x / 2)
                {
                    positions.Add(point);
                }
            }
        }
        return new Vector3(point.x, point.y, -0.01f);
    }
    public void DrawLine()
    {

        for (int i = positions.Count - 1; i >= 0; i--)
        {
            Vector2Int index = LocationToIndex(positions[i]);
            if (bubbleMaps[index.y][index.x].isExist())
                positions.RemoveAt(i);
            else break;
        }
        foreach (var pos in positions)
        {
            GameObject dot = Instantiate(listDot[idDot], pos, Quaternion.identity, transform);
            dots.Add(dot);
        }
    }
    public void MoveDots()
    {
        if (isMOveDot)
        {
            for (int i = 0; i < dots.Count; i++)
            {
                GameObject dot = dots[i];
                Vector2 point;
                if (i != dots.Count - 1) 
                {
                    point = positions[i + 1];

                    LeanTween.cancel(dot);
                    LeanTween.move(dot, new Vector3(point.x, point.y, -0.01f), 0.3f)
                    .setEase(LeanTweenType.linear)
                    .setOnCompleteParam(i)
                    .setOnComplete((index) =>
                    {
                        if ((int)index == dots.Count - 2)
                        {
                            GameObject temp = dots[dots.Count - 1];
                            dots.RemoveAt(dots.Count - 1);
                            dots.Insert(0, temp);
                            temp.transform.position = positions[0];
                            isMOveDot = true;
                        }
                    });
                }
            }
            isMOveDot = false;
        }
    }
    public void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            LeanTween.cancel(dot);
            Destroy(dot);
        }
        isMOveDot = true;
        dots.Clear();
        positions.Clear();
        posWall.Clear();
    }
    private Vector2Int LocationToIndex(Vector2 mousePos)
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
        if (y < 0) y = 0;
        y += 1;
        if (rowBubbleHide > 0) y += (rowBubbleHide - 1);
        return new Vector2Int((int)x, (int)y);
    }
    private Vector2 IndexToLocation(Vector2Int index)
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
}
