using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float current_speed;
    private Rigidbody rb;
    private Vector3 lastPosition;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = Vector3.zero;
        current_speed = 0;
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        /* float jump = Input.GetKey("up"); */
        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rb.AddForce (movement*speed);
        current_speed = (((transform.position - lastPosition).magnitude) / Time.deltaTime) ;
        lastPosition = transform.position;
    }

}