using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private ArrowSwitch arrowSwitch;
    [SerializeField] private Island nextIsland;
    [SerializeField] private RotationIsland curIsland;
    [SerializeField] [Range(1f, 3f)] private float appearTime = 2f;
    private Vector3 initialPosition;
    private Vector3 downPosition;
    private bool isTouchable;

    private void Awake()
    {
        initialPosition = transform.position;
        downPosition = initialPosition;
        downPosition.y -= 3f;
        transform.position = downPosition;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ArrowAppear().Forget();
    }


    private void OnMouseUpAsButton() // 게임뷰에서 이 스크립트를 들고 있는 게임오브젝트를 터치했을 때
    {
        if (isTouchable) // 터치가 가능한 상태일 때
        {
            arrowSwitch.TurnOffSwitch(); // 방향 화살표를 꺼준다

            // 다음 목적지를 정해준다
            Vector3 nextPosition = nextIsland.GetCurrentPosition();
            curIsland.SetNextPosition(nextPosition);

            // 회전할 각도를 구하고 회전시킨다
            Quaternion targetRotation = Quaternion.LookRotation(curIsland.GetCurrentPosition() - nextPosition);
            curIsland.ActivateRotatation(targetRotation);
        }
    }

    /// <summary>
    /// 방향 화살표가 수면 아래에서 올라오는 함수
    /// </summary>
    public async UniTaskVoid ArrowAppear()
    {
        await ExtensionMethod.Vector3LerpExtension(transform, downPosition, initialPosition, appearTime);

        isTouchable = true; // 다 올라오면 터치가 가능한 상태로 만들어준다
    }

    /// <summary>
    /// 방향 화살표가 수면 아래로 내려가는 함수
    /// </summary>
    public async UniTaskVoid ArrowDisappear()
    {
        await ExtensionMethod.Vector3LerpExtension(transform, initialPosition, downPosition, appearTime);

        gameObject.SetActive(false);
    }
}
