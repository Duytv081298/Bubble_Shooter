using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLight : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("rotation direction:")]
    public Vector3 vector;
    public int speeed;
    void Start()
    {

    }
    void Update()
    {
        transform.RotateAround(transform.position, vector, speeed * Time.deltaTime);
    }
}
