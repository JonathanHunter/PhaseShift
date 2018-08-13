using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLookAt : MonoBehaviour
{


    public Transform target;
    public Vector3 Axis;

    // Use this for initialization
    void Awake()
    {
        target = FindObjectOfType<Camera>().transform;
        //target = GetComponent<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target, Vector3.forward);
    }
}
