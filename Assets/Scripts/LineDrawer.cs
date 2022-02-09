using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    // [SerializeField]
    // private PlayerController playerController;

    private Vector3 startPos;
    private Vector3 mousePos;
    private float camW;
    private float camH;

    [SerializeField]

    private GameObject[] listDot;
    List<Vector3> positions = new List<Vector3>();
    List<GameObject> dots = new List<GameObject>();

    public float Delta = 5;


    private void Awake()
    {
        camH = 2f * Camera.main.orthographicSize;
        camW = camH * Camera.main.aspect;
    }
    public void Draw(Vector3 startPos, Vector3 mousePos)
    {
        DestroyAllDots();
        this.startPos = startPos;
        this.mousePos = mousePos;
        Vector3 point = startPos;
        Vector3 direction = (mousePos - startPos).normalized;
        float dist = Vector3.Distance(startPos, mousePos);
        Debug.Log(dist);
        var temp = Delta / dist;
        if (temp > 1.7f) temp = 1.7f;
        Debug.Log(temp);
        while ((mousePos - startPos).magnitude > (point - startPos).magnitude)
        {
            // positions.Add(point);
            point += (direction * temp);
            renderDot(point);
        }
    }

    public void renderDot(Vector3 pos)
    {
        GameObject dot = Instantiate(listDot[0], pos, Quaternion.identity, this.transform);
        dots.Add(dot);
    }
    private void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }


    void Start()
    {
        // Debug.Log("Screen Width : " + Screen.width);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
