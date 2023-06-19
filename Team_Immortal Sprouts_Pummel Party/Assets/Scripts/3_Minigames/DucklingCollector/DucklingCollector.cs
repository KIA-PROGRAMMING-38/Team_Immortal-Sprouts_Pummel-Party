using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class DucklingCollector : MonoBehaviour
{
    // 몸통박치기가 가능한 로직이 필요함
    // 다른 콜렉터가 사용한 대쉬에 날아가는 로직 필요함
    // 날아가는 건 맞은 방향의 반대방향 + 위로 살짝 랜덤하게 해야할듯?
    // 그리고 자기가 맞았다는 걸 알게 해줘야함
    // 명확하게 자기가 맞았다는걸 어떻게 알게 해줄까.. 흐음,....
    // 몸통박치기에 맞았으면 지닌 새끼오리를 다 떨구는 로직이 필요함
    // 우리와 닿았을때 우리 안에 새끼오리 넣는 로직이 필요함(단 본인 우리여야만 함)
    // 본인 우리가 아니라 다른 우리에 넣는것도 나름 재밌겠는데..?
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Transform ducklingPosition;
    [SerializeField] List<Duckling> ducklings = new List<Duckling>();
    [SerializeField] private MeshRenderer[] renderers;
    private Vector3 moveDirection;
    private float inputX; // 좌우 담당
    private float inputY;  // 앞뒤 담당
    private Rigidbody rigidbody;
    private BoxCollider collider;
    [SerializeField][Range(50f, 150f)] private float moveSpeed = 100f;

    private int playerLayerNum;
    private int ducklingLayerNum;


    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        playerMaterial = GetComponentInChildren<MeshRenderer>().material; // body에 있는 마티리얼 가져올꺼임
        SetPlayerColor(playerMaterial);
        playerLayerNum = LayerMask.NameToLayer("DucklingCollector");
        ducklingLayerNum = LayerMask.NameToLayer("Duckling");
    }


    private void FixedUpdate()
    {
        //moveDirection.Normalize(); 정규화를 하게 되면, 조이스틱을 조금 움직이던, 많이 움직이던 속도가 같아짐!정규화 피해야함!!!
        playerMovement(inputY, inputX);
    }

    /// <summary>
    /// 플레이어의 몸색을 정해주는 함수
    /// </summary>
    /// <param name="colorMaterial"></param>
    public void SetPlayerColor(Material colorMaterial)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = colorMaterial;
        }
    }

    private bool isMoving = true;
    /// <summary>
    /// PlayerInput에 구독시켜, 조이스틱을 움직일때 호출할 함수
    /// </summary>
    /// <param name="context"></param>
    public void OnJoyStickControl(InputAction.CallbackContext context)
    {
        if (isMoving)
        {
            Vector3 inputDirection = context.ReadValue<Vector2>();
            inputX = inputDirection.x;
            inputY = inputDirection.y;
        }
    }
    private void playerMovement(float inputY, float inputX)
    {
        if (isMoving)
        {
            moveDirection = transform.forward * inputY + transform.right * inputX;
            rigidbody.velocity = moveDirection * Time.fixedDeltaTime * moveSpeed;
            transform.Rotate(Vector3.up, inputX * Time.deltaTime * moveSpeed);
        }
    }

    private bool isTackable = true;
    /// <summary>
    /// PlayerInput에 구독시켜, 태클버튼을 누를때 호출할 함수
    /// </summary>
    /// <param name="context"></param>
    public void OnTackleButtonPressed(InputAction.CallbackContext context)
    {
        if (isTackable && context.started)
        {
            playerTackle().Forget();
        }
    }


    [SerializeField] float holdTime = 1f;
    [SerializeField] private float tackleForce = 10f;
    private bool isTackling = false;
    private async UniTaskVoid playerTackle()
    {
        isMoving = false; // 조이스틱 이동을 막는다
        isTackling = true;
        StartTackleTimer().Forget();

        setEmission(true);
        Physics.IgnoreLayerCollision(playerLayerNum, ducklingLayerNum, true); // 대쉬중 새끼오리 못먹게 막기

        rigidbody.velocity = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(holdTime));

        rigidbody.AddForce(transform.forward * tackleForce, ForceMode.Impulse);

        await UniTask.Delay(TimeSpan.FromSeconds(holdTime));
        Physics.IgnoreLayerCollision(playerLayerNum, ducklingLayerNum, false);
        setEmission(false);
        isMoving = true; // 다시 이동이 가능하게끔 한다
        isTackling = false;
    }

    private void setEmission(bool isTurnOn)
    {
        if (isTurnOn)
        {
            Color glowColor = playerMaterial.color;
            playerMaterial.SetColor("_EmissionColor", glowColor); // 본인색의 빛으로
            playerMaterial.EnableKeyword("_EMISSION"); // 발산한다
        }
        else
        {
            playerMaterial.DisableKeyword("_EMISSION");
        }
    }

    [SerializeField] private float tackleWaitTime = 10f;
    private async UniTaskVoid StartTackleTimer()
    {
        isTackable = false;
        await UniTask.Delay(TimeSpan.FromSeconds(tackleWaitTime));
        isTackable = true;
    }

    /// <summary>
    /// 플레이어의 콜라이더를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public BoxCollider GetPlayerCollider() => collider;

    /// <summary>
    /// 새끼오리의 색깔을 플레이어의 색깔로 바꿔주는 함수
    /// </summary>
    /// <param name="ducklingRenderers"></param>
    public void ChangeDucklingColor(MeshRenderer[] ducklingRenderers)
    {
        foreach (MeshRenderer element in ducklingRenderers)
        {
            element.material = playerMaterial;
        }
    }

    /// <summary>
    /// 새끼오리를 입양시키는 함수
    /// </summary>
    /// <param name="duckling"></param>
    public void AdoptDuckling(Duckling duckling) => ducklings.Add(duckling);

    /// <summary>
    /// 입양한 새끼오리가 따라갈 위치를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Transform GetFollowPosition()
    {
        if (ducklings.Count == 0) // 새끼오리가 없다면
        {
            return ducklingPosition; // 기본 저장된 위치를 전달한다
        }
        else // 입양한 새끼오리가 존재한다면
        {
            return ducklings[ducklings.Count - 1].GetFollowPosition(); // 마지막 새끼오리의 위치를 전달한다
        }
    }

    private void loseDucklings()
    {
        if (ducklings.Count != 0)
        {
            foreach (Duckling duckling in ducklings)
            {
                duckling.GetLost();
            }

            ducklings.Clear();
        }
    }

    private void putDucklingsIntoTheCage(DucklingCage ducklingCage)
    {
        if (ducklings.Count != 0) // 새끼오리가 존재한다면 
        {
            ducklingsJumpInOrder(ducklingCage).Forget();
        }
    }

    private async UniTaskVoid ducklingsJumpInOrder(DucklingCage cage)
    {
        cage.OpenCage(true); // 새장을 열어준다

        Transform destinationTransform = cage.GetDucklingDestination(ducklings.Count);

        foreach (Duckling duckling in ducklings)
        {
            duckling.PutDucklingIntoTheCage(destinationTransform);
            await UniTask.Delay(500); // 새끼오리끼리의 충돌방지용 0.5초 딜레이 추가
        }

        await UniTask.Delay(TimeSpan.FromSeconds(3f)); // 새끼오리 점프시간이 2초라서 넉넉하게 3초로 준다 
        ducklings.Clear(); // 새끼들을 전부 새장에 넣었으므로, 소지한 새끼오리들을 잃는다
        cage.OpenCage(false); // 새장을 닿아준다
    }

    /// <summary>
    /// 충돌당한 플레이어를 날려버리는 함수
    /// </summary>
    /// <param name="collisionDirection"></param>
    /// <returns></returns>
    public async UniTask CauseTackleCollision(Vector3 collisionDirection)
    {
        isMoving = false;
        isTackable = false;
        rigidbody.AddForce(collisionDirection * tackleForce * 0.5f, ForceMode.Impulse); // 부딪혀서 날아가고
        setEmission(true);
        loseDucklings(); // 새끼오리들을 잃는다
        await UniTask.Delay(1000);
        setEmission(false);
        isMoving = true;
        isTackable = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isTackling && collision.gameObject.CompareTag("DucklingCollector"))
        {
            Vector3 tackleDirection = transform.forward + Vector3.up; // 충돌방향 + 위로 1 up
            collision.gameObject.GetComponent<DucklingCollector>().CauseTackleCollision(tackleDirection).Forget();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BirdCage"))
        {
            DucklingCage ducklingCage = other.gameObject.GetComponent<DucklingCage>();
            putDucklingsIntoTheCage(ducklingCage);
        }
    }
}
