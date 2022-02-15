using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLight : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("rotation direction:")]
    public Vector3 vector;
    public float time;
    void Start()
    {
        LeanTween.cancel(gameObject);
        LeanTween.rotateAround(gameObject, vector, 360, time).setLoopClamp();
    }

}
