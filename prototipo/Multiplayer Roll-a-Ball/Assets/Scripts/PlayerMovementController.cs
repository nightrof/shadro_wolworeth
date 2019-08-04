using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    CharacterController charController;
    Animator anim;
    Camera cam;

    float rot = 0;
    const float rotSpeed = 180;
    Quaternion targetRotation;
    
    float verticalVelocity;
    const float gravity = -9.8f;
    const float jumpForce = 11f;
    const float gravityJump = 15f;

    float speed;
    const float walkSpeed = 8f;
    const float runningSpeed = 12f;
    const float backwardSpeed = 6f;
    float forwardInput, turnInput;

    Vector3 moveDir = Vector3.zero;
    Vector3 moving;

    int jumpHash = Animator.StringToHash ("Base Layer.Jumping");

    bool running;
    bool isMoving;
    int isBackward = 0;
    float lastJump = 0f;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    /*
        seta os componentes necessários
        charController = charController
            p mover o personagem, checar se ta no chão etc
        anim = animator
            p poder manipular as animaçoes de acordo com a movimentaçao etc
        cam = cameracontroller (script)
            dessa forma, assim que o player é spawnado, ele detecta qual é a camera existente e assinala que é nele que a camera deve focar
     */
    void SetComponents () {
        charController = GetComponent<CharacterController> ();
        anim = GetComponent<Animator> ();
        cam = Camera.allCameras[0];
        cam.GetComponent<Camera> ().GetComponent<CameraController> ().target = transform;
    }

    void Start () {
        Debug.Log(Time.time);
        SetComponents ();
    }

    /*
        checa se o personagem tá com a animação "jumping" e, ao mesmo tempo, está no chão
        e, então, corrige a animação pra Idle, caso seja essa a situaçao
     */
    void CheckIfAnimationIsJumping () {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
        var currentTime = Time.time;
        if (stateInfo.nameHash == jumpHash && (lastJump + 1.25f) <= currentTime) {
            anim.SetInteger ("condition", 0);
            anim.SetBool ("jumping", false);
        }
    }

    /*
        seta a animação de corrida, caminhada e andar pra trás
            flag "isBackward" é justamente isso
                caso a animação seja de caminhar, isBackward = 0
                caso a animação seja de andar p trás, isBackward = 3

                obs:
                    condition = 1 -> andar p frente
                    condition = 4 -> andar p trás
        além disso seta o vetor de moveDir (utilizado pelo charController p movimentar o personagem) com moving (que pode ter o valor "forward"(0,0,1) ou "backward"(0,0,-1))
     */
    void Move () {

        if (running && isMoving) {
            anim.SetInteger ("condition", 3);
        } else if(isMoving){
            anim.SetInteger ("condition", 1 + isBackward);
        }

        moveDir *= speed;
        moveDir.y = gravity;

        moveDir = transform.TransformDirection (moveDir * Time.deltaTime);
    }

    void Update () {
        Movement ();
        CheckIfAnimationIsJumping ();
    }

    /*
        controla toda a entrada de dados referente à locomoção do personagem
     */
    void Movement () {
        HandleGroundMove();
        HandleJump();
        HandleRotation();
        MoveChar();
    }

    void HandleRotation () {
        if(charController.isGrounded)
        {
            rot += Input.GetAxis ("Horizontal") * rotSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3 (0, rot, 0);
        }
    }

    void HandleGroundMove () {
        if (charController.isGrounded) {
/*             anim.SetInteger ("condition", 0);
            moveDir = notMoving; */
            isMoving = false;
            speed = walkSpeed;
            running = false;
            if (Input.GetKey (KeyCode.LeftShift)) {
                speed = runningSpeed;
                running = true;
            }

            if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
                if (Input.GetKey (KeyCode.W)) {
                    moving = Vector3.forward;
                    isBackward = 0;
                }
                if (Input.GetKey (KeyCode.S)) {
                    moving = Vector3.back;
                    isBackward = 3;

                    speed = backwardSpeed;
                    running = false;
                }

                if (anim.GetBool ("attacking") == true) {
                    return;
                }

                moveDir = moving;
                isMoving = true;
            }

            if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S)) {
                anim.SetInteger ("condition", 0);
                moveDir = Vector3.zero;
                isMoving = false;
            }
            Move ();
        }
    }

    void HandleJump () {
        // verticalVelocity = 0;
        if (charController.isGrounded) {
            verticalVelocity = -gravityJump * Time.deltaTime;
            if (Input.GetKeyDown (KeyCode.Space)) {
                anim.SetInteger ("condition", 5);
                anim.SetBool ("jumping", true);
                verticalVelocity = jumpForce;
                lastJump = Time.time;
                Debug.Log(Time.time);
            }
        } else {
            verticalVelocity -= gravityJump * Time.deltaTime;;
        }

        moveDir.y = verticalVelocity * Time.deltaTime;
    }

    void MoveChar() {
        charController.Move (moveDir);    
    }

}