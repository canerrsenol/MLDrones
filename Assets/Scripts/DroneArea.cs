using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DroneArea : MonoBehaviour
{
    public TextMeshPro rewardText;

    public DroneAgent droneAgent;

    public Package[] packagePrefabs;

    public List<GameObject> packageList;

    public Transform droneSpawnTransform;

    public void ResetArea()
    {
        RemoveAllPackages();
        CreatePackagesAndSetPositions();
        float rnd = UnityEngine.Random.value;
        if(rnd > 0.5f)
        {
            SetDroneAgentsPosition();
        }
        else
        {
            SetDroneAgentsPositionTopOfThePackage();
        }
    }

    private void RemoveAllPackages()
    {
        if(packageList != null)
        {
            for(int i=0; i < packageList.Count; i++)
            {
                if(packageList[i] != null)
                {
                    Destroy(packageList[i]);
                }
            }
        }

        packageList = new List<GameObject>();
    }

    public Vector3 ReturnRandomPos()
    {
        float x = UnityEngine.Random.Range(1f, 9f);
        float y = 0.2f;
        float z = UnityEngine.Random.Range(-11, -19);

        return new Vector3(x,y,z);
    }

    private void SetDroneAgentsPositionTopOfThePackage()
    {
        Vector3 packageTransform = packageList[0].transform.localPosition;
        packageTransform.y += UnityEngine.Random.Range(3f, 4f);

        SetDroneAtPosition(packageTransform.x, packageTransform.y, packageTransform.z);
    }

    private void SetDroneAgentsPosition()
    {
        float x = droneSpawnTransform.localPosition.x;
        float y = droneSpawnTransform.localPosition.y;
        float z = droneSpawnTransform.localPosition.z;
        SetDroneAtPosition(x, y, z);
    }

    private void SetDroneAtPosition(float x, float y, float z)
    {
        Rigidbody rb = droneAgent.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        droneAgent.transform.localPosition = new Vector3(x, y, z);
    }

    private void CreatePackagesAndSetPositions()
    {
        Vector3 spawnPosition = ReturnRandomPos();

        GameObject packageObject = Instantiate<GameObject>(packagePrefabs[UnityEngine.Random.Range(0, packagePrefabs.Length)].gameObject);

        packageObject.transform.parent = transform;
        packageObject.transform.localPosition = ReturnRandomPos();
        

        packageList.Add(packageObject);
    }

    private void Start()
    {
        ResetArea();
    }

    private void Update()
    {
        rewardText.text = droneAgent.GetCumulativeReward().ToString("0.00");
    }

}
