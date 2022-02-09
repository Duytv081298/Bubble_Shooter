using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    Vector3 mp;
    Vector2 mousePosition;
    Vector2 start;
    public Sprite Dot;
    [Range(0.01f, 1f)]
    public float Size;
    [Range(0.1f, 20f)]
    public float Delta;
    List<Vector2> positions = new List<Vector2>();
    List<Vector2> pos = new List<Vector2>();
    List<GameObject> dots;


    // Start is called before the first frame update


    [SerializeField]

    private GameObject[] listDot;
    void Start()
    {
        start = new Vector2(transform.position.x, transform.position.y);
        // Debug.Log(start);
        // positions = new List<Vector2>();
        dots = new List<GameObject>();


    }

    // Update is called once per frame
    void Update()
    {
        mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(mp.x, mp.y);
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(mousePosition);
            DrawPoints(mousePosition);
        }
    }
    public void DrawPoints(Vector2 mousePosition)
    {
        DestroyAllDots();
        pos.Add(start);


        Vector2 end;
        Vector2 direction = (mousePosition - start).normalized;


        RaycastHit2D ray = Physics2D.Raycast(start, direction);

        Vector3 rayPos = ray.point;
        end = new Vector2(rayPos.x, rayPos.y);
        pos.Add(end);

        if (ray.collider != null)
        {
            // Debug.DrawRay(point, direction, Color.blue, 5.0f);
            if (ray.collider.tag == "wall")
            {
                Debug.DrawLine(start, end, Color.blue, 10.0f);
                DrawOneLine(start, end, direction, true);
                while (ray.collider != null && ray.collider.tag == "wall")
                {
                    Debug.Log(ray.collider.name);
                    Vector3 newRayPos = ray.point;
                    Vector2 newStart = new Vector2(newRayPos.x, newRayPos.y);
                    direction = Vector2.Reflect(direction, Vector2.right);
                    Debug.DrawRay(newStart, direction, Color.red, 10.0f);
                    // Debug.DrawRay(end, Vector2.right, Color.red, 10.0f);

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
                        Debug.DrawLine(newStart, end, Color.blue, 10.0f);
                        pos.Add(end);
                        DrawOneLine(newStart, end, direction, false);
                    }else{

                    }
                }



            }

        }
        // Render();

    }
    public void DrawOneLine(Vector2 start, Vector2 end, Vector2 direction, bool drawFirst)
    {
        if (end != Vector2.zero)
        {
            Vector2 point = start;
            float dist = Vector2.Distance(start, end);
            while ((end - start).magnitude > (point - start).magnitude)
            {
                GameObject dot = Instantiate(listDot[0], point, Quaternion.identity, transform.parent);
                dots.Add(dot);
                point += (direction * Delta);
                GameObject dot1 = Instantiate(listDot[0], point, Quaternion.identity, transform.parent);
                dots.Add(dot1);
            }
        }
        else
        {

        }

    }
    private void Render()
    {
        foreach (var position in positions)
        {
            var g = GetOneDot();
            g.transform.position = position;
            dots.Add(g);
        }
    }
    GameObject GetOneDot()
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = Vector3.one * Size;
        gameObject.transform.parent = transform;

        var sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Dot;
        return gameObject;
    }

    public void DestroyAllDots()
    {
        foreach (var dot in dots)
            Destroy(dot);
        dots.Clear();
        positions.Clear();
        pos.Clear();
    }
}
