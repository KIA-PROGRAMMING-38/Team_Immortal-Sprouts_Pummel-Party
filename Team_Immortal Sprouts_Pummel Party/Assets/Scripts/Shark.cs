using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;
using static UnityEngine.Rendering.DebugUI;

public class Shark : MonoBehaviour
{
    [Header("---------------------Shark Rotate Around Island----------------------")]
    [SerializeField] private Transform sharkIslandTransform;
    [SerializeField] [Range(30f, 150f)] private float rotateSpeed = 40f;

    [Header("----------------------Shark Jump----------------------")]
    [SerializeField] private Transform bezierPoint;
    [SerializeField] private bool isAttack = false; // 테스트 위해 SerializeField 함
    [SerializeField] private float jumpTime = 2f;

    [Header("----------------------Shark Look At Player----------------------")]
    [SerializeField] private Transform playerPos;
    [SerializeField] private bool isLookingPlayer = false; // 테스트 위해 SerializeField 함

    [SerializeField] private float ySpinTime = 1f;
    [SerializeField] private float zSpinTime = 1f;



    private void Start()
    {
        RotateAroundIsland().Forget();
        SharkLookAtPlayer().Forget();
    }

    
    private float defaultX = 0f; // 이 각도가 상어가 회전하는 각도임.. Quaternion.LookRotation()이 안먹혀... 쏴리...
    private float defaultY = -110f;
    private float defaultZ = 23f;


    private async UniTaskVoid SharkLookAtPlayer()
    {
        await UniTask.WaitUntil(() => isAttack == true);

        //await SpinYaxis(defaultY);
        
        //await SpinZaxis(defaultZ);

        isLookingPlayer = true;

        await SharkWaitJump();

        //await SpinYaxis(defaultY);
        
        //await SpinZaxis(defaultZ + 10f);

        isAttack = false;

        SharkLookAtPlayer().Forget();
    }
    
    private async UniTask<bool> SharkWaitJump()
    {
        await UniTask.WaitUntil(() => isLookingPlayer == true);
        
        isLookingPlayer = false;

        float elapsedTime = 0f;


        oppositePosition = GetOppositePosition(transform.position, sharkIslandTransform.position);
        Vector3 initialPosition = transform.position;
        

        float initialZAngle = transform.rotation.eulerAngles.z;
        float targetZAngle = -transform.rotation.eulerAngles.z;
        float changeZAngle = 0f;
        Vector3 sharkRotation = transform.rotation.eulerAngles;

        while (elapsedTime <= jumpTime)
        {
            elapsedTime += Time.deltaTime;

            transform.position = SharkJumpTowardsOpposite(initialPosition, bezierPoint.position, oppositePosition, elapsedTime / jumpTime);

            //changeZAngle = Lerp(initialZAngle, targetZAngle, elapsedTime / jumpTime);
            //sharkRotation.Set(sharkRotation.x, sharkRotation.y, changeZAngle);
            //transform.rotation = Quaternion.Euler(sharkRotation);

            await UniTask.Yield();
        }

        //isAttack = false;

        return true;
        //SharkLookAtPlayer().Forget();
    }


    private Vector3 rotateAxis = Vector3.up;
    private async UniTaskVoid RotateAroundIsland()
    {
        while (true)
        {
            while (!isAttack)
            {
                transform.RotateAround(sharkIslandTransform.position, rotateAxis, -rotateSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
            await UniTask.Yield();
        }
    }


    private Vector3 oppositePosition;
    private Vector3 GetOppositePosition(Vector3 currentPosition, Vector3 sharkIslandPosition)
    {
        Vector3 offSet = sharkIslandPosition - currentPosition;
        offSet.y = currentPosition.y; // 상어는 타일보다 아래에 있으므로, y값을 다시 변경해준다

        return offSet + sharkIslandPosition;
    }

    private Vector3 SharkJumpTowardsOpposite(Vector3 initialPosition, Vector3 middleUpPosition, Vector3 targetPosition, float t)
    {
        Vector3 firstMiddle = PrimaryBezierCurve(initialPosition, middleUpPosition, t);
        Vector3 secondMiddle = PrimaryBezierCurve(middleUpPosition, targetPosition, t);

        return PrimaryBezierCurve(firstMiddle, secondMiddle, t);
    }
    private Vector3 PrimaryBezierCurve(Vector3 start, Vector3 end, float t) => Vector3.Lerp(start, end, t);

    private float Lerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }

    private async UniTask<bool> SpinYaxis(float _defaultY)
    {
        float elapsedTime = 0f;

        float initialYAngle = transform.rotation.eulerAngles.y;
        float targetYAngle = transform.rotation.eulerAngles.y + _defaultY;

        float changeYAngle = 0f;

        Vector3 sharkRotation = transform.rotation.eulerAngles;

        while (elapsedTime <= ySpinTime)
        {
            elapsedTime += Time.deltaTime;
            changeYAngle = Lerp(initialYAngle, targetYAngle, elapsedTime / ySpinTime);
            sharkRotation.Set(sharkRotation.x, changeYAngle, sharkRotation.z);
            transform.rotation = Quaternion.Euler(sharkRotation);
            await UniTask.Yield();
        }

        return true;
    }

    private async UniTask<bool> SpinZaxis(float _defaultZ)
    {
        float elapsedTime = 0f;

        float initialZAngle = transform.rotation.eulerAngles.z;
        float targetZAngle = transform.rotation.eulerAngles.z + _defaultZ;

        float changeZAngle = 0f;

        Vector3 sharkRotation = transform.rotation.eulerAngles;

        while (elapsedTime <= zSpinTime)
        {
            elapsedTime += Time.deltaTime;
            changeZAngle = Lerp(initialZAngle, targetZAngle, elapsedTime / ySpinTime);
            sharkRotation.Set(sharkRotation.x, sharkRotation.y, changeZAngle);
            transform.rotation = Quaternion.Euler(sharkRotation);
            await UniTask.Yield();
        }

        return true;
    }
}
