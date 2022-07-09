using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

    [Header("camera to assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;


    [Header("Camera Animator")]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("Aimwalk", true);
            animator.SetBool("Walk", true);


            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
        }

        else if(Input.GetButton("fire2"))
        {

            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("Aimwalk", false);
            animator.SetBool("Walk", false);


            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
        }
        else
        {

            animator.SetBool("Idle", true);
            animator.SetBool("IdleAim", false);
            animator.SetBool("Aimwalk", false);


            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
        }
    }
}
