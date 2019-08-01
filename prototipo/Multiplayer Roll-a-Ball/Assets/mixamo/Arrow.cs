using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    float lifeTimer = 2f;
    float timer;
    bool hitSomething = false;
    public int parentID;
    // Start is called before the first frame update
    void Start()
    {
        parentID = transform.parent.gameObject.GetInstanceID();
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
        
        int colID = col.gameObject.GetInstanceID();
        Debug.Log("o pai {" + parentID + "} o colisor {"+ colID + "}");
        if(!col.gameObject.CompareTag("Arrow") && colID != parentID)
        {
            hitSomething = true;
            transform.parent = null;
            Stick();
        }
        
    }

    void Stick()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
