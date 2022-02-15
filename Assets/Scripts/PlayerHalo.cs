using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHalo : MonoBehaviour
{

    [Header("List Prefabs outer:")]
    [SerializeField]
    private GameObject[] listBubbles;

    private GameObject hola0;
    private GameObject hola1;
    void Start()
    {

    }

    public void SpawbHola(int id, float x, float y)
    {
        // this.transform.SetParent(transformParent);
        this.hola0 = Instantiate(listBubbles[id], new Vector3(x, y, 0), Quaternion.identity, this.transform);
        this.hola0.transform.localScale += new Vector3(0.2f, 0.15f, 0.2f);
        this.hola1 = Instantiate(listBubbles[id], new Vector3(x, y, 0), Quaternion.identity, this.transform);
        this.hola1.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        rotateHalo();
    }

    // Update is called once per frame
    void Update()
    {
        // if (this.hola0 && this.hola1)
        // {
        //     this.hola0.transform.RotateAround(this.hola0.transform.position, new Vector3(0, 0, 1), 40 * Time.deltaTime);
        //     this.hola1.transform.RotateAround(this.hola1.transform.position, new Vector3(0, 0, -1), 40 * Time.deltaTime);
        // }
    }
    private void rotateHalo(){
        LeanTween.cancel(hola0);
        LeanTween.cancel(hola1);
        LeanTween.rotateAround(hola0, new Vector3(0, 0, 1), 360, 8f).setLoopClamp();
        LeanTween.rotateAround(hola1, new Vector3(0, 0, -1), 360, 8f).setLoopClamp();
    }
    public void DestroyHalo()
    {
        Destroy(hola0);
        Destroy(hola1);
    }
}
