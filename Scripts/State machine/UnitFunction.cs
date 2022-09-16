using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class UnitFunction
{
    [VerticalGroup("V"), HideLabel, EnumPaging] public FunctionType functionType;

    private bool ShowInt => functionType is FunctionType.Log;
    private bool ShowFloat => functionType is FunctionType.Log or FunctionType.Move_L or FunctionType.Move_R or FunctionType.Move_L_R;
    private bool ShowString => functionType is FunctionType.Log or FunctionType.Change_State;

    [VerticalGroup("V"), HideLabel, ShowIf("ShowInt")] public int @int;
    [VerticalGroup("V"), HideLabel, ShowIf("ShowFloat")] public float @float;
    [VerticalGroup("V"), HideLabel, ShowIf("ShowString")] public string @string;

    private InputManager _inputManager;
    private InputManager InputManager
    {
        get
        {
            if (_inputManager == null)
                _inputManager = AnimationValues.inputManager;
            return _inputManager;
        }
    }
    
    private PlayerAnimationValues _animationValues;
    private PlayerAnimationValues AnimationValues
    {
        get
        {
            if (_animationValues == null)
                _animationValues = GameManager.Instance.player.GetComponent<PlayerAnimationValues>();
            return _animationValues;
        }
    }

    private Transform _transform;
    private Transform Transform
    {
        get
        {
            if (_transform == null) _transform = AnimationValues.gameObject.transform;
            return _transform;
        }
    }


    public void Function()
    {
        switch (functionType)
        {
            case FunctionType.Log:
                Log();
                break;
            case FunctionType.Change_State:
                ChangeState();
                break;
            case FunctionType.Place_On_Ground:
                PlaceOnGround();
                break;
            case FunctionType.Move_L:
                MoveL();
                break;
            case FunctionType.Move_R:
                MoveR();
                break;
            case FunctionType.Move_L_R:
                MoveLR();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // ____________   F U N C T I O N S   _____________ \\

    private void Log()
    {
        Debug.Log(@string + " " + @int + " " + @float);
    }

    private void ChangeState()
    {
        AnimationValues.animator.Play(@string);
    }


    private void PlaceOnGround()
    {
        RaycastHit hit;
        Vector3 ori = Transform.position + Vector3.up * 0.1f;
        Physics.Raycast(ori, Vector3.down, out hit, 10f, AnimationValues.layerMask);
        if (hit.collider == null) return;
        if (hit.transform.CompareTag("Ground"))
        {
            Transform.position = hit.point;
        }
    }

    private void MoveL()
    {
        if(InputManager.Tilt.x > 0) return;
        MoveLR();
    }
    
    private void MoveR()
    {
        if(InputManager.Tilt.x < 0) return;
        MoveLR();
    }

    private void MoveLR()
    {
        Vector3 position = Transform.position;
        position.x += @float * InputManager.Tilt.x * Time.deltaTime;
        Transform.position = position;
    }
}

public enum FunctionType
{
    Change_State,
    Place_On_Ground,
    Move_L,
    Move_R,
    Move_L_R,
    Log,
}