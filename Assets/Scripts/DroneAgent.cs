using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DroneAgent : Agent
{
    public float moveForce = 2f;

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
        sensor.AddObservation(Vector3.Distance(transform.position , targetTransform.position));

        sensor.AddObservation(transform.position.y);
    }

    private void CalculateTargetPosition()
    {
        if(!hasCargo && droneArea.packageList[0] != null)
        {
            targetTransform = droneArea.packageList[0].transform;
        }
        else
        {
            foreach(GameObject house in houses)
            {
                string target;
                switch(takenPackage.packageId)
                {
                    case "1" : target = "home1";
                    break;
                    case "2" : target = "home2";
                    break;
                    case "3" : target = "home3";
                    break; 
                    default : target = "Untagged";
                    break;
                }
                if(house.CompareTag(target))
                {
                    targetTransform = house.transform;
                    return;
                }
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-1f / MaxStep);

        float moveX = 0;
        float moveY = 0;
        float moveZ = 0;

        if(actions.DiscreteActions[0] == 1) moveX = 1;
        else if(actions.DiscreteActions[0] == 2) moveX = -1;

        if(actions.DiscreteActions[1] == 1) moveY = 1;
        else if(actions.DiscreteActions[1] == 2) moveY = -1;

        if(actions.DiscreteActions[2] == 1) moveZ = 1;
        else if(actions.DiscreteActions[2] == 2) moveZ = -1;

        Vector3 move = new Vector3(moveX, moveY, moveZ);
        rigidbody.AddForce(move * moveForce * Time.fixedDeltaTime);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continiousActions = actionsOut.DiscreteActions;

        int forward = 0;
        int left = 0;
        int up = 0;

        if (Input.GetKey(KeyCode.W)) forward = 1;
        else if (Input.GetKey(KeyCode.S)) forward = 2;

        if (Input.GetKey(KeyCode.A)) left = 1;
        else if (Input.GetKey(KeyCode.D)) left = 2;

        if (Input.GetKey(KeyCode.UpArrow)) up = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) up = 2;

        continiousActions[0] = left;
        continiousActions[1] = up;
        continiousActions[2] = forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasCargo && other.CompareTag("home1") && takenPackage.packageId == "1")
        {
            hasCargo = false;
            takenPackage.DropPackage(droneArea.transform);
            AddReward(2f);
            
            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home2") && takenPackage.packageId == "2")
        {
            hasCargo = false;
            takenPackage.DropPackage(droneArea.transform);
            AddReward(2f);

            EndEpisode();
        }
        else if(hasCargo && other.CompareTag("home3") && takenPackage.packageId == "3")
        {
            hasCargo = false;
            takenPackage.DropPackage(droneArea.transform);
            AddReward(2f);

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
        else if(!hasCargo && other.collider.GetComponent<Package>() != null && CheckHeight())
        {
            hasCargo = true;
            takenPackage = other.collider.GetComponent<Package>();
            takenPackage.TakePackage(transform);

            AddReward(1f);
        }
    }

    private bool CheckHeight()
    {
        if(transform.position.y - droneArea.packageList[0].transform.position.y > 0.69f) return true;
        return false;
    }
}
