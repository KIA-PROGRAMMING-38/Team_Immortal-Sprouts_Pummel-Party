using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardgamePlayerAnimID
{
    public static readonly int HOVERING = Animator.StringToHash("Hovering");
    public static readonly int MOVESTART = Animator.StringToHash("MoveStart");
    public static readonly int MOVEINPROGRESS = Animator.StringToHash("MoveInProgress");
    public static readonly int MOVEEND = Animator.StringToHash("MoveEnd");
    public static readonly int TACKLING = Animator.StringToHash("Tackling");
    public static readonly int BUMPED = Animator.StringToHash("Bumped");
    public static readonly int DRAGGED = Animator.StringToHash("Dragged");
    public static readonly int DIE = Animator.StringToHash("Die");
}
