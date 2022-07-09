using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{

    [Header("Rifle")]
    public Camera cam;
    public float giveDamage = 10f;
    public float shootInRange = 100f;
    public float fireCharge = 15f;
    public PlayerMovement player;
    public Animator animator;
  

    [Header("Rifle Ammunition and shooting")]
    private float nextTimeShoot = 0f;
    private int maximumAmunition = 20;
    private int mag = 15;
    private int presentAmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;


  [Header("Rifle effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    public GameObject goreEffect;


    private void Awake()
    {
        presentAmunition = maximumAmunition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setReloading)
            return;

        if(presentAmunition<=0)
        {
            StartCoroutine(Reload());
            return;
        }


        if (Input.GetButton("Fire1") && Time.time>=nextTimeShoot)

        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeShoot = Time.time + 1f / fireCharge;
            Shoot();
        }

        else if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
          
            animator.SetBool("Firewalk", true);
            
        }

        else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("Firewalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("Firewalk", false);
           
        }
    }

    void Shoot()
    {

        if(mag==0)
        {

        }

        presentAmunition--;

        if(presentAmunition==0)
        {
            mag--;
        }


        muzzleSpark.Play();
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootInRange))
        {
            Objects objects = hitInfo.transform.GetComponent<Objects>();

            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (objects != null)
            {
                objects.objectHitDamage(giveDamage);
                GameObject WoodGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
            }
            else if(enemy!=null)
            {
                enemy.enemyHitDamage(giveDamage);

                GameObject goreGO = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreGO, 1f);
            }
        }  
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;

        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadingTime);

        animator.SetBool("Reloading", false);
        presentAmunition = maximumAmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;

    }
}
