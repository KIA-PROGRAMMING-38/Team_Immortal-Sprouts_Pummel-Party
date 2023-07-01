using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBowl : RealItem
{
    public override void Use(BoardPlayerController player = null)
    {
        // 사용 로직
    }

    public override void OnTimeOut()
    {
        // 사용시간 다되었을떄 로직
    }
}
