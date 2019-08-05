using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 1.6f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -3.5f;
        public float zoomSmooth = 100;
        public float maxZoom = -2;
        public float minZoom = -15;
        public bool smoothFollow = true;
        public float smoothTime = 0.05f;

        [HideInInspector]
        public float newDistance = -8;
        [HideInInspector]
        public float adjustmentDistance = -8;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -15;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -40;
        public float maxYRotation = 230;
        public float minYRotation = 130;
        public float vOrbitSmooth = 150; 
        public float hOrbitSmooth = 150; 
    }

    [System.Serializable]
    public class InputSettings
    {
        public string MOUSE_ORBIT = "MouseOrbit";
        public string MOUSE_ORBIT_VERTICAL = "MouseOrbitVertical";
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCOllisionLines = true;
    }
    
    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CameraCollisionController.CollisionHandler collision;

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camVel = Vector3.zero;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;

    PlayerController _playerController;

    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput, vMouseOrbitInput;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        SetCameraTarget(target);
        vOrbitInput = hOrbitInput = zoomInput = hOrbitSnapInput = mouseOrbitInput = vMouseOrbitInput = 0;
        SetCollisionHandler();
        MoveToTarget();
        collision.Initialize(transform.GetComponent<Camera>());
        UpdateCameraClipPoints();
        previousMousePos = currentMousePos = Input.mousePosition;
    }

    void SetCollisionHandler()
    {
        if (transform.GetComponent<CameraCollisionController>())
        {
            var cameraCollisionScript = transform.GetComponent<CameraCollisionController>();
            collision = cameraCollisionScript.collision;
            cameraCollisionScript.target = target;
        }
        else
        {
            Debug.LogError("No camera collision handler attached to camera");
        }
    }

    void UpdateCameraClipPoints()
    {
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

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
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Moving
        MoveToTarget();
        // Rotating
        LookAtTarget();    
        // Orbiting (via input)
        OrbitTarget();
        MouseOrbitTarget();

        UpdateCameraClipPoints();

        DrawDebugLines();

        CheckIfColliding();
        UpdateAdjustmentDistance();
    }

    // using raycasts
    void CheckIfColliding ()
    {
        collision.CheckColliding(targetPos);
    }
    
    // whenever the camera collides, it'll start to use the adjustmentDistance
    void UpdateAdjustmentDistance()
    {
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);

    }
    
    // draw debug lines to clip points
    void DrawDebugLines()
    {
        for (int i = 0; i < 5; ++i)
        {
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }
            else
            {
                // nothing to do here
            }

            if (debug.drawAdjustedCOllisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
            else
            {
                // nothing to do here
            }

        }
    }

    void MouseOrbitTarget()
    {
        if (mouseOrbitInput != 0)
        {
            //getting the camera to orbit around our character
            orbit.xRotation += vMouseOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
            orbit.yRotation += mouseOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;
        }
    }
    
    void MoveToTarget()
    {
//        targetPos = target.position + position.targetPosOffset;
        targetPos = target.position + Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x);
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget
        ;
        destination += targetPos;
        transform.position = destination;

        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            MoveTorwardTo(adjustedDestination);
        }
        else
        {
            MoveTorwardTo(destination);
        }
    }

    void MoveTorwardTo(Vector3 dest)
    {
        if (position.smoothFollow)
        {
            // use smooth damp function
            transform.position = Vector3.SmoothDamp(transform.position, dest, ref camVel, position.smoothTime);
        }
        else
        {
            transform.position = dest;
        }            
    }

    void Update() {
        GetInput();
        ZoomInOnTarget();    
    }

    void GetInput()
    {
        vOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
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
        if (orbit.yRotation > orbit.maxYRotation)
        {
            orbit.yRotation = orbit.maxYRotation;
        }
        if (orbit.yRotation < orbit.minYRotation)
        {
            orbit.yRotation = orbit.minYRotation;
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