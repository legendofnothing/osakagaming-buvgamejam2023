using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMagnitude;
    //[SerializeField] private float slowedMoveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float moveDrag;
    [SerializeField] private float stopDrag;

    //[SerializeField] private GameObject lazerSightObj;

    private Vector2 moveInput;
    private Rigidbody2D rb;

    private float moveSpeedCount;

    //Action trigger bool
    private bool dash;

    //ManaManager manaManagerCS;

    private void Awake()
    {
        //manaManagerCS = FindObjectOfType<ManaManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //lazerSightObj.SetActive(false);
        moveSpeedCount = moveSpeed;
    }


    void Update()
    {
        if(rb.velocity.magnitude > maxMagnitude)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxMagnitude);
        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //ChangeMovespeedOnMouseInput();


    }

    //private void ChangeMovespeedOnMouseInput()
    //{
    //    if (isDashing) { return; }

    //    //Slow down when pressing right mouse
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        moveSpeedCount = slowedMoveSpeed;
    //    }
    //    else if (Input.GetMouseButtonUp(1))
    //    {
    //        moveSpeedCount = moveSpeed;
    //    }
    //}

    private void FixedUpdate()
    {
        Turn();

        //Change the drag based on moving key is press or not//
        if (Mathf.Abs(moveInput.x) == 0 && Mathf.Abs(moveInput.y) == 0)
        {
            rb.drag = stopDrag;
        }
        //If key is press
        if (Mathf.Abs(moveInput.x) > 0 || Mathf.Abs(moveInput.y) > 0)
        {
            Move();
            rb.drag = moveDrag;
        }
    }

    //Turn player base on mouse position
    void Turn()
    {
        float playerRotation = gameObject.transform.localRotation.eulerAngles.z;
        Quaternion playerRotationVar = Quaternion.Euler(0, 0, playerRotation);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse postion
        Vector2 mouseDirection = mousePosition - new Vector2(transform.position.x, transform.position.y); //Get mouse direction (direction = destination - source)
        float mouseEulerRotationVar = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90; // reverse cotan to get angle

        Quaternion MouseRotationVar = Quaternion.Euler(0, 0, mouseEulerRotationVar);

        this.transform.rotation = Quaternion.Slerp(playerRotationVar, MouseRotationVar, Time.deltaTime * turnSpeed);

    }

    private void Move()
    {
        
        rb.AddForce(moveSpeedCount * Time.deltaTime * moveInput);
    }

}
