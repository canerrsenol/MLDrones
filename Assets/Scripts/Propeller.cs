using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public float rotationSpeed;
    public Transform[] propellers;

    void Update()
    {
        for(int i =0; i < propellers.Length; i++)
        {
            propellers[i].Rotate(0, 0, rotationSpeed*Time.deltaTime);
        }
    }
}
