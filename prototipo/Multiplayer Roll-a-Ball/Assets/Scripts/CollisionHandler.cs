using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public float life = 100;

    void OnCollisionEnter(Collision collision)
    {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.CompareTag("Player"))
        {
            var object_speed = collision.gameObject.GetComponent<MoveBall>();
            var speed = object_speed.current_speed;
            life -= speed;
            string debug = gameObject.name + "=" + life + "; lost " + speed + "hitpoints.";
            Debug.Log(debug);
            if (life <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
