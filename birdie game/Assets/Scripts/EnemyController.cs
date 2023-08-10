using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public Transform player;
    public float chaseRange = 5f;
    public float chaseSpeed = 5f;
    public float maxChaseTime = 10f;

    private int currentWaypointIndex = 0;
    private bool isChasing = false;
    private float chaseTimer = 0f;
    private float returnTimer = 0f;
    private bool isReturning = false;
    private Vector3 patrolPosition;

    void Start()
    {
        patrolPosition = transform.position;
    }

    void Update()
    {
        if (isChasing)
        {
            Chase();
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= maxChaseTime)
            {
                StopChasing();
            }
        }
        else
        {
            if (isReturning)
            {
                ReturnToPatrol();
                returnTimer += Time.deltaTime;

                if (returnTimer >= CalculateTimeToWaypoint())
                {
                    isReturning = false;
                    returnTimer = 0f;
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned to the enemy!");
            return;
        }

        if (!isChasing)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                StopChasing();
            }

            // Make the enemy look at the waypoint with a 90-degree offset
            transform.LookAt(targetWaypoint);
            transform.Rotate(Vector3.up, 90f);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < chaseRange)
            {
                isChasing = true;
                chaseTimer = 0f;
                patrolPosition = transform.position;
            }
        }
    }



    void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            Vector3 chaseDirection = (player.position - transform.position).normalized;
            chaseDirection.y = 0f;

            transform.position += chaseDirection * chaseSpeed * Time.deltaTime;

            // Calculate the rotation towards the chase direction with a 90-degree offset
            Quaternion newRotation = Quaternion.LookRotation(chaseDirection, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);
            transform.rotation = newRotation;
        }
        else
        {
            StopChasing(); // Call this method to reset the enemy's state
        }
    }


    void StopChasing()
    {
        isChasing = false;
        chaseTimer = 0f;

        int nearestWaypointIndex = FindNearestWaypointIndex();
        currentWaypointIndex = nearestWaypointIndex;
        isReturning = true;
    }



    void ReturnToPatrol()
    {
        Vector3 moveDirection = (patrolPosition - transform.position).normalized;

        // Check if the moveDirection is very small or zero
        if (moveDirection.sqrMagnitude > 0.01f) // Adjust the threshold as needed
        {
            // Make the enemy look at the patrol position with a 90-degree offset
            transform.LookAt(patrolPosition);
            transform.Rotate(Vector3.up, 90f);
        }

        transform.position = Vector3.MoveTowards(transform.position, patrolPosition, moveSpeed * Time.deltaTime);
    }





    int FindNearestWaypointIndex()
    {
        int nearestIndex = 0;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < minDistance)
            {
                nearestIndex = i;
                minDistance = distance;
            }
        }
        return nearestIndex;
    }

    float CalculateTimeToWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        return distanceToWaypoint / moveSpeed;
    }
}
