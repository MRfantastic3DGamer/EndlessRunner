using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class State : StateMachineBehaviour
{
    [Space(10), Searchable] public List<Unit> startUnits;
    [Space(10), Searchable] public List<Unit> units;
    [Space(10), Searchable] public List<Unit> endUnits;

    private static PlayerAnimationValues _animationValues;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animationValues = animator.transform.GetComponent<PlayerAnimationValues>();
        
        _animationValues.conditionsMet[ConditionType.End] = 
            _animationValues.conditionsMet[ConditionType.Peak] = false;
        
        for (int i = 0; i < startUnits.Count; i++)
            startUnits[i].ExecuteUnit();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < units.Count; i++)
            units[i].ExecuteUnit();
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animationValues.conditionsMet[ConditionType.End] = 
            _animationValues.conditionsMet[ConditionType.Peak] = false;
        for (int i = 0; i < endUnits.Count; i++)
            endUnits[i].ExecuteUnit();
    }
}

// OnStateMove is called right after Animator.OnAnimatorMove()
//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//{
//    // Implement code that processes and affects root motion
//}

// OnStateIK is called right after Animator.OnAnimatorIK()
//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//{
//    // Implement code that sets up animation IK (inverse kinematics)
//}