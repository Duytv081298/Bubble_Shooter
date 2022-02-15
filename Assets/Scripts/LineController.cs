using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private Vector2 screenSize;
    Vector2 start;

    [Range(0.1f, 20f)]
    public float Delta;
    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();
    List<Vector3> posWall = new List<Vector3>();

    [SerializeField]
    private GameObject[] listDot;

    private int idDot;
    private bool isMOveDot = true;

    public bool isIsMOveDot()
    {
        return this.isMOveDot;
    }

    public void setIsMOveDot(bool isMOveDot)
    {
        this.isMOveDot = isMOveDot;
    }


    public void setIdDot(int idDot)
    {
        this.idDot = idDot;
    }


    public void setStart(Vector2 start, Vector2 screenSize)
    {
        this.start = start;
        this.screenSize = screenSize;
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
            if (ray.collider.tag == "Wall")
            {
                posWall.Add(DrawOneLine(start, end, direction, true));
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
                        posWall.Add(DrawOneLine(newStart, end, direction, false));
                    }
                    else
                    {

                    }
                }
            }
            else if(ray.transform.gameObject.layer  == LayerMask.NameToLayer("Bubble")){
                Debug.Log("bubble");
                posWall.Add(DrawOneLine(start, end, direction, true));
            }
        }
        return posWall;
    }
    public Vector2 DrawOneLine(Vector2 start, Vector2 end, Vector2 direction, bool drawFirst)
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
                    GameObject dot = Instantiate(listDot[idDot], point, Quaternion.identity, transform);
                    positions.Add(point);
                    dots.Add(dot);
                }
            }
        }
        else
        {

        }

        return new Vector3 (point.x, point.y, -0.01f);

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
}
