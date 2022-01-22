using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemController : MonoBehaviour
{
    public GameObject target;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    private NavMeshAgent agent;
    private Animator animator;
    private float speed = 0f;
    void Start()
    {
        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        elapsed = 0.0f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        
        var distance = Vector3.Distance(transform.position, target.transform.position);
        speed = Vector3.Distance(agent.velocity, new Vector3(0, 0, 0));
        animator.SetFloat("Speed", speed);
        if(distance > 1f && speed > 0.1f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
        if (elapsed > 2.0f)
        {
            elapsed -= 2.0f;
            NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
            agent.SetPath(path);
            
            if (distance < 0.8f)
            {
                hit();
            }
        }

        
    }

    void hit()
    {
        var stats = target.GetComponent<TargetStats>();
        if (stats.health <= 0) return;
        stats.health -= 1;
        animator.SetBool("Hit", true);
    }
}
