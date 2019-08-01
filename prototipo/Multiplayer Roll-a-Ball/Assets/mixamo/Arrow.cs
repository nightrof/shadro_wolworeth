using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    float lifeTimer = 2f;
    float timer;
    bool hitSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Rotate();
        Destroy(gameObject, lifeTimer);
    }

    void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);    
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(!hitSomething)
        {
            Rotate();
        }
        else
        {
            if(timer >= lifeTimer) {
                Destroy(gameObject);
            }
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Arrow"))
        {
            hitSomething = true;
            Stick();
        }
        
    }

    void Stick()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
