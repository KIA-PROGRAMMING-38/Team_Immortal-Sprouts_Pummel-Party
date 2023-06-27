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
        updateCurrentIsland();
        Inventory.InitInventory();
        isOnStartIsland = true;
    }

    private int moveCount;
    public bool CanUseItem = false;

    public void OnDiceStoped()
    {
        moveCount = dice.ConveyDiceReuslt();
        helpMoveAsync().Forget();
    }

    private Island currentIsland;
    private const int WAIT_TIME_BEFORE_MOVE = 1000;
    private bool canMoveOnDirectionIsland;
    private async UniTaskVoid helpMoveAsync()
    {
        await checkDepartureIsland();

        checkReachableIsland();

        await UniTask.Delay(WAIT_TIME_BEFORE_MOVE);
        move().Forget();
    }

    private async UniTask checkDepartureIsland()
    {
        if (currentIsland is RotationIsland && moveCount >= 1)
        {
            currentIsland.GetComponent<RotationIsland>().PopUpDirectionArrow(transform);

            await UniTask.WaitUntil(() => currentIsland.GetComponent<RotationIsland>().GetRotationStatus() == true);
            canMoveOnDirectionIsland = true;
        }
    }

    private async UniTask onExitDepartureIsland()
    {
        if (currentIsland is RotationIsland)
        {
            currentIsland.GetComponent<RotationIsland>().ActivateResetRotation();
        }
    }

    private async UniTask checkTransitIsland()
    {
        
    }

    private async UniTask checkDestIsland()
    {
        if(currentIsland is ItemIsland)
        {
            await currentIsland.GetComponent<ItemIsland>().Activate(this);
        }
    }

    private async UniTask checkHealIsland()
    {
        if (currentIsland is HealIsland)
        {
            await currentIsland.GetComponent<HealIsland>().OnActiveHealIsland(this);
        }
    }

    private const float MOVE_TIME = 0.5f;
    private async UniTaskVoid move()
    {
        Vector3 start;
        Vector3 end;
        Vector3 mid;

        if (currentIsland.GetCurrentPosition() == destIslandPosition)
        {
            await lookForward();
            return;
        }

        animator.SetBool(BoardgamePlayerAnimID.IS_MOVING, true);
        await onExitDepartureIsland();

        while (moveCount >= 1)
        {
            float elapsedTime = 0f;
            start = rigidbody.position;
            end = destIslandPosition;
            mid = getMidPoint(start, end);

            moveCount -= 1;

            await lookNextDestIsland((end - start).normalized);

            while (elapsedTime <= MOVE_TIME)
            {
                rigidbody.MovePosition(secondaryBezierCurve(start, mid, end, elapsedTime / MOVE_TIME));
                elapsedTime += Time.deltaTime;

                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }

            await checkTransitIsland();

            updateCurrentIsland();
            checkReachableIsland();
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }

        animator.SetBool(BoardgamePlayerAnimID.IS_MOVING, false);

        await lookForward();
    }

    private const float ROTATE_TIME = 1f;
    private async UniTask lookNextDestIsland(Vector3 dir)
    {
        // 회전타일에서부터 출발하는 턴에서는 회전하지 않음
        if (currentIsland is RotationIsland && canMoveOnDirectionIsland)
        {
            canMoveOnDirectionIsland = false;
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
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
    }

    private Vector3 getReverseVector(Vector3 vec) { return vec * -1f; }
    private async UniTask lookForward()
    {
        Vector3 lookDir = getReverseVector(Vector3.forward);
        lookDir = lookDir.normalized;

        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(lookDir, transform.up);

        float elapsedTime = 0f;
        while (elapsedTime <= ROTATE_TIME)
        {
            elapsedTime += Time.deltaTime;
            var lerpval = Quaternion.Lerp(start, end, elapsedTime / ROTATE_TIME);
            transform.rotation = lerpval;
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }



        await checkDestIsland();


        // TODO: 추후 턴 시작을 알리는 곳에서 해야 함
        dice.OnAppearDice();
    }

    private const int JUMP_HEIGHT = 5;
    private Vector3 getMidPoint(Vector3 start, Vector3 end)
    {
        Vector3 mid = Vector3.Lerp(start, end, 0.5f);
        mid.y += JUMP_HEIGHT;
        return mid;
    }

    private Vector3 primaryBezierCurve(Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(p1, p2, t);
    }

    private Vector3 secondaryBezierCurve(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 m0 = primaryBezierCurve(p1, p2, t);
        Vector3 m1 = primaryBezierCurve(p2, p3, t);

        return primaryBezierCurve(m0, m1, t);
    }

    private void updateCurrentIsland()
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
    private void checkReachableIsland()
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
    public int Hp = 20;
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

    #region Recover
    /// <summary>
    /// 플레이어의 체력을 회복할 때 호출할 메소드
    /// </summary>
    /// <param name="amount"></param>
    public void OnRecover(int amount)
    {
        int MaxHp = 30;

        Hp += amount;

        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
    }
    #endregion
}
