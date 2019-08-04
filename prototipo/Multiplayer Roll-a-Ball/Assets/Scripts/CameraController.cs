using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 3.4f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -8;
        public float zoomSmooth = 100;
        public float maxZoom = -2;
        public float minZoom = -15;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -85;
        public float vOrbitSmooth = 150; 
        public float hOrbitSmooth = 150; 
    }

    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
    }
    
    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    PlayerController _playerController;

    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        SetCameraTarget(target);
        MoveToTarget();
    }

    void SetCameraTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            if (target.GetComponent<PlayerController>())
            {
                _playerController = target.GetComponent<PlayerController>();
                
            }
            else
            {
                Debug.LogError("No PlayerController attached to target ");
            }
        }
        else
        {
            Debug.LogError("Camera needs target!");
        }
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        // Moving
        MoveToTarget();
        // Rotating
        LookAtTarget();
    }

    void MoveToTarget()
    {
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget
        ;
        destination += targetPos;
        transform.position = destination;
    }

    void Update() {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();    
    }

    void GetInput()
    {
        vOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        hOrbitSnapInput = Input.GetAxisRaw("meudeusdoceu");
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }

    void OrbitTarget()
    {
        if (hOrbitSnapInput > 0)
        {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }
        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }

    }

    void ZoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;
        if (position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }
        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }
}
/* 
    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                                  target.eulerAngles.y,
                                                  ref rotateVel,
                                                  lookSmooth);
        transform.rotation = Quaternion.Euler(xTilt, eulerYAngle, 0);
    } */
/* public Transform target;
    /
    private Vector3 offset;

    
    void LateUpdate ()
    {
        transform.position = target.transform.position + offset;
    }
    /
    public Transform target;
    private Vector3 offset;
    private Space offsetPositionSpace = Space.Self;
    private bool lookAt = false;

    public void Start ()
    {
        offset = new Vector3(1, 2.5f, -3.8f);
    }

    private void LateUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        if(target == null)
        {
            Debug.LogWarning("Missing target ref !", this);
            return;
        }

        // compute position
        if(offsetPositionSpace == Space.Self)
        {
            transform.position = target.TransformPoint(offset);
        }
        else
        {
            transform.position = target.position + offset;
        }

        // compute rotation
        if(lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            var y = target.transform.eulerAngles.y;
            var z = target.transform.eulerAngles.z;
            transform.eulerAngles = new Vector3(15f, y-18.1f, z);
        }
    } */