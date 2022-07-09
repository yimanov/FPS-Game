using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;





public class Enemy : MonoBehaviour
{

   


    [Header("Enemy Health and Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float enemySpeed;

    [Header("Enemy Things")]

    public NavMeshAgent enemyAgent;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public Transform LookPoint;
    public GameObject ShootingRaycastArea;
 
    public Transform spawn;
    public Transform EnemyCharachter;

    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;


    [Header("Enemy States")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool isPlayer = false;



    [Header("Enemy Animation and Spark Effect")]

    public Animator animator;


    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        presentHealth = enemyHealth;


    }

    // Update is called once per frame
    void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        if (playerInvisionRadius && !playerInshootingRadius) Pursueplayer();
        if (playerInvisionRadius && playerInshootingRadius) ShootPlayer();

    }

    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);

        if(!previouslyShoot)
        {
            RaycastHit hit;

            if(Physics.Raycast(ShootingRaycastArea.transform.position,ShootingRaycastArea.transform.forward,out hit,shootingRadius))
            {
                PlayerMovement playerBody = hit.transform.GetComponent<PlayerMovement>();

                if(playerBody!=null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
            }


            animator.SetBool("Running", false);
            animator.SetBool("Shooting", true);


        }

        previouslyShoot = true;
        Invoke(nameof(ActiveShooting), timebtwShoot);
    }

    public void ActiveShooting()
    {
        previouslyShoot = false;  
    }
    private void Pursueplayer()
    {
        if(enemyAgent.SetDestination(playerBody.position))
        {
            animator.SetBool("Running", true);
            animator.SetBool("Shooting", false);

        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("Shooting", false);
        }
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 1f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInvisionRadius = false;
        playerInshootingRadius = false;
        animator.SetBool("Die", true);
        animator.SetBool("Running", true);
        animator.SetBool("Shooting", false);


        yield return new WaitForSeconds(5f);

        presentHealth = 120f;
        enemySpeed = 3f;
        visionRadius = 100f;
        shootingRadius = 10f;
        playerInvisionRadius = true;
        playerInshootingRadius = false;

        animator.SetBool("Die", false);
        animator.SetBool("Running", true);
        EnemyCharachter.transform.position = spawn.transform.position;

        Pursueplayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

   
}
