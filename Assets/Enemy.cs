using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform[] waypoint;
    private int waypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update()
    {   
        // Check if the agent is close to current target point
        if(agent.remainingDistance < .5f)
        {
            // Set the destination to the next waypoint
            agent.SetDestination(GetNextWWaypoint());
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {

    }

    private Vector3 GetNextWWaypoint()
    {
        if (waypointIndex < waypoint.Length)
        {
            return transform.position;  
        }
        Vector3 targetPoint = waypoint[waypointIndex].position;
        waypointIndex++;

        return targetPoint;
    }
}
