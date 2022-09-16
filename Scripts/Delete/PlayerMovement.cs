using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovement : MonoBehaviour
{
    public InputManager inputManager;
    public LayerMask layerMask;
    
    [TabGroup("Run")]
    public float speed, lrSpeed;

    [TabGroup("Jump")] public float jumpTime;
    [TabGroup("Jump")]
    public AnimationCurve jumpCurve;

    [TabGroup("Slide")] public float slideTime;
    [TabGroup("Slide")]
    public AnimationCurve sideSlideCurve;

    [HideInInspector] public bool backToRunning = false;
    private Vector2 InputTilt => inputManager.Tilt;
    private String Swipe => inputManager.InputDirection;
    private bool Pressed => inputManager.Pressed;
    
    private Rigidbody _rigidbody;
    private Animator _animator;
    private PlayerAnimationValues _playerAnimationValues;
    private Vector3 _position;
    private float _flag;


    private float _currentStateTime;
    private float _startingHeight;
    private float _startingPosition;
    private int _direction = 1;
    
    
    private delegate void Tick();
    private Tick _tick;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerAnimationValues = GetComponent<PlayerAnimationValues>();
        _tick = Running;
    }


    private void Update()
    {
        _tick();
    }

    private void RunSetup()
    {
        //_animator.SetInteger(_playerAnimationValues.movementIndex, -1);
        backToRunning = false;
        _tick = Running;
    }
    private void Running()
    {
        if(!PositionOnGround()) return;
        
        Debug.Log("Running");
        
        if(Swipe == "T"){JumpSetup();return;}
        if(Swipe == "L" || Swipe == "R"){SlidSetup();return;}

        Vector3 v = new Vector3(0, 0, 0);
        v.z += speed;
        v.x += lrSpeed * InputTilt.x;

        _rigidbody.velocity = v;
    }

    private void JumpSetup()
    {
        //_animator.SetInteger(_playerAnimationValues.movementIndex, 0);
        _currentStateTime = 0;
        _startingHeight = _rigidbody.position.y;
        _flag = 2;
        _tick = Jumping;
    }
    private void Jumping()
    {
        Debug.Log("Jumping");
        if(backToRunning) {RunSetup(); return;}
        
        float y = jumpCurve.Evaluate(_currentStateTime);
        _position = _rigidbody.position;
        _position.y = _startingHeight + y;
        _rigidbody.position = _position;
        
        _currentStateTime += Time.deltaTime;
    }

    private void SlidSetup()
    {
        //_animator.SetInteger(_playerAnimationValues.movementIndex, 1);
        _startingPosition = _rigidbody.position.z;
        _currentStateTime = 0;
        _direction = Swipe == "R" ? 1 : -1;
        _tick = Slide;
    }
    private void Slide()
    {
        Debug.Log("Sliding");
        if(backToRunning) {RunSetup(); return;}
        float x = sideSlideCurve.Evaluate(_currentStateTime);
        x *= _direction;
        _position = _rigidbody.position;
        _position.x = _startingPosition + x;
        _rigidbody.position = _position;

        _currentStateTime += Time.deltaTime;
    }

    private bool PositionOnGround()
    {
        RaycastHit hit;
        Vector3 ori = transform.position + Vector3.up * 0.1f;
        Physics.Raycast(ori, Vector3.down, out hit, 10f,layerMask);
        if (hit.collider == null) return false;
        if (hit.transform.CompareTag("Ground"))
        {
            transform.position = hit.point;
            Debug.Log("On ground");
            return true;
        }
        return false;
    }
}