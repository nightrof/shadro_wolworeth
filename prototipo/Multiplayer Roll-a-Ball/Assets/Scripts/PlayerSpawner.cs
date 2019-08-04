using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    /*
        faz o personagem nascer em algum aleatório válido no mapa
    */
    void RandomSpawnPosition () {
        var x = Random.Range (3, -40);
        var y = 15;
        var z = Random.Range (70, 40);
        transform.position = new Vector3 (x, y, z);
        transform.rotation = Quaternion.identity;
    }

    void Awake () {
        Debug.Log(transform.position);
        RandomSpawnPosition ();
        Debug.Log(transform.position);
    }

}