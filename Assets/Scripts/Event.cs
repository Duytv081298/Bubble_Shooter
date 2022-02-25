using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{

    [SerializeField] private GameController gameController;
    private bool play = false;
    public bool isPlay()
    {
        return this.play;
    }
    public void setPlay(bool play)
    {
        this.play = play;
    }
    void Start()
    {

    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = new Vector2(mousePos.x, mousePos.y);
        if (play)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameController.Test();
            }
        }

    }
    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }
}

