using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    [Header("List Prefabs bubbles:")]
    [SerializeField]
    private GameObject[] listBubbles;
    [SerializeField]
    private float camW;
    private float camH;

    private float widthBubble;

    private float heightBubble;
    // private bool move = true;


    private List<Vector2> listPos = new List<Vector2>();
    private void Awake()
    {
        camH = 2f * Camera.main.orthographicSize;
        camW = camH * Camera.main.aspect;
        GameObject bubble = (GameObject)Instantiate(listBubbles[0]);
        Collider2D col = bubble.GetComponent<Collider2D>();
        widthBubble = col.bounds.size.x;
        heightBubble = col.bounds.size.y;
        bubble.SetActive(false);
        Destroy(bubble);
    }
    public void setPosition(Bubble_level[] bubbles)
    {
        foreach (Bubble_level bubble in bubbles)
        {
            GameObject bubbleTemplate;
            Vector2 pos;
            if (bubble.id < 8)
            {
                bubbleTemplate = listBubbles[bubble.id];
                if (bubble.y % 2 == 0)
                {

                    pos = new Vector2(-camW / 2 + widthBubble * 0.95f + bubble.x, (camH - heightBubble) * 0.5f - bubble.y * 0.87f);
                }
                else

                    pos = new Vector2(-camW / 2 + widthBubble / 2 + bubble.x, (camH - heightBubble) * 0.5f - bubble.y * 0.87f);
                listPos.Add(pos);
                SpawbBubble(bubbleTemplate, pos);
            }
        }
    }
    public void SpawbBubble(GameObject bubble, Vector2 pos)
    {
        Instantiate(bubble, pos, Quaternion.identity, this.transform);
    }
    void Start()
    {
        // SpawbBubble();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, z, 0);
        transform.Translate(movement * 2 * Time.deltaTime);
    }
}
