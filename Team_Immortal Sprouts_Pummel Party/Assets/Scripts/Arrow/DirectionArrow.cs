using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Island nextIsland;
    [SerializeField] private RotationIsland curIsland;

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseUpAsButton() // 게임뷰에서 이 스크립트를 들고 있는 게임오브젝트를 터치했을 때
    {
        // 다음 목적지를 정해준다
        Vector3 nextPosition = nextIsland.GetCurrentPosition();
        curIsland.SetNextPosition(nextPosition);
        
        // 회전할 각도를 구하고 회전시킨다
        Quaternion targetRotation = Quaternion.LookRotation(curIsland.GetCurrentPosition() - nextPosition);
        curIsland.ActivateRotatation(targetRotation);
    }
}
