using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public string packageId;

    public void TakePackage(Transform agentTransform)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Destroy(rb);
        tag = "collectedPackage";

        transform.parent = agentTransform;
    }

    public void DropPackage(Transform droneArea)
    {
        tag = "delivered";
        transform.parent = droneArea;
        gameObject.AddComponent<Rigidbody>();
    }
    
}
