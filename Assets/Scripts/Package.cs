using UnityEngine;

public class Package : MonoBehaviour
{
    public string packageId;

    public void TakePackage(Transform agentTransform)
    {
        Destroy(GetComponent<Rigidbody>());
        tag = "Untagged";
        transform.parent = agentTransform;
    }

    public void DropPackage(Transform droneArea)
    {
        transform.parent = droneArea;
        gameObject.AddComponent<Rigidbody>();
    }
    
}
