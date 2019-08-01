using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    float life = 100;
    Slider healthBar;
    int selfID;

    void Start()
    {
        healthBar = Camera.allCameras[0].GetComponentInChildren(typeof(Slider)) as Slider;
        healthBar.maxValue = 100;
        healthBar.value = 100;
        selfID = gameObject.GetInstanceID();
    }

    void OnCollisionEnter(Collision collision)
    {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (collision.gameObject.GetComponent<Arrow>().parentID != selfID) {
                life -= 25;
                healthBar.value = life;
                string debug = gameObject.name + "=" + life + "; lost " + 25 + "hitpoints.";
                Debug.Log(debug);
                if (life <= 0) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
