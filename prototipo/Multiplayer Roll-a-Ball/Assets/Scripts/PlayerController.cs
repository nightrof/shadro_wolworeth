using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform arms;
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
        public float mouseSensitivity = 3;
        public float smoothRot = 100f;
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
        public string TURN_SIDEWAYS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();    

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, sidewaysInput, jumpInput, mouseX, mouseY, lockInput;
    Camera cam;
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

        mouseX = mouseY = jumpInput = forwardInput = sidewaysInput = lockInput = 0;

        cam = Camera.allCameras[0];
        cam.GetComponent<Camera> ().GetComponent<CameraController>().target = arms.transform;
    }

/*     /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
    } */

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        GetInput();
        CursorState();
        Run();
        Jump();
        RotatePlayer();
        WalkSideways();        
        rb.velocity = transform.TransformDirection(velocity);
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        sidewaysInput = Input.GetAxis(inputSetting.TURN_SIDEWAYS);
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); // (-1 ou 1)
        lockInput = Input.GetAxisRaw("Cancel");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");        
    }

    void CursorState()
    {
        if (lockInput > 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
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

    void WalkSideways ()
    {
        if (CheckDelay(sidewaysInput))
        {
            //move
            velocity.x = moveSetting.forwardVel * sidewaysInput;
        }
        else
        {
            velocity.x = 0;
        }
    }

    void RotatePlayer()
    {
        RotateHorizontally();
        RotateVertically();
    }

    void RotateHorizontally ()
    {
        float rotAmoutX = mouseX * moveSetting.mouseSensitivity;
        Vector3 rotPlayer = transform.rotation.eulerAngles;
        rotPlayer.y += rotAmoutX;
        SmoothRotation(transform, Quaternion.Euler(rotPlayer));
    }
    Vector3 camVel = Vector3.zero;
    void RotateVertically ()
    {
        float rotAmountY = mouseY * 0.15f;

        Vector3 posPlayerArms = arms.transform.localPosition;
        posPlayerArms.x = 0.05f; //0.40f;
        posPlayerArms.z = 0.9f;
        posPlayerArms.y += rotAmountY;
        if (posPlayerArms.y < 0.02f)
        {
            posPlayerArms.y = 0.02f;
        }
        if (posPlayerArms.y > 2.8f)
        {
            posPlayerArms.y = 2.8f;
        }
        arms.transform.localPosition = Vector3.SmoothDamp(arms.transform.localPosition, posPlayerArms, ref camVel, 0.05f);
/* 
0.02 --  24
2.80 -- -50
1.39 -- -13
1 -- x */

        Vector3 rotPlayerArms = arms.transform.localRotation.eulerAngles;
        rotPlayerArms.x = (74*(2.82f-posPlayerArms.y))/2.8f - 50;
        rotPlayerArms.z = 0;
        arms.transform.localRotation = Quaternion.Euler(rotPlayerArms);
        cam.GetComponent<Camera>().GetComponent<CameraController>().orbit.xRotation = 360-arms.transform.rotation.eulerAngles.x;
        Debug.Log(arms.transform.rotation.eulerAngles.x);
        //cam.transform.rotation.eulerAngles.x = arms.transform.rotation.eulerAngles.x;
    }

    void SmoothRotation (Transform t, Quaternion next)
    {
        //Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        t.rotation = Quaternion.Lerp(t.rotation, next, moveSetting.smoothRot * Time.deltaTime);
    }
}
