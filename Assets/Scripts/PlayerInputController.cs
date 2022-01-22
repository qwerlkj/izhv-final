using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    private Rigidbody rb;
    public float mouseSensitivity = 100f, speed = 5f;
    public Transform head;
    private float rotation = 0f;
    private Vector2 movement;
    private bool moving = false;
    public GameObject arrow; 
    public GameObject arrowStartPosition;
    public Transform LUpperArm;
    public GameObject arrowInfo;
    public Slider powerBar;

    public int maxArrows = 10;
    public int remainingArrows;
    public float reloadTime = 5f, maxPower = 100f;
    private bool reloading = false, loaded = false, dragging = false;
    private InputAction.CallbackContext reloadingContext;
    private double startReloading = 100f;
    private float arrowPower = 0f;
    public Slider reloadBar;
    private Image reloadBarImage;
    private float myTime = 0f;
    private bool canJump = true;
    private GameObject tempArrow;
    private GameObject[] arrows;
    public float distanceToCollectArrow = 1f;

    private void Awake()
    {
        arrows = GameObject.FindGameObjectsWithTag("Arrow");
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        powerBar.value = 0f;

        if (reloadBar != null)
        {
            reloadBar.maxValue = reloadTime;
            reloadBarImage = reloadBar.fillRect.GetComponent<Image>();
        }
    }

    private void Update()
    {
        arrowInfo.GetComponent<TextMeshProUGUI>().SetText("{0}/{1}", remainingArrows, maxArrows);
        if (reloading)
        {
            if (reloadBar != null)
            {
                myTime += Time.deltaTime;
                if (myTime >= reloadTime)
                {
                    myTime = reloadTime;
                }

                if(reloadBarImage != null) reloadBarImage.color = Color.Lerp(Color.red, Color.green, myTime / reloadTime);
                reloadBar.value = myTime;

            }
        }

        
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            rb.velocity = transform.TransformDirection(new Vector3(movement.x*speed, rb.velocity.y, movement.y*speed));
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 mouse = context.ReadValue<Vector2>();
        float mouseX = mouse.x * mouseSensitivity * 0.002f;
        float mouseY = mouse.y * mouseSensitivity * 0.002f;
        if (dragging)
        {
            Debug.Log("DRAGG");
            arrowPower -= mouseY * 0.1f;
            arrowPower = Math.Clamp(arrowPower, 0f, maxPower);
            powerBar.value = arrowPower / 100f;
            Debug.Log(arrowPower);
            return;
        };
        
        if (head != null)
        {
            rotation -= mouseY;
            rotation = Mathf.Clamp(rotation, -90f, 90f);
            head.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            LUpperArm.localRotation = Quaternion.Euler(rotation, 0, 0);
        }
        rb.transform.Rotate(0, mouseX, 0);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            movement = context.ReadValue<Vector2>();
            moving = true;
        }

        if (context.canceled)
        {
            moving = false;
        }
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up*5f, ForceMode.Impulse);
        }

    }

    public void reload(InputAction.CallbackContext context)
    {
        
        if (!loaded)
        {
            
            if (context.started)
            {
                if (remainingArrows <= 0) return;
                tempArrow = Instantiate(arrow, arrowStartPosition.transform.position,
                    Quaternion.LookRotation(head.forward));
                tempArrow.GetComponent<ArrowShoot>().positionObject = arrowStartPosition;
                tempArrow.GetComponent<ArrowShoot>().head = head;
                tempArrow.GetComponent<ArrowShoot>().playerCollider = GetComponent<Collider>();
                var i = 0;
                while (arrows[i] != null) ++i;
                arrows[i] = tempArrow;
                
                remainingArrows -= 1;
                reloadingContext = context;
                reloading = true;
                myTime = 0f;
                startReloading = context.startTime;
            }
            if (context.canceled)
            {
                reloading = false;
                if (context.time - startReloading < reloadTime)
                {
                    dropArrow();
                    startReloading = 100f;
                }
                else
                {
                    loaded = true;
                }
            }
        }
        
    }
    public void powerUp(InputAction.CallbackContext context)
    {
        if (loaded && context.started)
        {
            Debug.Log("STARTED dragging");
            dragging = true;
        }

        if (loaded && context.canceled && dragging)
        {
            powerBar.value = 0f;
            dragging = false;
            if (arrowPower < 0.01f) return;
            resetReloadBar();
            Debug.Log("STOPPED dragging");
            loaded = false;
            shoot();
        }
    }
    private void shoot()
    {
        //Transform arrow with power to destination
        Debug.Log("SHOOT");
        if (tempArrow == null) return;
        var passVars = tempArrow.GetComponent<ArrowShoot>();
        passVars.shoot = true;
        passVars.power = arrowPower;
        
        arrowPower = 0f;
        tempArrow = null;
    }

    private void dropArrow()
    {
        if (tempArrow == null) return;
        tempArrow.GetComponent<ArrowShoot>().drop = true;
        tempArrow = null;
    }

    public void collectArrow(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(remainingArrows >= maxArrows) return;
            for (int i = 0; i < arrows.Length; i++)
            {
                if (arrows[i] != null)
                {
                    var distanceToArrow = Vector3.Distance(arrows[i].transform.position, transform.position);
                    if (distanceToArrow < distanceToCollectArrow)
                    {
                        Destroy(arrows[i]);
                        remainingArrows += 1;
                        arrows[i] = null;
                        break;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            canJump = true;
        }
    }

    private void resetReloadBar()
    {
        if (reloadBar != null) reloadBar.value = 0f;
        if(reloadBarImage != null) reloadBarImage.color = Color.red;
    }
}
