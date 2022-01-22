using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform head;
    public GameObject positionObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = positionObject.transform.position;
        transform.rotation = Quaternion.LookRotation(head.forward);
    }
}
