using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 8f, Color.red, -1);
    }


}
