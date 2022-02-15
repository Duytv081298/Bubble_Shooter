using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BubbleMaps : MonoBehaviour
{
    public Vector2 index;
    public bool is_active;
    public GameObject bubble;

    public Vector2 location;

    public BubbleMaps(Vector2 index, bool is_active, GameObject bubble, Vector2 location)
    {
        this.index = index;
        this.is_active = is_active;
        this.bubble = bubble;
        this.location = location;
    }
}
