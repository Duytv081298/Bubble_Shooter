using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Newtonsoft.Json;

public class GameHandler : MonoBehaviour
{

    private int levelInt = 0;


    private Level level;

    public int getLevelInt()
    {
        return this.levelInt;
    }

    public void setLevelInt(int levelInt)
    {
        this.levelInt = levelInt;
    }

    public void nextLevel()
    {
        this.levelInt++;
        this.level = getdataLevel(this.levelInt);
    }
    public void previousLevel()
    {
        if (this.levelInt == 0)
        {
            Debug.Log("This is the lowest");
            return;
        }
        this.levelInt--;
        this.level = getdataLevel(this.levelInt);
    }
    public Level getLevel()
    {
        return this.level;
    }

    public void setLevel(Level level)
    {
        this.level = level;
    }

    private void Awake()
    {
        level = getdataLevel(this.levelInt);

    }
    public Level getdataLevel(int levelInt)
    {
        string url = Application.dataPath + "/_Levels/level_" + levelInt + ".json";
        string json = File.ReadAllText(url);

        Level lv = JsonConvert.DeserializeObject<Level>(json);
        return lv;
    }
    void Start()
    {

    }
    void Update()
    {

    }
}
