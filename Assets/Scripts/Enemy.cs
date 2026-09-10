using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Basic, Fast, None}

public class Enemy : MonoBehaviour , IDamagable
{
    private GameManager gameManager;
    private EnemyPortal myPortal;
    private NavMeshAgent agent;

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform centerPoint;
    public int healthPoints = 4;

    [Header("Movement")]
    [SerializeField] private float turnSpeed = 10;

    [SerializeField] private List<Transform> myWaypoints;
    private int nextWaypointIndex;
    private int currentWaypointIndex;

    private float totalDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);

        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void SetupEnemy(List<Waypoint> newWaypoints,EnemyPortal myNewPortal)
    {
        myWaypoints = new List<Transform>();

        foreach (var point in newWaypoints)
        {
            myWaypoints.Add(point.transform);
        }

        CollectTotalDistance();

        myPortal = myNewPortal;
    }
   

    private void Update()
    {
        FaceTarget(agent.steeringTarget);

        if (ShouldChangeWaypoint())
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }


    private bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
            return false;

        if (agent.remainingDistance < .5f)
            return true;

        Vector3 currentWaypoint = myWaypoints[currentWaypointIndex].position;
        Vector3 nextWaypoint = myWaypoints[nextWaypointIndex].position;

        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distnaceBeetwenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        
        return distnaceBeetwenPoints > distanceToNextWaypoint;
    }

    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;

     private void CollectTotalDistance()
    {
        for (int i = 0; i < myWaypoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);
            totalDistance = totalDistance + distance;
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        Vector3 directionToTarget = newTarget - transform.position;
        directionToTarget.y = 0;

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            // waypointIndex = 0;
            return transform.position;
        }

        Vector3 targetPoint = myWaypoints[nextWaypointIndex].position;

        if (nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(myWaypoints[nextWaypointIndex].position, myWaypoints[nextWaypointIndex - 1].position);
            totalDistance = totalDistance - distance;
        }

        nextWaypointIndex = nextWaypointIndex + 1;
        currentWaypointIndex = nextWaypointIndex - 1;

        return targetPoint;
    }

    public Vector3 CenterPoint() => centerPoint.position;
    public EnemyType GetEnemyType() => enemyType;
    
    public void TakeDamage(int damage)
    {
        healthPoints = healthPoints - damage;

        if (healthPoints <= 0)
            Die();
    }

    private void Die()
    {
        myPortal.RemoveActiveEnemy(gameObject);
        gameManager.UpdateCurrency(1);
        Destroy(gameObject);
    }

    public void DestroyEnemy()
    {
        myPortal.RemoveActiveEnemy(gameObject);
        Destroy(gameObject);
    }
}
