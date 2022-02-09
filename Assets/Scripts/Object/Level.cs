using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int steps_num;
    public int[] target_scores;
    public int id;

    public Target target_bubbles;
    public Target targets;
    public Bubble_level[] bubbles;
    public static Level CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Level>(jsonString);
    }

}
[System.Serializable]
public class Bubble_level
{
    // Start is called before the first frame update
    public int id;
    public int x;
    public int y;
    public bool is_invalid;
}
[System.Serializable]


public class Target
{
    // Start is called before the first frame update
    public int type_1;
    public int type_2;
    public int type_3;
    public int type_4;
}