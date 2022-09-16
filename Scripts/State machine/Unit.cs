using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Unit
{

    [EnumPaging, HideLabel] public UnitType unitType;

    bool showInput
    {
        get
        {
            return unitType is UnitType.Function_For_Input
                or UnitType.Function_For_Condition_And_Input;
        }
    }

    bool showInputS
    {
        get
        {
            return unitType is UnitType.Function_For_Input_S
                or UnitType.Function_For_Condition_And_Input_S;
        }
    }
    
    private List<String> st;
    private List<String> ConditionTypes => AnimationValues.condition.ConditionTypesString;
    bool showConditions
    {
        get
        {
            return unitType is UnitType.Function_For_Condition or UnitType.Function_For_Condition_And_Input
                or UnitType.Function_For_Condition_And_Input_S;
        }
    }

    bool showFunctionTypes
    {
        get
        {
            return unitType is UnitType.Function or UnitType.Function_For_Condition or UnitType.Function_For_Input
                or UnitType.Function_For_Input_S
                or UnitType.Function_For_Condition_And_Input or UnitType.Function_For_Condition_And_Input_S;
        }
    }

    [HorizontalGroup("H"), HideLabel, LabelWidth(1), ShowIf("showInput")]
    public InputEnum inputEnum;

    [HorizontalGroup("H"), LabelWidth(1), ShowIf("showInputS")]
    public List<InputEnum> inputs;

    //[HorizontalGroup("H"), ShowIf("showConditions")]
    [HideInInspector]
    public List<UnitCondition> conditions;
    
    [HorizontalGroup("H", 100), ShowIf("showConditions"), HideLabel]
    [ValueDropdown("ConditionTypes")] public String type;

    [HorizontalGroup("H"), ShowIf("showFunctionTypes")]
    public List<UnitFunction> functions;


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


    private bool CorrectInput => InputManager.InputEnum == inputEnum;
    private bool CorrectInputS => inputs.Any(i => i == InputManager.InputEnum);

    private bool CorrectConditions =>
        conditions.TrueForAll(c => AnimationValues.conditionsMet[c.conditionType] == c.@bool);

    private bool Ready
    {
        get
        {
            if (unitType is UnitType.Function) return true;
            if (unitType is UnitType.Function_For_Condition) return CorrectConditions;
            if (unitType is UnitType.Function_For_Input) return CorrectInput;
            if (unitType is UnitType.Function_For_Input_S) return CorrectInputS;
            if (unitType is UnitType.Function_For_Condition_And_Input)
                return CorrectInput && CorrectConditions;
            if (unitType is UnitType.Function_For_Condition_And_Input_S)
                return CorrectInputS && CorrectConditions;
            else return true;
        }
    }

    public void ExecuteUnit()
    {
        if (!Ready) return;
        for (int i = 0; i < functions.Count; i++)
        {
            functions[i].Function();
        }
    }
}

public enum UnitType
{
    Function,
    Function_For_Condition,
    Function_For_Input,
    Function_For_Input_S,
    Function_For_Condition_And_Input,
    Function_For_Condition_And_Input_S,
}