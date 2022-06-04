using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public float rotationSpeed;
    public Transform[] propellers;

    private float propellersLength;

    private void Start()
    {
        propellersLength = propellers.Length;        
    }

    void Update()
    {
        for(int i =0; i < propellersLength; i++)
        {
            propellers[i].Rotate(0, 0, rotationSpeed*Time.deltaTime);
        }
    }
}
