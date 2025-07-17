using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator anim;

    [Header("Wandering")]
    public bool pause = false;
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;
    private float wanderTimer;
    public float wanderSpeed = 2f;

    [Header("Chasing")]
    public bool chasing = false;
    public Transform player;
    public float chaseSpeed = 4f;
    public float attackDistance = 2f;
    public float stopDistance = 15f;

    private Vector3 destination;

    [Header("Raycast")]
    public Transform raycastPoint;
    public float raycastDistance = 10f;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        wanderTimer = wanderInterval;
        agent.speed = wanderSpeed;
    }

    void Update()
    {
        if (!pause)
        {
            if (chasing)
            {
                ChasePlayer();
            }
            else
            {
                Wander();
            }
        }


        //Raycast
        Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            //Debug.Log("Ray hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Enemy sees the player!");
                chasing = true;
            }

            Debug.DrawRay(raycastPoint.position, raycastPoint.forward * hit.distance, Color.red); 
        }
    }

    void ChasePlayer()
    {
        ResetAnimation();
        anim.SetBool("isRunning", true);

        agent.speed = chaseSpeed;
        if (player != null)
        {
            agent.SetDestination(player.position);

            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackDistance)
            {
                StealItem();
            }
            
            if (dist >= stopDistance)
            {
                Debug.Log("Stop chasing");
                chasing = false;
            }
        }
    }

    void Wander()
    {
        ResetAnimation();
        anim.SetBool("isWalking", true);

        agent.speed = wanderSpeed;
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f || agent.remainingDistance < 0.5f)
        {
            Vector3 newPos = RandomNavMeshLocation(wanderRadius);
            agent.SetDestination(newPos);
            wanderTimer = wanderInterval;
        }
    }

    void StealItem()
    {
        agent.SetDestination(transform.position);
        anim.SetBool("isRunning", false);
        Debug.Log("Attack player");

        GameObject item = player.GetComponent<PlayerInteraction>().pickedUpGO;

        if (item != null)
        {
            pause = true;

            item.GetComponent<Pickup>().StealAndDestroy(gameObject.transform.Find("Item Holder"));
        }
    }

    void ResetAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", false);
    }

    Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position; // fallback if no valid point found
    }

    void OnDrawGizmos()
    {
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(agent.destination, 0.3f);
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }

    public void UnPause()
    {
        pause = false;
    }

}
