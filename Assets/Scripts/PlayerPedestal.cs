using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPedestal : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    private GameObject Pedestal0;

    [SerializeField]
    private GameObject Pedestal1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Pedestal0 && Pedestal1)
        {
            Pedestal0.transform.RotateAround(Pedestal0.transform.position, new Vector3(0, 0, 1), 40 * Time.deltaTime);
            Pedestal1.transform.RotateAround(Pedestal0.transform.position, new Vector3(0, 0, -1), 40 * Time.deltaTime);
        }
    }
}
