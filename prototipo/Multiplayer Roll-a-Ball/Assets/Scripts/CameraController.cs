using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;

    public void Start ()
    {
        Debug.Log("entrou <<<<<<");
        offset = new Vector3(-2, 35, -20);
    }
    
    void LateUpdate ()
    {
        transform.position = player.transform.position + offset;
    }
}