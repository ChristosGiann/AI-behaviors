using UnityEngine;
using UnityEngine.AI;

public class SphereFollowerNavMesh : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public float followDistance = 5.0f;  // distance from player
    public float detectionRange = 6.0f;  // distance the player can interact with the ally

    private bool isFollowing = false;  // check if the ally is following the player
    private NavMeshAgent agent;  // Reference to ally nav mesh agent

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // check player distance to the ally

        if (distanceToPlayer <= detectionRange && Input.GetKeyDown(KeyCode.E)) //if player in range and presses E, checks if ally is following or not and does the opposite
        {
            isFollowing = !isFollowing;
        }

        if (isFollowing)
        {
            FollowPlayer();
        }
    }
    private void FollowPlayer() //set agent destination to player position
    {
        Vector3 targetPosition = player.position - player.forward * followDistance;

        agent.SetDestination(targetPosition);
    }
}
