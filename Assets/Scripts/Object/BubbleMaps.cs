using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class BubbleMaps
{
    public int id;
    public Vector2 index;
    private bool collision;
    private GameObject bubble;
    public Vector2 location;
    private bool exist;
    private bool check;
    private bool connection;

    public bool isConnection()
    {
        return this.connection;
    }

    public void setConnection(bool connection)
    {
        this.connection = connection;
    }



    public bool isCollision()
    {
        return this.collision;
    }
    public void setCollision(bool is_collision)
    {
        this.collision = is_collision;
    }
    public GameObject getBubble()
    {
        return this.bubble;
    }
    public void setBubble(GameObject bubble)
    {
        this.bubble = bubble;
    }
    public bool isExist()
    {
        return this.exist;
    }
    public void setExist(bool exist)
    {
        this.exist = exist;
    }
    public bool isCheck()
    {
        return this.check;
    }

    public void setCheck(bool check)
    {
        this.check = check;
    }


    public string color;

    public BubbleMaps(int id, Vector2 index, bool collision, GameObject bubble, Vector2 location, bool exist, bool check, string color, bool connection)
    {
        this.id = id;
        this.index = index;
        this.collision = collision;
        this.bubble = bubble;
        this.location = location;
        this.exist = exist;
        this.connection = connection;
        this.check = check;
        this.color = color;
    }
    public BubbleMaps(Vector2 index, bool is_exist)
    {
        this.id = -1;
        this.index = index;
        this.collision = false;
        this.bubble = null;
        this.location = Vector2.zero;
        this.exist = is_exist;
        this.connection = false;
        this.check = false;
        this.color = null;
    }
    public void Destroy()
    {
        this.id = -1;
        this.collision = false;
        this.bubble = null;
        this.location = Vector2.zero;
        this.exist = false;
        this.color = null;
        this.check = false;
        this.connection = false;
    }
}
