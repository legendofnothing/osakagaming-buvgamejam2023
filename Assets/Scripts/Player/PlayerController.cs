using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
    public class PlayerMovement : Core.Singleton<PlayerMovement>
    {
        public float moveSpeed;
        public float maxSpeed;
        [ReadOnly] public float currentSpeed;

        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _moveDrag;
        [SerializeField] private float _stopDrag;

        [SerializeField] private GameObject _playerArm;
        [SerializeField] private GameObject _playerBody;

        private Vector2 _moveInput;
        private Rigidbody2D rb;
        private SpriteRenderer _BodySpriteRenderer;
        private Animator _animator;
        
        private void Awake()
        {
            //manaManagerCS = FindObjectOfType<ManaManager>();
            rb = GetComponent<Rigidbody2D>();
            _BodySpriteRenderer = _playerBody.GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            currentSpeed = moveSpeed;
        }

        private void Update() {
            _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            
            if (rb.velocity.magnitude > maxSpeed) {
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            }

            Flip();
            TurnPlayerArm();
        }

        private void FixedUpdate()
        {
            //Change the drag based on moving key is press or not//
            if (Mathf.Abs(_moveInput.x) == 0 && Mathf.Abs(_moveInput.y) == 0)
            {
                rb.drag = _stopDrag;
                _animator.SetBool("IsMoving", false);
            }
            //If key is press
            if (Mathf.Abs(_moveInput.x) > 0 || Mathf.Abs(_moveInput.y) > 0)
            {
                Move();
                rb.drag = _moveDrag;
                _animator.SetBool("IsMoving",true);
            }
        }

        //Flip player based on mouse position
        private void Flip()
        {        
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse postion
            float faceIndex = Mathf.Sign(mousePosition.x - transform.position.x);
            //_playerBody.transform.localScale = faceIndex <= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1 ,1);
            _BodySpriteRenderer.flipX = faceIndex <= 0 ? false : true;
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

        private void Move() {
            rb.AddForce(_moveInput * (currentSpeed * Time.fixedDeltaTime), ForceMode2D.Force);
        }
    }
}
