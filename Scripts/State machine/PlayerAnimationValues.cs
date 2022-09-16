using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using State_machine;
using UnityEngine;

public partial class PlayerAnimationValues : MonoBehaviour
{
    public Condition condition;
    
    public Dictionary<ConditionType, bool> conditionsMet = new Dictionary<ConditionType, bool>()
    {
        {ConditionType.True, true},
        {ConditionType.End, false},
        {ConditionType.Peak, false},
        {ConditionType.Ground_Below, false},
        {ConditionType.Ground_L, false},
        {ConditionType.Ground_R, false},
        {ConditionType.Grounded, false},
        {ConditionType.Wall_L, false},
        {ConditionType.Wall_R, false},
        {ConditionType.Tilt_L, false},
        {ConditionType.Tilt_R, false},
        {ConditionType.Tilt_B, false},
        {ConditionType.Tilt_F, false},
        {ConditionType.Obstacle_F, false},
        {ConditionType.Obstacle_F_T, false},
        {ConditionType.Obstacle_F_D, false},
        {ConditionType.Obstacle_L, false},
        {ConditionType.Obstacle_R, false},
    };
    

    [HideInInspector] public Animator animator;

    public float groundBelowDist,
        groundedDist,
        lRGroundDist,
        lRWallDist,
        lRObstacleDist,
        fObsDist,
        fdObsDist,
        ftObsDist;

    public string movementIndex = "Movement Index";
    public string speedModifier = "Speed Modifier";

    [SerializeField] private float speedModifierValue = 1;
    public LayerMask layerMask;
    public InputManager inputManager;


    public void End() => conditionsMet[ConditionType.End] = true;
    public void Peak() => conditionsMet[ConditionType.Peak] = true;
    public void UnPeak() => conditionsMet[ConditionType.Peak] = false;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat(speedModifier, speedModifierValue);
    }

    private void Update()
    {
        RaycastCheks();
        InputTiltCheck();

#if UNITY_EDITOR
        animator.SetFloat(speedModifier, speedModifierValue);
#endif
    }

    private void RaycastCheks()
    {
        Vector3 position = transform.position + Vector3.up;
        RaycastCheck(position, Vector3.down, ConditionType.Ground_Below, "Ground",
            groundBelowDist);

        RaycastCheck(position, Vector3.left, ConditionType.Wall_L, "Wall",
            lRWallDist);

        RaycastCheck(position, Vector3.right, ConditionType.Wall_R, "Wall",
            lRWallDist);

        RaycastCheck(position, Vector3.left, ConditionType.Obstacle_L, "Obstacle",
            lRObstacleDist);

        RaycastCheck(position, Vector3.right, ConditionType.Obstacle_R, "Obstacle",
            lRObstacleDist);

        RaycastCheck(position, Vector3.down, ConditionType.Grounded, "Ground",
            groundedDist);
        position.x += lRGroundDist;
        RaycastCheck(position, Vector3.down, ConditionType.Ground_R, "Ground",
            groundedDist);
        position.x -= 2 * lRGroundDist;
        RaycastCheck(position, Vector3.down, ConditionType.Ground_L, "Ground",
            groundedDist);
        position.x += lRGroundDist;
        RaycastCheck(position, Vector3.forward, ConditionType.Obstacle_F, "Obstacle",
            fObsDist);
        position.y -= fdObsDist;
        RaycastCheck(position, Vector3.forward, ConditionType.Obstacle_F_D, "Obstacle",
            fObsDist);
        position.y += fdObsDist + ftObsDist;
        RaycastCheck(position, Vector3.forward, ConditionType.Obstacle_F_T, "Obstacle",
            fObsDist);
        position.y -= ftObsDist;
    }

    private void InputTiltCheck()
    {
        conditionsMet[ConditionType.Tilt_R] = (inputManager.Tilt.x > 0);
        conditionsMet[ConditionType.Tilt_L] = (inputManager.Tilt.x < 0);
        conditionsMet[ConditionType.Tilt_F] = (inputManager.Tilt.y > 0);
        conditionsMet[ConditionType.Tilt_B] = (inputManager.Tilt.y < 0);
    }

    private void RaycastCheck(Vector3 position, Vector3 direction, ConditionType conditionType, String tag,
        float distance)
    {
        RaycastHit hit;
        Physics.Raycast(position, direction, out hit, distance, layerMask);
        if (hit.collider == null)
        {
            conditionsMet[conditionType] = false;
            return;
        }

        if (hit.collider.CompareTag(tag))
            conditionsMet[conditionType] = true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 position = transform.position + Vector3.up;

        RayGizmo(position, Vector3.down, groundBelowDist, ConditionType.Ground_Below);

        RayGizmo(position, Vector3.down, groundedDist, ConditionType.Grounded);

        RayGizmo(position, Vector3.left, lRWallDist, ConditionType.Wall_L);

        RayGizmo(position, Vector3.right, lRWallDist, ConditionType.Wall_R);

        RayGizmo(position, Vector3.forward, fObsDist, ConditionType.Obstacle_F);

        RayGizmo(position, Vector3.left, lRObstacleDist, ConditionType.Obstacle_L);

        RayGizmo(position, Vector3.right, lRObstacleDist, ConditionType.Obstacle_R);

        position.x += lRGroundDist;
        RayGizmo(position, Vector3.down, groundedDist, ConditionType.Ground_R);
        position.x -= 2 * lRGroundDist;
        RayGizmo(position, Vector3.down, groundedDist, ConditionType.Ground_L);
        position.x += lRGroundDist;

        position.y += ftObsDist;
        RayGizmo(position, Vector3.forward, fObsDist, ConditionType.Obstacle_F_T);
        position.y -= fdObsDist + ftObsDist;
        RayGizmo(position, Vector3.forward, fObsDist, ConditionType.Obstacle_F_D);
        position.y += fdObsDist;
        
        condition.Debug();
    }

    private void RayGizmo(Vector3 position, Vector3 direction, float dist, ConditionType condition)
    {
        Gizmos.color = conditionsMet[condition] ? Color.green : Color.black;
        Gizmos.DrawLine(position, position + direction * dist);
    }
#endif

    [System.Serializable]
    public class StringType
    {
        [HorizontalGroup("H"), HideLabel] public string str;
        [HorizontalGroup("H"), HideLabel] public List<String> strs;
    }
}