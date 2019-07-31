﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovementController : MonoBehaviour 
{
    float speed = 4;
    float rotSpeed = 180;
    float rot = 0;
    float gravity = 14;
    float jumpSpeed = 10;

    Vector3 moveDir = Vector3.zero;
    Vector3 moving;
    Vector3 forward = new Vector3 (0,0,1);
    Vector3 backward = new Vector3 (0,0,-1);    
    Vector3 notMoving = Vector3.zero;

    CharacterController controller;
    Animator anim;
    int jumpHash = Animator.StringToHash("Base Layer.Jumping");

    bool running;
    Camera cam;
    int isBackward = 0;
    float shootForce = 20f;
    public Transform arrowSpawnPos;
    public GameObject arrowPrefab;
    // Start is called before the first frame update
    void Start () 
    {
        SetComponents ();
        //RandomSpawnPosition ();
    }

    void SetComponents () 
    {
        controller = GetComponent<CharacterController> ();
        anim = GetComponent<Animator> ();
        cam = Camera.allCameras[0];
        cam.GetComponent<Camera>().GetComponent<CameraController>().target = transform;
    }

    void RandomSpawnPosition () 
    {
        var x = Random.Range (3, -40);
        var y = 15;
        var z = Random.Range (70, 40);
        transform.position = new Vector3 (x, y, z);
        transform.rotation = Quaternion.identity;
    }
    void CheckIfJumping ()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.nameHash == jumpHash && controller.isGrounded)
        {
            anim.SetInteger("condition", 0);
            anim.SetBool("jumping", false);
        }

    }
    void Move()
    {
        if (running)
        {
            anim.SetInteger("condition", 3);
        }
        else
        {
            anim.SetInteger("condition", 1+isBackward);
        }
        moveDir = moving;
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
    }

    void Update () 
    {
        Movement();
        GetInput();
        CheckIfJumping();
        //Death();
/*         anim.SetInteger("condition", 8);
        Destroy(gameObject, 3);
 */    
    }

    void Movement()
    {
        if (controller.isGrounded) 
        {

            if (Input.GetKey(KeyCode.LeftShift)) 
            {
                speed = 8f;
                running = true;
            }
            else
            {
                speed = 4f;
                running = false;
            } 

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                
                if (Input.GetKey(KeyCode.W))
                {
                    moving = forward;
                    isBackward = 0;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    speed = 2f;
                    moving = backward;
                    running = false;
                    isBackward = 3;
                }

                if (anim.GetBool("attacking") == true)
                {
                    return;
                } 
                else if (anim.GetBool("attacking") == false)
                {
                    Move();
                }
            }

            if (Input.GetButton("Jump"))
            {
                anim.SetInteger("condition", 5);
                moveDir.y = jumpSpeed;
                anim.SetBool("jumping", true);
            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            anim.SetInteger("condition", 0);
            moveDir = notMoving;
            if (anim.GetBool("jumping") == true) 
            {
                moveDir.y = jumpSpeed;
            }
        }

        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot, 0);
        
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }
    
    void GetInput()
    {
        if(controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attacking();
                Shot();
            }
        }
    }

    void Shot()
    {
        GameObject go = Instantiate(arrowPrefab, arrowSpawnPos.position, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = cam.transform.forward * shootForce;
    }

    void Attacking()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        anim.SetBool("attacking", true);
        anim.SetInteger("condition", 2);
        yield return new WaitForSeconds(1);
        anim.SetInteger("condition", 0);
        anim.SetBool("attacking", false);
    }
}