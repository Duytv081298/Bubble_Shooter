using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BubbleMaps
{
    public Vector2 index;
    public bool is_active;
    public GameObject bubble;
    public Vector2 location;
    public bool is_exist;
    public bool is_check;

    public string color;

    public BubbleMaps(Vector2 index, bool is_active, GameObject bubble, Vector2 location, bool is_exist, bool is_check, string color)
    {
        this.index = index;
        this.is_active = is_active;
        this.bubble = bubble;
        this.location = location;
        this.is_exist = is_exist;
        this.is_check = is_check;
        this.color = color;
    }
        public BubbleMaps(Vector2 index, bool is_exist)
    {
        this.index = index;
        this.is_active = false;
        this.bubble = null;
        this.location = Vector2.zero;
        this.is_exist = is_exist;
        this.is_check = false;
        this.color = null;
    }
}
