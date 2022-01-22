using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float power = 0f;
    public bool shoot = false;
    public bool drop = false;
    public GameObject positionObject;
    public Transform head;
    public Collider playerCollider;
    public Collider arrowCollider;
    private bool stay = false;
    private bool onBow = true;
    
    private Rigidbody rb;

    private void Start()
    {
        if(playerCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, arrowCollider);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    
    void FixedUpdate()
    {
        handleShoot();
    }

    private void handleShoot()
    {
        if (!drop && !shoot && !stay)
        {
            if (positionObject == null) return;
            transform.position = positionObject.transform.position;
            transform.rotation = Quaternion.LookRotation(head.forward);
            onBow = true;
        }
        if (shoot)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(head.forward*power*50f, ForceMode.Force);
            shoot = false;
            drop = true;
            onBow = false;
        }

        if (drop && !stay)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            drop = false;
            rb.isKinematic = true;
            stay = true;
            rb.useGravity = false;
            transform.position = transform.position + new Vector3(0f, 0.3f, 0f);
            Debug.Log("Ground");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Arrow Hit ENEMy!!!");
            if(!onBow)
                Destroy(collision.gameObject.GetComponent<EnemyMesh>().body);
        }
        
        
    } 
}
