using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetStats : MonoBehaviour
{
    public int health = 10;
    public TextMeshProUGUI healthDisplay;
    private void Update()
    {
        if (healthDisplay != null)
        {
            healthDisplay.SetText("{0} HP", health);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.GetComponent<EnemyMesh>().body);
            if(health > 0)
                health -= 1;
        }
    }
}
