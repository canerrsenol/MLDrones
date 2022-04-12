using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DroneAgent : Agent
{
    public float moveForce = 2f;

    public Transform bottomCenterTip;

    public GameObject[] houses;

    new private Rigidbody rigidbody;

    private DroneArea droneArea;

    private Package takenPackage;

    private bool hasCargo;

    private Transform targetTransform;

    public override void Initialize()
    {
        droneArea = GetComponentInParent<DroneArea>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        hasCargo = false;
        rigidbody.velocity *= 0;

        droneArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(hasCargo);

        CalculateTargetPosition();
        sensor.AddObservation(targetTransform.position);
    }

    private void CalculateTargetPosition()
    {
        if(!hasCargo)
        {
            targetTransform = droneArea.packageList[0].transform;
        }
        else
        {
            foreach(GameObject house in houses)
            {
                if(house.CompareTag(takenPackage.packageId))
                {
                    targetTransform = house.transform;
                    return;
                }
            }
        }
    }

    // 3 continious actions
    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-1f / MaxStep);

        // Calculate movement vector
        Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);

        // Add force in the direction of the move vector
        rigidbody.AddForce(move * moveForce * Time.fixedDeltaTime);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continiousActions = actionsOut.ContinuousActions;

        Vector3 forward = Vector3.zero;
        Vector3 left = Vector3.zero;
        Vector3 up = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) forward = transform.forward;
        else if (Input.GetKey(KeyCode.S)) forward = -transform.forward;

        if (Input.GetKey(KeyCode.A)) left = -transform.right;
        else if (Input.GetKey(KeyCode.D)) left = transform.right;

        if (Input.GetKey(KeyCode.UpArrow)) up = transform.up;
        else if (Input.GetKey(KeyCode.DownArrow)) up = -transform.up;

        Vector3 combined = (forward + left + up).normalized;

        continiousActions[0] = combined.x;
        continiousActions[1] = combined.y;
        continiousActions[2] = combined.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasCargo && other.CompareTag("home1") && takenPackage.packageId == "1")
        {
            hasCargo = false;
            takenPackage.DropPackage(droneArea.transform);
            AddReward(1f);
            
            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home2") && takenPackage.packageId == "2")
        {
            hasCargo = false;
            takenPackage.DropPackage(droneArea.transform);
            AddReward(1f);

            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home3") && takenPackage.packageId == "3")
        {
            hasCargo = false;
            takenPackage.DropPackage(droneArea.transform);
            AddReward(1f);

            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home1") && takenPackage.packageId != "1")
        {
            AddReward(-0.2f);

            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home2") && takenPackage.packageId != "2")
        {
            AddReward(-0.2f);

            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home3") && takenPackage.packageId != "3")
        {
            AddReward(-0.2f);

            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("wall"))
        {
            AddReward(-0.4f);
        }
        else if(!hasCargo && other.collider.GetComponent<Package>() != null)
        {
            hasCargo = true;
            takenPackage = other.collider.GetComponent<Package>();
            takenPackage.TakePackage(transform);

            if(transform.position.y - droneArea.packageList[0].transform.position.y > 0.69f) AddReward(1.5f);
            AddReward(1f);
        }
    }
}
