using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;
        public float rotateVel = 100;        
        public float jumpVel = 25;
        // tolerancia p considerar como no chão
        public float distToGrounded = 0.8f;
        // especificar objetos cujo os quais possibilitam pular ou nao
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();    

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, turnInput, jumpInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position,
                               Vector3.down,
                               moveSetting.distToGrounded,
                               moveSetting.ground);
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        targetRotation = transform.rotation;
        if(GetComponent<Rigidbody>()){
            rb = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("no rigidbody!");
        }

        jumpInput = forwardInput = turnInput = 0;

        Camera cam = Camera.allCameras[0];
        cam.GetComponent<Camera> ().GetComponent<CameraController> ().target = transform;

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        GetInput();
        Turn();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Run();
        Jump();        
        rb.velocity = transform.TransformDirection(velocity);
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); // (-1 ou 1)
        
    }

    bool CheckDelay (float input) {
        if (Mathf.Abs(input) > inputSetting.inputDelay)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    void Run ()
    {
        if (CheckDelay(forwardInput))
        {
            //move
            velocity.z = moveSetting.forwardVel * forwardInput;
        }
        else
        {
            velocity.z = 0;
        }
    }

    void Jump ()
    {
        if (jumpInput > 0 && Grounded())
        {
            //jumping
            velocity.y = moveSetting.jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            //grounded
            velocity.y = 0;
        }
        else
        {
            //falling
            velocity.y -= physSetting.downAccel;
        }
    }

    void Turn ()
    {
        if (CheckDelay(turnInput))
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime,
                                                   Vector3.up);

        }
        transform.rotation = targetRotation; 
    }
}
