using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SleepingEnemyController : MonoBehaviour
{
    public enum EnemyState { Sleep, Wake } // Enemy states of existence
    public EnemyState currentState = EnemyState.Sleep; // Original enemy state

    public float visionRange = 15.0f;  // Enemy area of detection when awake
    public Transform player; // Reference to player
    public LayerMask obstacleMask; // Reference to obstacles
    public AudioSource wakeSound;  // Enemy sound to warn player
    public float chaseSpeed = 20.0f;  // Enemy movement speed
    public float rotationSpeed = 5.0f; // Rotation speed
    public float stateChangeInterval = 10.0f;  // Duration of enemy states

    private bool isChasing = false; // checks if enemy is chasing the player
    private bool isWaitingBeforeChasing = false;  // timer before enemy is starting the chase
    private bool canCheckPlayer = false; // check for when the enemy starts the detection
    private Vector3 lastPlayerPosition; // check last player movement

    private void Start()
    {
        StartCoroutine(ChangeStateAutomatically()); // change enemy states

        lastPlayerPosition = player.position; // Player original position

        if (currentState == EnemyState.Wake) // enemy warns the player through sound
        {
            wakeSound.loop = true;
            wakeSound.Play();
        }
    }

    private void Update()
    {
        if (currentState == EnemyState.Sleep) //in sleep state , enemy stays dormant
        {
            return;
        }
        else if (currentState == EnemyState.Wake && !isWaitingBeforeChasing && canCheckPlayer)
        {
            if (!wakeSound.isPlaying)
            {
                wakeSound.Play();
            }

            CheckPlayerDistanceAndMovement(); //check for player
        }

        if (isChasing)
        {
            ChasePlayer(); //if enemy detects player , it chases
        }
    }

    private void CheckPlayerDistanceAndMovement() // player detection
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRange) //check if player is on range
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask)) //check for obstacles
            {
                if (canCheckPlayer && IsPlayerMoving()) //check for player after 3 seconds , to give enough time to the player
                {
                    Debug.Log("Player spotted moving! Preparing to chase..."); //if player moves , enemy chases
                    StartCoroutine(WaitBeforeChasing());  
                }
            }
            else
            {
                isChasing = false; //if there is obstacles , enemy stops chasing
            }
        }
        else
        {
            isChasing = false; //if player not in range , dont chase
        }
    }

    private bool IsPlayerMoving() // check player movement
    {
        Vector3 playerMovement = player.position - lastPlayerPosition;
        lastPlayerPosition = player.position;  // update player placement

        return playerMovement.magnitude > 0.01f; // check if the player moved
    }

    private IEnumerator WaitBeforeChasing()
    {
        isWaitingBeforeChasing = true;  //waits before starting to chase
        yield return new WaitForSeconds(1.0f);  
        isChasing = true;  //enemy start chasing
        isWaitingBeforeChasing = false; 
    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); //rotate towards player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime); // move towards the player
    }

    private IEnumerator ChangeStateAutomatically()
    {
        while (true)
        {
            yield return new WaitForSeconds(stateChangeInterval);  // change enemy state based on timer

            if (currentState == EnemyState.Sleep)
            {
                SetState(EnemyState.Wake);
            }
            else
            {
                SetState(EnemyState.Sleep);
            }
        }
    }
    public void SetState(EnemyState newState) // function to change enemy states
    {
        currentState = newState;

        if (newState == EnemyState.Wake)
        {
            Debug.Log("Enemy is waking up...");
            if (!wakeSound.isPlaying)
            {
                wakeSound.Play();  // awake warning sound
            }
            StartCoroutine(WaitBeforeCheckingPlayer()); // wait time before player detection
        }
        else if (newState == EnemyState.Sleep)
        {
            Debug.Log("Enemy is going to sleep...");
            isChasing = false;  // when enemy goes to sleep , stop chasing
            canCheckPlayer = false;  // when enemy goes to sleep , stop enemy detection 
            if (wakeSound.isPlaying)
            {
                wakeSound.Stop();  // stop warning sound when enemy is asleep
            }
        }
    }

    private IEnumerator WaitBeforeCheckingPlayer()
    {
        canCheckPlayer = false;  // unable enemy detection to give time to the player to react
        yield return new WaitForSeconds(3.0f); 
        lastPlayerPosition = player.position;  // take player position after the given time
        canCheckPlayer = true;  // enable enemy detection
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check tag for player
        {
            Debug.Log("Player caught!");
            RestartLevel(); 
        }
    }

    private void RestartLevel() // reset scene
    {
        Debug.Log("Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
