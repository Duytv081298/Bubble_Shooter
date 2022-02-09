using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("List Prefabs bubbles:")]
    [SerializeField]
    private GameObject[] listBubbles;

    [SerializeField]
    private PlayerHalo PlayerHalo;

    private GameObject player;

    private Vector3 playerPos;

    private int id;
    public Vector3 getPlayerPos()
    {
        return this.playerPos;
    }
    public GameObject getPlayer()
    {
        return this.player;
    }

    public void setPlayer(int id, Vector3 pos)
    {
        this.player = Instantiate(listBubbles[id], pos, Quaternion.identity, this.transform);
        this.id = id;
        this.playerPos = pos;
        PlayerHalo.SpawbHola(id, pos.x, pos.y);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
