using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class BoardgamePlayer : MonoBehaviour
{
    [SerializeField] private Dice dice;
    private Rigidbody rigidbody;
    private Animator animator;
    private Inventory inventory;
    public Inventory Inventory { get 
        {
            if(inventory == null)
            {
                inventory = new Inventory(this);
            }
            return inventory;
        } }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateCurrentIsland();
        Inventory.InitInventory();
        isOnStartIsland = true;
    }

    private int moveCount;
    public bool CanUseItem = false;

    public void OnDiceStoped()
    {
        moveCount = dice.ConveyDiceReuslt();
        HelpMoveAsync().Forget();
    }

    private Island currentIsland;
    private const int WAIT_TIME_BEFORE_MOVE = 1000;
    private bool _canMoveOnDirectionIsland;
    private async UniTaskVoid HelpMoveAsync()
    {
        if (currentIsland.CompareTag("RotationIsland")) // 회전 섬에서 출발하는 경우 섬의 회전이 끝날 때까지 대기
        {
            if (0 < moveCount)
            {
                await UniTask.WaitUntil(() => currentIsland.GetComponent<RotationIsland>().GetRotationStatus() == true);
                _canMoveOnDirectionIsland = true;
            }
        }

        CheckReachableIsland();

        await UniTask.Delay(WAIT_TIME_BEFORE_MOVE);
        Move().Forget();
    }

    private const float MOVE_TIME = 0.5f;
    private async UniTaskVoid Move()
    {
        Vector3 start;
        Vector3 end;
        Vector3 mid;

        if (currentIsland.GetCurrentPosition() == destIslandPosition)
        {
            await LookForward();
            return;
        }

        animator.SetBool(BoardgamePlayerAnimID.IS_MOVING, true);

        while (moveCount >= 1)
        {
            float elapsedTime = 0f;
            start = rigidbody.position;
            end = destIslandPosition;
            mid = GetMidPoint(start, end);

            moveCount -= 1;

            await LookNextDestIsland((end - start).normalized);

            while (elapsedTime <= MOVE_TIME)
            {
                rigidbody.MovePosition(SecondaryBezierCurve(start, mid, end, elapsedTime / MOVE_TIME));
                elapsedTime += Time.deltaTime;

                await UniTask.Yield();
            }

            UpdateCurrentIsland();
            CheckReachableIsland();
            await UniTask.Yield();
        }

        animator.SetBool(BoardgamePlayerAnimID.IS_MOVING, false);

        await LookForward();
    }

    private const float ROTATE_TIME = 1f;
    private async UniTask<bool> LookNextDestIsland(Vector3 dir)
    {
        // 회전타일에서부터 출발하는 턴에서는 회전하지 않음
        if (currentIsland.CompareTag("RotationIsland") && _canMoveOnDirectionIsland)
        {
            _canMoveOnDirectionIsland = false;
            return true;
        }

        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(dir);

        float elapsedTime = 0f;
        while (elapsedTime < ROTATE_TIME)
        {
            elapsedTime += Time.deltaTime;

            if (transform.position != destIslandPosition)
            {
                var lerpval = Quaternion.Lerp(start, end, elapsedTime / ROTATE_TIME);
                transform.rotation = lerpval;
            }
            await UniTask.Yield();
        }

        return true;
    }

    private Vector3 GetReverseVector(Vector3 vec) { return vec * -1f; }
    private async UniTask<bool> LookForward()
    {
        Vector3 lookDir = GetReverseVector(Vector3.forward);
        lookDir = lookDir.normalized;

        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(lookDir, transform.up);

        float elapsedTime = 0f;
        while (elapsedTime <= ROTATE_TIME)
        {
            elapsedTime += Time.deltaTime;
            var lerpval = Quaternion.Lerp(start, end, elapsedTime / ROTATE_TIME);
            transform.rotation = lerpval;
            await UniTask.Yield();
        }

        dice.OnAppearDice();

        return true;
    }

    private const int JUMP_HEIGHT = 5;
    private Vector3 GetMidPoint(Vector3 start, Vector3 end)
    {
        Vector3 mid = Vector3.Lerp(start, end, 0.5f);
        mid.y += JUMP_HEIGHT;
        return mid;
    }

    private Vector3 PrimaryBezierCurve(Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(p1, p2, t);
    }

    private Vector3 SecondaryBezierCurve(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 m0 = PrimaryBezierCurve(p1, p2, t);
        Vector3 m1 = PrimaryBezierCurve(p2, p3, t);

        return PrimaryBezierCurve(m0, m1, t);
    }

    private void UpdateCurrentIsland()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down * 10f, out hit, int.MaxValue, LayerMask.GetMask("Island"));

        if (hit.collider != null)
        {
            currentIsland = hit.collider.gameObject.GetComponentInParent<Island>();
        }
        else
        {
            Debug.Log("섬 감지 안됨");
        }
    }

    private Vector3 destIslandPosition;
    // TODO: 죽었을 때 시작 섬으로 가니까 true로 바꿔줘야함
    private bool isOnStartIsland;
    private void CheckReachableIsland()
    {
        if (moveCount >= 1)
        {
            if (isOnStartIsland == true) isOnStartIsland = false;

            destIslandPosition = currentIsland.GetNextPosition();
        }
        else if (moveCount == -1)
        {
            if(isOnStartIsland == true)
            {
                moveCount = 0;
            }
            else
            {
                destIslandPosition = currentIsland.GetPrevPosition();
                moveCount = 1;
            }
        }
        else
        {
            destIslandPosition = currentIsland.GetCurrentPosition();
        }
    }

    #region Damage
    [SerializeField] private ParticleSystem onDamagedParticle;
    public int Hp = 30;
    /// <summary>
    /// 플레이어에게 데미지를 줄 때 호출할 메소드
    /// </summary>
    public void GetDamage(int power)
    {
        Hp -= power;
        onDamagedParticle?.Play();
        animator.SetTrigger(BoardgamePlayerAnimID.DAMAGED);
    }
    #endregion
}
