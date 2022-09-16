using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    private Input _input;

    private String L_R;
    private String T_B;

    private InputEnum _inputEnum;

    private bool _initialTiltSet = false;
    private Vector2 _initialTilt;
    private Vector2 _tiltRaw; //  x: Sideways,   y: forward
    [SerializeField, HorizontalGroup("H"), LabelWidth(100)] private bool logTilt;
    [SerializeField, HorizontalGroup("H"), LabelWidth(100)] private bool logTouch;
    [SerializeField, HorizontalGroup("H"), LabelWidth(100)] private bool logSwipe;
    [SerializeField] private Vector2 tiltRange;
    [SerializeField] private Vector2 tiltNullRange;
    [SerializeField] private float swipeAngle;
    public Vector2 Tilt { get; private set; }
    public bool Pressed { get; private set; }
    public String InputDirection { get; private set; }
    public InputEnum InputEnum => Pressed ? _inputEnum : InputEnum.O;

    private void Update()
    {
        if (logSwipe) Debug.Log(_inputEnum);
    }

    private void Awake()
    {
        _input = new Input();
        _input.Enable();
        _input.Movement.Swipe.performed += Swipe_preformed;

        _input.Movement.Tilt.performed += Tilt_performed;

        _input.Movement.Press.performed += Press_performed;

        InputDirection = "O";
        TouchSimulation.Enable();
    }

    private void Swipe_preformed(InputAction.CallbackContext ctx)
    {
        // O : Not Pressed ||   L : Left ||   R : Right   
        // T : Top         ||  TL        ||  TR           
        // B : Bottom      ||  BL        ||  BR           

        Vector2 inputDirection = ctx.ReadValue<Vector2>();

        L_R = inputDirection.x > 0 ? "R" : "L";
        T_B = inputDirection.y > 0 ? "T" : "B";
        if (Mathf.Abs(inputDirection.x) < swipeAngle) L_R = "";
        if (Mathf.Abs(inputDirection.y) < swipeAngle) T_B = "";

        string s = T_B + L_R;
        InputDirection = (s == "") ? InputDirection : s;
        if (!Pressed) InputDirection = "O";
        
        // set SwipeInput
        switch (InputDirection)
        {
            case "O":
                _inputEnum = InputEnum.O;
                return;
            case "T":
                _inputEnum = InputEnum.T;
                return;
            case "B":
                _inputEnum = InputEnum.B;
                return;
            case "L":
                _inputEnum = InputEnum.L;
                return;
            case "R":
                _inputEnum = InputEnum.R;
                return;
            case "TL":
                _inputEnum = InputEnum.TL;
                return;
            case "TR":
                _inputEnum = InputEnum.TR;
                return;
            case "BL":
                _inputEnum = InputEnum.BL;
                return;
            case "BR":
                _inputEnum = InputEnum.BR;
                return;
        }
    }

    private void Tilt_performed(InputAction.CallbackContext ctx)
    {
        Vector3 val = ctx.ReadValue<Vector3>();
        _tiltRaw.x = val.x;
        _tiltRaw.y = val.z;
        if (!_initialTiltSet)
        {
            _initialTiltSet = true;
            _initialTilt = _tiltRaw;
            if (logTilt) Debug.Log("Initial set");
        }

        Vector2 Z;
        Z = _tiltRaw - _initialTilt;
        Z.x = Mathf.Clamp(Z.x, -tiltRange.x, tiltRange.x);
        Z.y = Mathf.Clamp(Z.y, -tiltRange.y, tiltRange.y);
        if (Mathf.Abs(Z.x) < tiltNullRange.x) Z.x = 0; 
        if (Mathf.Abs(Z.y) < tiltNullRange.y) Z.y = 0; 
        Tilt = Z;
        if (logTilt) Debug.Log(Tilt);
    }

    private void Press_performed(InputAction.CallbackContext ctx)
    {
        float i = ctx.ReadValue<float>();
        Pressed = i == 1;
        if (!Pressed) InputDirection = "O";
        if (logTouch) Debug.Log(Pressed);
    }

    #region ENABLE_DISABLE

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input.Disable();
    }

    #endregion
}

public enum InputEnum
{
    O,T,B,L,R,TL,TR,BL,BR,
}