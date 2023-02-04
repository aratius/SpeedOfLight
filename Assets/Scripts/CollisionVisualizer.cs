using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionVisualizer : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

}
