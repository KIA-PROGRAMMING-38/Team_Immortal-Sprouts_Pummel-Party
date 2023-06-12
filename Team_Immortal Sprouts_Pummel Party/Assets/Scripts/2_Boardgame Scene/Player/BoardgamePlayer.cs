using Cysharp.Threading.Tasks;
using UnityEngine;


public class BoardgamePlayer : MonoBehaviour
{
    private Dice _dice;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _dice = new Dice();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateCurrentIsland();
    }

    [SerializeField] private bool _canRoll = false;  // 프레임워크랑 연결하기 전에 테스트하려고 열어둠
    private int _moveCount;
    private void OnRollDice()
    {
        if (_canRoll == false)
        {
            return;
        }

        _canRoll = false;

        _moveCount = _dice.Roll();
        HelpMoveAsync().Forget();
    }

    private Island _currentIsland;
    private const int WAIT_TIME_BEFORE_MOVE = 1000;
    private bool _canMoveOnDirectionIsland;
    private async UniTaskVoid HelpMoveAsync()
    {
        if (_currentIsland.CompareTag("RotationIsland")) // 회전 섬에서 출발하는 경우 섬의 회전이 끝날 때까지 대기
        {
            if (0 < _moveCount)
            {
                await UniTask.WaitUntil(() => _currentIsland.GetComponent<RotationIsland>().GetRotationStatus() == true);
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

        if (_currentIsland.GetCurrentPosition() == _destIslandPosition)
        {
            await LookForward();
            return;
        }

        _animator.SetBool(BoardgamePlayerAnimID.IS_MOVING, true);

        while (_moveCount >= 1)
        {
            float elapsedTime = 0f;
            start = _rigidbody.position;
            end = _destIslandPosition;
            mid = GetMidPoint(start, end);

            _moveCount -= 1;

            await LookNextDestIsland((end - start).normalized);

            while (elapsedTime <= MOVE_TIME)
            {
                elapsedTime += Time.deltaTime;
                _rigidbody.MovePosition(SecondaryBezierCurve(start, mid, end, elapsedTime * 2));

                await UniTask.Yield();
            }

            UpdateCurrentIsland();
            CheckReachableIsland();
            await UniTask.Yield();
        }

        _animator.SetBool(BoardgamePlayerAnimID.IS_MOVING, false);

        await LookForward();
    }

    private const float ROTATE_TIME = 1f;
    private async UniTask<bool> LookNextDestIsland(Vector3 dir)
    {
        // 회전타일에서부터 출발하는 턴에서는 회전하지 않음
        if (_currentIsland.CompareTag("RotationIsland") && _canMoveOnDirectionIsland)
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

            if (transform.position != _destIslandPosition)
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
        Physics.Raycast(transform.position, Vector3.down * 3f, out hit, int.MaxValue, LayerMask.GetMask("Island"));

        if (hit.collider != null)
        {
            _currentIsland = hit.collider.gameObject.GetComponentInParent<Island>();
        }
        else
        {
            Debug.Log("섬 감지 안됨");
        }
    }

    private Vector3 _destIslandPosition;
    private void CheckReachableIsland()
    {
        if (_moveCount >= 1)
        {
            _destIslandPosition = _currentIsland.GetNextPosition();
        }
        else if (_moveCount == -1)
        {
            _destIslandPosition = _currentIsland.GetPrevPosition();
            _moveCount = 1;
        }
        else
        {
            _destIslandPosition = _currentIsland.GetCurrentPosition();
        }
    }
}
