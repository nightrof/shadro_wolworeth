using System.Collections;
using UnityEngine;

/* 
    full reference -> 1) https://www.youtube.com/watch?v=MkbovxhwM4I&list=PL4CCSwmU04MjDqjY_gdroxHe85Ex5Q6Dy&index=4
                      2) https://www.youtube.com/watch?v=Uqi2jEgvVsI&list=PL4CCSwmU04MjDqjY_gdroxHe85Ex5Q6Dy&index=5
 */
public class CameraCollisionController : MonoBehaviour 
{
    [HideInInspector]
    public Transform target;

    [System.Serializable]
    public class CollisionHandler 
    {
        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        public LayerMask collisionLayer;
        Camera cam;

        public void Initialize (Camera c) 
        {
            cam = c;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints (Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray) 
        {
            if (!cam) 
            {
                return;
            }
            else 
            {
                intoArray = new Vector3[5];

                float z = setZ ();
                float x = setX (z);
                float y = setY (x);

                //top left
                intoArray[0] = setClip (-x, y, z, cameraPosition, atRotation);
                //top right
                intoArray[1] = setClip (x, y, z, cameraPosition, atRotation);
                //bottom left
                intoArray[2] = setClip (-x, -y, z, cameraPosition, atRotation);
                //bottom right
                intoArray[3] = setClip (x, -y, z, cameraPosition, atRotation);
                //camera's position
                intoArray[4] = cameraPosition - cam.transform.forward;
            }
        }
        // distance from camera to nearest clipPlane
        private float setZ () 
        {
            return cam.nearClipPlane;
        }
        // width, horizontal distance to clip
        private float setX (float z) 
        {
            return Mathf.Tan (cam.fieldOfView / 3.41f) * z;
        }
        // height, vertical distance to clip
        private float setY (float x) 
        {
            return x / cam.aspect;
        }
        // added and rotated the point relative to camera
        private Vector3 setClip (float x, float y, float z, Vector3 cameraPosition, Quaternion atRotation) 
        {
            return (atRotation * new Vector3 (x, y, z)) + cameraPosition;
        }

        // cast a ray from targetposition to the direction, for the length of the distance 
        bool CollisionDetectedAtClipPoints (Vector3[] clipPoints, Vector3 targetPosition) 
        {
            for (int i = 0; i < clipPoints.Length; ++i) 
            {
                Vector3 direction = clipPoints[i] - targetPosition;
                Ray ray = new Ray (targetPosition, direction);
                float distance = Vector3.Distance (clipPoints[i], targetPosition);
                if (Physics.Raycast (ray, distance, collisionLayer)) 
                {
                    return true;
                }
                else 
                {
                    // nothing to do
                }
            }
            return false;
        }
        // only used if collision has been detected
        // check the distance the camera has to be from the target
        public float GetAdjustedDistanceWithRayFrom (Vector3 target) 
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; ++i) 
            {
                // find shortest distance between any of the clip colliding

                // direction from target position torwards camera clip point
                Vector3 direction = desiredCameraClipPoints[i] - target;
                Ray ray = new Ray (target, direction);
                RaycastHit hit;
                if (Physics.Raycast (ray, out hit) && !hit.collider.CompareTag ("Player")) 
                {
                    Debug.Log (hit.collider.tag);
                    if (distance == -1) 
                    {
                        distance = hit.distance;
                    }
                    else 
                    {
                        if (hit.distance < distance) 
                        {
                            distance = hit.distance;
                        }
                        else 
                        {
                            // nothing to do
                        }
                    }
                }
                else 
                {

                }
            }
            if (distance == -1) 
            {
                return 0;
            }
            else 
            {
                return distance;
            }
        }

        public void CheckColliding (Vector3 targetPosition) 
        {
            if (CollisionDetectedAtClipPoints (desiredCameraClipPoints, targetPosition)) 
            {
                colliding = true;
            }
            else 
            {
                colliding = false;
            }
        }

    }

    public CollisionHandler collision = new CollisionHandler ();
}