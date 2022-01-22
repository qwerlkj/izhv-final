using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMesh : MonoBehaviour
{
    public GameObject body;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("ARROW");
        }
    }
}
