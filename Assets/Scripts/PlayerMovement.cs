using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Player Health Things")]
    private float playerHealth = 1000f;
    private float presentHealth;


    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint;
    public float currentPlayerSprint = 0f;


    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController character;
    public float gravity = -9.81f;



    [Header("Player Jumping and velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public Transform surfacecheck;

    bool onSurface;
    public float surfaceDistance = 0.4f;

    public LayerMask surfaceMask;

    Vector3 velocity;

    public float jumpRange = 1f;

    public Animator animator;

  

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        onSurface = Physics.CheckSphere(surfacecheck.position, surfaceDistance, surfaceMask);

        if(onSurface && velocity.y<0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity*Time.deltaTime);

        playerMove();

        Jump();
        Sprint();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            animator.SetBool("AnimWalk", false);
            animator.SetBool("IdleAnim", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            character.Move(direction.normalized * playerSpeed * Time.deltaTime);

            currentPlayerSpeed = playerSpeed;
        }

        else
        {
            animator.SetBool("Idle", true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("AnimWalk", false);
            currentPlayerSpeed = 0f;
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if(presentHealth<=0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Object.Destroy(gameObject);
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)

        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetTrigger("Jump");
                animator.SetBool("Walk", false);
               
                animator.SetBool("AnimWalk", false);


                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                character.Move(direction.normalized * playerSprint * Time.deltaTime);

                currentPlayerSprint = playerSprint;
            }
            else
            {
              
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                currentPlayerSprint = 0f;
            }
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Jump");
      
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }
}
