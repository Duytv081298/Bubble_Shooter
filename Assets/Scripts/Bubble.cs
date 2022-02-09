using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Start is called before the first frame update
    // public Bounds size;
    private float width;

    private float height;

    public float getWidth()
    {
        return this.width;
    }

    public void setWidth(float width)
    {
        this.width = width;
    }

    public float getHeight()
    {
        return this.height;
    }

    public void setHeight(float height)
    {
        this.height = height;
    }


    [SerializeField]
    private SpriteRenderer spRender;

    // [SerializeField]
    // private Rigidbody2D rb;

    // [SerializeField]
    // private Collider2D col;

    private void Awake()
    {
        width = spRender.bounds.size.x;
        height = spRender.bounds.size.y;
    }
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {

    }
}
