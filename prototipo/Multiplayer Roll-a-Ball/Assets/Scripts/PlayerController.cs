using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float jumpSpeed;
    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float rotateSpeed;
    [HideInInspector]
    public float current_speed;


    private Vector3 moveDirection;
    private Vector3 lastPosition;
    private CharacterController controller;

    private void Awake()
    {
        Camera.main.GetComponent<CameraController>().target = transform;
        controller = GetComponent<CharacterController>();
    }
    
    void RandomSpawnPosition(){
        var x = Random.Range(3, -40);
        var y = 15;
        var z = Random.Range(70, 40);
        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.identity;
        Debug.Log("Entrou" + "x="+x +";y="+ y +";z="+ z);
        Debug.Log(transform.position);
    }

    void InitialValues(){
        speed = 18.0F;
        current_speed = 0;

        jumpSpeed = 30.0F;
        gravity = 520.0F;
        rotateSpeed = 5.0F;

        moveDirection = Vector3.zero;
        
        lastPosition = Vector3.zero;
    }

    void Start()
    {
        InitialValues();
        RandomSpawnPosition();
    }
    
    void Update()
    {
/*         if (controller.isGrounded)
        {
 */
        moveDirection = new Vector3(0f, 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        if (Input.GetButton("Jump"))
            moveDirection.y = jumpSpeed;
/*         moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

 */
/*         moveDirection.y -= gravity * Time.deltaTime;
 */        
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        //Rotate Player
        transform.Rotate(0, Input.GetAxis("Horizontal")*rotateSpeed, 0);
/* 
        current_speed = (((transform.position - lastPosition).magnitude) / Time.deltaTime);
        lastPosition = transform.position;
 */
    }

}
