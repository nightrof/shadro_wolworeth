using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovementController : MonoBehaviour {
    float rot = 0;
    const float rotSpeed = 180;

    float verticalVelocity;
    const float gravity = -9.8f;
    const float jumpForce = 11f;
    const float gravityJump = 15f;

    float speed;
    const float walkSpeed = 8f;
    const float runningSpeed = 12f;
    const float backwardSpeed = 6f;

    Vector3 moveDir = Vector3.zero;
    Vector3 moving;
    Vector3 forward = new Vector3 (0, 0, 1);
    Vector3 backward = new Vector3 (0, 0, -1);
    Vector3 notMoving = Vector3.zero;

    CharacterController charController;
    Animator anim;
    int jumpHash = Animator.StringToHash ("Base Layer.Jumping");

    bool running;
    bool isMoving;
    Camera cam;
    int isBackward = 0;
    float shootForce = 20f;
    public Transform arrowSpawnPos;
    public GameObject arrowPrefab;

    float lastJump = 0f;
    float nextAttack = 0f;
    float attackDelay = 1.3f;

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

    /*
        faz o personagem nascer em algum aleatório válido no mapa
     */
    void RandomSpawnPosition () {
        var x = Random.Range (3, -40);
        var y = 15;
        var z = Random.Range (70, 40);
        transform.position = new Vector3 (x, y, z);
        transform.rotation = Quaternion.identity;
    }

    void Start () {
        Debug.Log(Time.time);
        SetComponents ();
        RandomSpawnPosition ();
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

/*         moveDir = Vector3.ClampMagnitude (moveDir, speed);
         */
    }

    void Update () {
        Movement ();
        CheckIfAttacking ();
        CheckIfAnimationIsJumping ();
        //Death();
        /*         anim.SetInteger("condition", 8);
                Destroy(gameObject, 3);
         */
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
                    moving = forward;
                    isBackward = 0;
                }
                if (Input.GetKey (KeyCode.S)) {
                    moving = backward;
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
                moveDir = notMoving;
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
    /*
        controla toda a entrada de dados referente à locomoção do personagem
     */
    void Movement () {
        HandleGroundMove();
        HandleJump();
        HandleRotation();
        MoveChar();
    }

    void CheckIfAttacking () {
        if (charController.isGrounded) {
            if (Input.GetMouseButtonDown (0) && Time.time > nextAttack) {
                nextAttack = Time.time + attackDelay;
                Attacking();
            }
        }
    }


    void Attacking () {
        StartCoroutine (AttackRoutine ());
        Shot();
    }

    void Shot () {
        GameObject go = Instantiate (arrowPrefab, arrowSpawnPos.position, Quaternion.identity);
        //go.transform.SetParent(transform);
        Rigidbody rb = go.GetComponent<Rigidbody> ();
        go.GetComponent<Arrow>().parentID = gameObject.GetInstanceID();
        rb.velocity = arrowSpawnPos.forward * shootForce;
    }

    IEnumerator AttackRoutine () {
        anim.SetBool ("attacking", true);
        anim.SetInteger ("condition", 2);
        yield return new WaitForSeconds (1);
        anim.SetInteger ("condition", 0);
        anim.SetBool ("attacking", false);
    }
}