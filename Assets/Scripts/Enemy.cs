using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Basic, Fast, None}

public class Enemy : MonoBehaviour , IDamagable
{
    private NavMeshAgent agent;

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform centerPoint;
    public int healthPoints = 4;

    [Header("Movement")]
    [SerializeField] private float turnSpeed = 10;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private float totalDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    private void Start()
    {
        waypoints = FindAnyObjectByType<WaypointManager>().GetWaypoints();

        CollectTotalDistance();
    }

    private void Update()
    {
        FaceTarget(agent.steeringTarget);

        // Check if the agent is close to current target point
        if(agent.remainingDistance < .5f)
        {
            // Set the destination to the next waypoint
            agent.SetDestination(GetNextWaypoint());
        }
    }

    public float DistanceToTheFinishLine() => totalDistance + agent.remainingDistance;

    private void CollectTotalDistance()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance = totalDistance + distance;
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        // Calculate the direction from current position to the new target
        Vector3 directionToTarget = newTarget - transform.position;
        if (directionToTarget.magnitude == 0) return;
        directionToTarget.y = 0; //Ignore any diference in the vertical position // Removes vertical component

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed* Time.deltaTime);
    }

    private Vector3 GetNextWaypoint()
    {
        // Check if the waypoint index is beyond the last waypoint
        if (waypointIndex >= waypoints.Length)
        {
            //If true, return the agent's current position, effectively stopping it
            return transform.position;  
        }

        // Get the current target point from the waypoints array
        Vector3 targetPoint = waypoints[waypointIndex].position;

        // If this is not the first waypoint, calculate the distance from the previous waypoint
        if (waypointIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex - 1].position);
            //Subtract this distance from the total distance
            totalDistance = totalDistance - distance;
        }

        waypointIndex++;

        return targetPoint;
    }

    public Vector3 CenterPoint() => centerPoint.position;
    public EnemyType GetEnemyType() => enemyType;

    public void TakeDamage(int damage)
    {
        healthPoints = healthPoints - damage;

        if (healthPoints <= 0) Destroy(gameObject);
    }
}
