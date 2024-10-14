using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints; // Patrol points of the enemy
    public float waitTime = 0.3f; // Enemy checks before continuing patrol
    public float visionRange = 10.0f; 
    public float visionAngle = 60.0f; 
    public Transform player; // Reference to player

    private int currentWaypoint = 0; // Current patrol point
    private float waitTimer; // Timer for the patrol time
    private bool isChasing = false; // Checks if the enemy chases the player 
    private NavMeshAgent agent; // Reference to the enemy's nav mesh agent

    
    private LineRenderer lineRenderer; // Reference to enemy's vision for graphical presentation
    public int fovResolution = 30; // Points to render the field of view

    private void Start()
    {
        
        agent = GetComponent<NavMeshAgent>(); // Use of nav mesh agent component for enemy movement

        waitTimer = waitTime; // Timer using the requested patrol check time

        lineRenderer = GetComponent<LineRenderer>(); // LineRenderer to depict enemy vision
        lineRenderer.positionCount = fovResolution + 2; 

        if (waypoints.Length > 0) // Set first waypoint 
        {
            currentWaypoint = 0;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            ChasePlayer(); // enemy is chasing
        }
        else
        {
            Patrol();
            CheckVision(); // enemy is patroling and checking for the player
        }

        UpdateFieldOfView(); //Update enemy graphical vision
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) //when the cycle of points finishes , we reset the function
            return;

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 1f) //enemy reaches the current waypoint
        {
            waitTimer -= Time.deltaTime; //enemy waits for the timer to run out
            if (waitTimer <= 0)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length; //proceeds to next waypoint
                agent.SetDestination(waypoints[currentWaypoint].position);
                waitTimer = waitTime;
            }
        }
    }

    private void CheckVision() //check if enemy sees the player
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRange) //check if player is on range
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            
            if (angleToPlayer <= visionAngle / 2f) // check if player is in the right angle
            {
                Debug.Log("Player spotted! Chasing...");
                isChasing = true;
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position); // set agent movement towards the player
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //check player tag
        {
            Debug.Log("Player caught!");
            RestartLevel(); //reset scene
        }
    }

    private void RestartLevel()
    {
        Debug.Log("Restarting level...");
        SceneManager.LoadScene("Gameplay");
    }

    private void UpdateFieldOfView() //update enemy graphical vision
    {
        float angleStep = visionAngle / fovResolution;
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i <= fovResolution; i++)
        {
            float currentAngle = -visionAngle / 2 + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            Vector3 fovPoint = transform.position + direction * visionRange;

            lineRenderer.SetPosition(i + 1, fovPoint);
        }
    }
}
