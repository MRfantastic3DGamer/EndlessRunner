using Sirenix.OdinInspector;

[System.Serializable]
public class UnitCondition
{
    [HorizontalGroup("H"), HideLabel] public ConditionType conditionType;
    [HorizontalGroup("H"), HideLabel] public bool @bool = true;
}

public enum ConditionType
{
    True,
    End,
    Ground_Below,
    Ground_L,
    Ground_R,
    Grounded,
    Wall_R,
    Wall_L,
    Tilt_L,
    Tilt_R,
    Tilt_F,
    Tilt_B,
    Obstacle_F,
    Obstacle_F_T,
    Obstacle_F_D,
    Obstacle_L,
    Obstacle_R,
    Peak,
}