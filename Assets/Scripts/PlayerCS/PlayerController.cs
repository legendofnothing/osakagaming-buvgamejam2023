using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxMagnitude;
    //[SerializeField] private float slowedMoveSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveDrag;
    [SerializeField] private float _stopDrag;

    [SerializeField] private GameObject _playerArm;
    [SerializeField] private GameObject _playerBody;
    [SerializeField] private GameObject _playerWeapon;

    private Vector2 _moveInput;
    private Rigidbody2D rb;

    private float _moveSpeedCount;

    //Action trigger bool
    private bool dash;

    //ManaManager manaManagerCS;

    private void Awake()
    {
        //manaManagerCS = FindObjectOfType<ManaManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        _moveSpeedCount = _moveSpeed;
    }
    
    private void Update() {
        if(rb.velocity.magnitude > _maxMagnitude) {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, _maxMagnitude);
        }

        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Flip();
        TurnPlayerArm();
    }

    private void FixedUpdate()
    {
        //Change the drag based on moving key is press or not//
        if (Mathf.Abs(_moveInput.x) == 0 && Mathf.Abs(_moveInput.y) == 0)
        {
            rb.drag = _stopDrag;
        }
        //If key is press
        if (Mathf.Abs(_moveInput.x) > 0 || Mathf.Abs(_moveInput.y) > 0)
        {
            Move();
            rb.drag = _moveDrag;
        }
    }

    //Flip player based on mouse position
    private void Flip()
    {        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse postion

        float faceIndex = Mathf.Sign(mousePosition.x - transform.position.x);
        
        if (faceIndex <= 0)
        {
            _playerBody.transform.localScale = new Vector3(1, 1, 1);
            _playerWeapon.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _playerBody.transform.localScale = new Vector3(-1, 1 ,1);
            _playerWeapon.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    private void TurnPlayerArm()
    {
        float playerArmRotation = _playerArm.transform.localRotation.eulerAngles.z;
        Quaternion playerArmRotationVar = Quaternion.Euler(0, 0, playerArmRotation);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = mousePosition - new Vector2(_playerArm.transform.position.x, _playerArm.transform.position.y);
        float mouseEulerRotationVar = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 180;

        Quaternion MouseRotationVar = Quaternion.Euler(0, 0, mouseEulerRotationVar);

        _playerArm.transform.rotation = Quaternion.Slerp(playerArmRotationVar, MouseRotationVar, Time.deltaTime * _turnSpeed);
    }

    private void Move()
    {
        rb.AddForce(_moveSpeedCount * Time.fixedDeltaTime * _moveInput);
    }

}
