using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

/*     public Transform target;

    private Vector3 offset;

    
    void LateUpdate ()
    {
        transform.position = target.transform.position + offset;
    } */
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
    }
}