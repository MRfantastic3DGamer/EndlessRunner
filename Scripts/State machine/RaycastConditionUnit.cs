using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class RaycastConditionUnit
{
    
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


    [HideLabel]
    public String conditionName;

    [HorizontalGroup("H")]
    [VerticalGroup("H/1")]
    [HideLabel] public Transform transform;

    [VerticalGroup("H/1")]
    public void AddTransform()
    {
        
    }
    [HorizontalGroup("H")]
    public float length;
    [HorizontalGroup("H")]
    public LayerMask layerMask;

    public bool RayCheck()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, length, layerMask);
        return hit.collider != null;
    }

    public void DebugRay()
    {
        if(transform != null)
            Debug.DrawRay(transform.position,transform.forward * length,RayCheck()? Color.green : Color.black);
    }
}