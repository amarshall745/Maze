using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCover : MonoBehaviour
{
    private bool chasing, reloading;
    public float distanceToChase, distanceToWalk, distanceToLose, distanceToStop;

    private Vector3 targetPoint, startingPoint, newTargetPoint;

    public NavMeshAgent agent;

    public float keepChasingTime = 5f;
    private float chaseCounter;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweeenShtots = 2f, timeToShoot = 1f;
    private float fireCount, shotWaitCounter, shootTimeCounter;

    public Animator anim;

    // Ammo system
    public int maxAmmo = 6; // Max bullets before reload
    private int currentAmmo;

    public float reloadTime = 1f; // Time per reload animation

    // Cover
    public Vector3 coverPosition = new Vector3(0, 0, 0);
    public GameObject coverGO;
    public bool coverAvailable;

    void Start()
    {
        startingPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweeenShtots;

        currentAmmo = maxAmmo; // Start with full ammo
    }

    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!reloading)
        {
            if (!chasing)
            {
                if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
                {
                    chasing = true;
                    shootTimeCounter = timeToShoot;
                    shotWaitCounter = waitBetweeenShtots;
                }

                if (chaseCounter > 0)
                {
                    chaseCounter -= Time.deltaTime;
                    if (chaseCounter <= 0)
                    {
                        agent.destination = startingPoint;
                    }
                }

                anim.SetBool("isMoving", agent.remainingDistance >= 0.25f);
            }
            else
            {
                if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
                {
                    if(Vector3.Distance(transform.position, targetPoint) < distanceToWalk)
                    {
                        //agent.destination = transform.position - transform.right * 2f;
                        //agent.speed = 2f;
                        //transform.LookAt(PlayerController.instance.transform.position);
                        //agent.destination = transform.position - transform.right * 2f;
                        //anim.SetBool("leftWalk", true);
                       // Debug.Log("DO somehting");
                    }
                    else
                    {
                        agent.destination = targetPoint;
                        anim.SetBool("leftWalk", false);
                    }
                }
                else
                {
                    
                    agent.destination = transform.position;
                    //console.log("moving backwards");
                    //Vector3 directionAway = transform.position + (transform.position - PlayerController.instance.transform.position).normalized * 2f;
                    //agent.destination = directionAway;
                }
                anim.SetBool("leftWalk", false);

                if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
                {
                    chasing = false;
                    chaseCounter = keepChasingTime;
                }

                if (shotWaitCounter > 0)
                {
                    shotWaitCounter -= Time.deltaTime;
                    if (shotWaitCounter <= 0)
                    {
                        shootTimeCounter = timeToShoot;
                    }
                    anim.SetBool("isMoving", true);
                }
                else
                {
                    if (PlayerController.instance.gameObject.activeInHierarchy)
                    {
                        if (currentAmmo > 0)
                        {
                            shootTimeCounter -= Time.deltaTime;
                            if (shootTimeCounter > 0)
                            {
                                fireCount -= Time.deltaTime;

                                if (fireCount <= 0)
                                {
                                    fireCount = fireRate;

                                    firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 0.4f, 0));

                                    Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                                    float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                                    if (Mathf.Abs(angle) < 30f)
                                    {
                                        Instantiate(bullet, firePoint.position, firePoint.rotation);
                                        anim.SetTrigger("fireShot");
                                        currentAmmo--; // Reduce ammo

                                        if (currentAmmo <= 0)
                                        {
                                            StartCoroutine(Reload()); // Start reload process
                                            reloading = true;
                                        }
                                    }
                                    else
                                    {
                                        shotWaitCounter = waitBetweeenShtots;
                                    }
                                }

                                agent.destination = transform.position;
                            }
                            else
                            {
                                shotWaitCounter = waitBetweeenShtots;
                            }
                        }
                    }
                    anim.SetBool("isMoving", false);
                }
            }
        }
    }

    IEnumerator Reload()
    {
        for (int i = 0; i < maxAmmo-2; i++) // Play reload 6 times
        {
            anim.SetTrigger("reload"); // Trigger reload animation
            yield return new WaitForSeconds(reloadTime); // Wait for reload time
        }
        reloading = false;
        currentAmmo = maxAmmo; // Refill ammo after reload
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.tag == "Cover")
        {
            coverAvailable = true;
            transform.position = coverPosition;
            coverGO = other.gameObject;
            Debug.Log("Cover near enemy");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cover")
        {
            coverAvailable = false;
            coverGO = null;
            Debug.Log("Cover left enemy");
        }
    }
}
