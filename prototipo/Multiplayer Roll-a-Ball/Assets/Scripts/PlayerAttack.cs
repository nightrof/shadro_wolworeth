using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    CharacterController charController;
    Animator anim;
    Camera cam;
    public Transform arrowSpawnPos;
    public GameObject arrowPrefab;

    int jumpHash = Animator.StringToHash ("Base Layer.Jumping");
    bool running;
    bool isMoving;
    int isBackward = 0;
    float shootForce = 20f;
    float nextAttack = 0f;
    float attackDelay = 1.3f;
    int playerID;

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
        SetComponents ();
        playerID = gameObject.GetInstanceID();
    }

    void Update () {
        CheckIfAttacking ();
    }

    void CheckIfAttacking () {
        //if (charController.isGrounded) {
            if (Input.GetMouseButtonDown (0) && Time.time > nextAttack) {
                nextAttack = Time.time + attackDelay;
                Attacking();
            }
        //}
    }


    void Attacking () {
        StartCoroutine (AttackRoutine ());
        Shot();
    }

    void Shot () {
        GameObject go = Instantiate (arrowPrefab, arrowSpawnPos.position, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody> ();
        go.GetComponent<Arrow>().parentID = playerID;
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