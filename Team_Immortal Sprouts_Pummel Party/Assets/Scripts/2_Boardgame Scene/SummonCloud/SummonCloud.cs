using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SummonCloud : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cloudBody;
    [SerializeField] private Light spotLight;
    [SerializeField] private bool isSummonPlayer = false; // 테스트위해 SerializeField 넣었음
    
    [Header("----------------------Cloud-------------------------------------------------")]
    [SerializeField][Range(0.5f, 4f)] private float flyTime = 2f;
    [SerializeField][Range(0.5f, 3f)] private float sizeTime = 1.5f;
    [SerializeField][Range(1f, 3f)] private float gapDistance = 2f;

    private Vector3 initialSize = Vector3.one;
    private Vector3 bigSize = Vector3.one * 2f; // 2가 되었을 때 플레이어도 쏙 가리고, 괜찮은듯 함?
    private const float PIVOT_MODIFIER = 0.8f; // 구름 피봇이 중앙이 아니라서 이걸로 플레이어 중앙에 맞출 수 있음
    private readonly Color SUMMON_COLOR = Color.white; // 소환할때의 빛 색
    private readonly Color ESCAPE_COLOR = Color.green; // 무인도에서 데려갈때의 빛 색

    [Header("----------------------Player Control-------------------------------------------------")]
    [SerializeField][Range(1f, 2f)] private float resetRotationTime = 1f;
    [SerializeField][Range(20f, 50f)] private float rotateForce = 30f;
    [SerializeField][Range(3f, 5f)] private float moveTime = 3f;

    private Vector3 playerSummonPosition;
    private Quaternion playerInitialRotation;
    private Rigidbody playerRigid;
    private Animator playerAnimator;
    private Camera mainCam;
    private float cameraHeight;
    private float cameraWidth;

    private void Awake()
    {
        mainCam = Camera.main;
        cameraHeight = mainCam.orthographicSize * 2f; // orthographicsize가 절반이라 2를 곱해줌
        cameraWidth = cameraHeight * mainCam.aspect; // 비율을 곱해주면 width 얻을 수 있음
    }

    private void OnEnable()
    {
        playerRigid = playerTransform.gameObject.GetComponent<Rigidbody>(); // 지금은 잠시 직접 가져옴....  나중에 없애줘야함
        playerAnimator = playerTransform.gameObject.GetComponent<Animator>(); // 지금은 잠시 직접 가져옴....  나중에 없애줘야함
        playerInitialRotation = playerTransform.rotation; // 지금은 잠시 직접 가져옴....  나중에 없애줘야함
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallCloud(isSummonPlayer).Forget();
        }
    }

    #region Public 함수
    /// <summary>
    /// 구름에게 소환 또는 납치할 플레이어를 할당하는 함수
    /// </summary>
    /// <param name="playerTrans"></param>
    public void SetCurrentPlayer(Transform playerTrans)
    {
        playerTransform = playerTrans;
        playerRigid = playerTransform.gameObject.GetComponent<Rigidbody>();
        playerAnimator = playerTransform.gameObject.GetComponent<Animator>();
        playerInitialRotation = playerTransform.rotation;
    }

    /// <summary>
    /// 플레이어를 소환하고자 할때, 소환위치를 정해주는 함수
    /// </summary>
    /// <param name="summonPosition"></param>
    public void SetPlayerSummonPosition(Vector3 summonPosition)
    {
        playerSummonPosition = summonPosition;
    }

    /// <summary>
    /// 플레이어를 무인도에서 탈출시킬 구름을 불러오는 함수
    /// </summary>
    /// <returns></returns>
    public async UniTaskVoid CallCloud(bool isSummoningPlayer)
    {
        if (isSummoningPlayer)
        {
            Vector3 calledPosition = playerSummonPosition + Vector3.up * gapDistance;
            calledPosition.x = calledPosition.x - PIVOT_MODIFIER;
            transform.position = getCameraOutsidePosition(); // 구름을 카메라 위치에 따라 밖으로 이동시킨다
            await flyToSpot(calledPosition, flyTime); // 구름이 가야할 곳으로 간다
        }
        else
        {
            Vector3 targetPosition = playerTransform.position + Vector3.up * gapDistance;
            targetPosition.x = targetPosition.x - PIVOT_MODIFIER;
            await flyToSpot(targetPosition, flyTime); // 플레이어 머리위로 날아오고
        }

        await cloudResize(true); // 구름 크기를 키운다
        await controlPlayer(isSummoningPlayer);
    }

    #endregion

    #region Pirvate 함수
    private Vector3 getCameraOutsidePosition()
    {
        Vector3 cameraOutPosition = mainCam.transform.position + mainCam.transform.right * cameraWidth + mainCam.transform.up * cameraHeight;
        return cameraOutPosition;
    }

    private void emitSpotLight(bool isSummoningPlayer, bool shouldTurnOn)
    {
        if (shouldTurnOn)
        {
            spotLight.enabled = true;
            if (isSummoningPlayer)
                spotLight.color = SUMMON_COLOR;
            else
                spotLight.color = ESCAPE_COLOR;
        }
        else
        {
            spotLight.enabled = false;
        }
    }
    
    private async UniTask controlPlayer(bool isSummoningPlayer)
    {
        if (!playerTransform.gameObject.activeSelf) // 플레이어가 꺼져있다면
        {
            playerTransform.gameObject.SetActive(true); // 플레이어를 켜준다
        }

        emitSpotLight(isSummonPlayer, true); // 빛을 비추고
        playerAnimator.SetBool(BoardgamePlayerAnimID.IS_MOVING, true);
        Vector3 playerPosition = playerTransform.position;
        Vector3 cloudPosition = transform.position;

        playerSummonPosition = new Vector3(-2.5f, 2f, 0f); // 지금은 테스트를 위해 잠시 있음 나중에 없애야함...


        playerRigid.angularVelocity = UnityEngine.Random.insideUnitSphere * rotateForce; // 플레이어를 랜덤한 방향으로 회전시킨다

        if (isSummoningPlayer) // 플레이어를 소환하는 경우라면
        {
            await playerBodyControl(cloudPosition, playerSummonPosition, moveTime); // 플레이어를 구름 밖으로 빼낸다
            await resetPlayerRotation(playerInitialRotation); // 플레이어를 다시 원래 회전값으로 리셋한다
            playerAnimator.SetBool(BoardgamePlayerAnimID.IS_MOVING, false); // 플레이어를 IDLE 상태로 세팅한다
        }
        else // 플레이어를 무인도에서 탈출시키는 경우라면
        {
            await playerBodyControl(playerPosition, cloudPosition, moveTime); // 플레이어를 구름 안으로 들여보낸다
            playerTransform.gameObject.SetActive(false); // 플레이어를 꺼준다
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초를 기다리고 => 자연스러운 대기를 위해
        emitSpotLight(isSummoningPlayer, false); // 빛도 꺼준다
        await cloudResize(false); // 구름 크기를 다시 줄여준다
        await flyToSpot(getCameraOutsidePosition(), flyTime); // 카메라 밖의 공간으로 구름이 이동한다
    }

    private async UniTask playerBodyControl(Vector3 start, Vector3 end, float moveTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= moveTime)
        {
            playerTransform.position = Vector3.Lerp(start, end, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }
    
    private async UniTask resetPlayerRotation(Quaternion targetRotation)
    {
        playerRigid.angularVelocity = Vector3.zero;
        Quaternion initialRotation = playerTransform.rotation;
        float elapsedTime = 0f;
        while (elapsedTime <= resetRotationTime)
        {
            playerTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / resetRotationTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async UniTask cloudResize(bool shouldGetBigger)
    {
        if (shouldGetBigger)
        {
            await controlSize(initialSize, bigSize, sizeTime);
        }
        else
        {
            await controlSize(bigSize, initialSize, sizeTime);
        }
    }

    private async UniTask flyToSpot(Vector3 targetPosition, float flyTime)
    {
        float elapsedTime = 0f;

        Vector3 initialPosition = transform.position;
        Vector3 midPoint = Vector3.Lerp(initialPosition, targetPosition, 0.5f);
        float magnitude = Vector3.Magnitude(midPoint - targetPosition);
        Vector3 controlPoint = midPoint - Vector3.up * magnitude; // 크기를 곱해줘서 더 아래값을 가져올 수 있음

        while (elapsedTime <= flyTime)
        {
            float t = elapsedTime / flyTime;
            Vector3 m1 = Vector3.Lerp(initialPosition, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, targetPosition, t);

            transform.position = Vector3.Lerp(m1, m2, t);

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async UniTask controlSize(Vector3 startSize, Vector3 endSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            cloudBody.localScale = Vector3.Lerp(startSize, endSize, elapsedTime / duration);
            cloudBody.localPosition = applyLocalPosition(startSize, endSize);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private const float X_FACTOR = 0.725f; // 인스펙터창에서 사이즈를 늘렸을때 localPosition이 아래와 같은 계수를 가지고 증가하는 규칙을 확인하였음
    private const float Y_FACTOR = 0.01f;
    private const float Z_FACTOR = 0.25f;
    private Vector3 applyLocalPosition(Vector3 startSize, Vector3 endSize)
    {
        Vector3 localPositionVector = Vector3.zero;

        if ((endSize - startSize).magnitude > 0f) // 구름 사이즈가 커져야한다는 의미임
        {
            localPositionVector.Set((1 - cloudBody.localScale.x) * X_FACTOR, (1 - cloudBody.localScale.y) * Y_FACTOR, (1 - cloudBody.localScale.z) * Z_FACTOR);
        }
        else // 구름 사이즈가 작아져야한다는 의미임
        {
            localPositionVector.Set((cloudBody.localScale.x - 1) * X_FACTOR, (cloudBody.localScale.y - 1) * Y_FACTOR, (cloudBody.localScale.z - 1) * Z_FACTOR);
        }

        return localPositionVector;
    }

    #endregion
}
