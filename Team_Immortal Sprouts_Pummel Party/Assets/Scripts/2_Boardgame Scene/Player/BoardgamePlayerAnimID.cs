using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardgamePlayerAnimID
{
    public static readonly int IS_MOVING = Animator.StringToHash("isMoving");
    public static readonly int DAMAGED = Animator.StringToHash("Damaged");
    public static readonly int DIE = Animator.StringToHash("Die");
    public static readonly int DISSOLVED = Animator.StringToHash("Dissolved");
}
