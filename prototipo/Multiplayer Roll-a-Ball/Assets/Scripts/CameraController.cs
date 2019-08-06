using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [HideInInspector]
    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0.1f, 0f, 0);//new Vector3(0.4f, 1.45f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -2f;
        public float zoomSmooth = 100;
        public float maxZoom = -1;
        public float minZoom = -4;
        public bool smoothFollow = false;
        public float smoothTime = 0.05f;

        [HideInInspector]
        public float newDistance = -2f;
        [HideInInspector]
        public float adjustmentDistance = -2f;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -5;
        public float yRotation = 170;
        public float maxXRotation = 25;
        public float minXRotation = -80;
        public float maxYRotation = 230;
        public float minYRotation = 130;
        public float vOrbitSmooth = 150; 
        public float hOrbitSmooth = 150; 
        public float mouseSensitivity = 3;
    }

    [System.Serializable]
    public class InputSettings
    {
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

    float vOrbitInput, hOrbitInput, zoomInput, mouseX, mouseY;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        vOrbitInput = hOrbitInput = zoomInput = mouseX = mouseY =  0;
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

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Moving
        MoveToTarget();
        // Rotating
        LookAtTarget();    

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
    
    void MoveToTarget()
    {
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

    void LateUpdate() {
        GetInput();
        ZoomInOnTarget();
        OrbitTarget();  
    }
    
    void GetInput()
    {
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }

    void OrbitTarget()
    {

/*         orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }
        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        } */

        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;
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
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
