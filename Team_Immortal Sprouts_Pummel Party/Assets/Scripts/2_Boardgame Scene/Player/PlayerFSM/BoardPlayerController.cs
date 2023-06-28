using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BoardPlayerController : MonoBehaviour
{
    [SerializeField] private Roulette roulette;
    private Animator animator;
    private Rigidbody rigidbody;

    private PlayerInput playerInput;
    private InputAction rouletteTouchAction;

    public StateMachine stateMachine { get; private set; }


    #region 플레이어 상태
    public HoveringState HoveringState { get; private set; }
    public WaitState WaitState { get; private set; }
    public MovingState MovingState { get; private set; }    
    public LookBackState LookBackState { get; private set; }
    public TacklingState TacklingState { get; private set; }
    public BumpedState BumpedState { get; private set; }
    public DraggedState DraggedState { get; private set; }
    public DieState DieState { get; private set; }

    #endregion

    public UnityEvent<int> OnConveyDiceResult = new UnityEvent<int>();


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rouletteTouchAction = playerInput.actions["RouletteTouch"];

        stateMachine = new StateMachine(); // 스테이트 머신 초기화

        initializePlayerStates(); // 플레이어 상태 초기화
    }

    private void OnEnable()
    {
        rouletteTouchAction.started -= stopRoulette;
        rouletteTouchAction.started += stopRoulette;
        roulette.OnRouletteFinished.RemoveListener(conveyDiceResult);
        roulette.OnRouletteFinished.AddListener(conveyDiceResult);
    }

    private void Start()
    {
        stateMachine.InitializeState(HoveringState);
    }

    private void Update()
    {
        Debug.Log($"현재 상태 = {stateMachine.currentState}");
    }

    private void OnDisable()
    {
        rouletteTouchAction.started -= stopRoulette;
        roulette.OnRouletteFinished.RemoveListener(conveyDiceResult);
    }


    private void stopRoulette(InputAction.CallbackContext context)
    {
        roulette.ShowDiceResult().Forget();
    }
    

    private void conveyDiceResult(int diceResult)
    {
        enableRoulette(false);
        if (1 <= diceResult)
        {
            OnConveyDiceResult?.Invoke(diceResult);
        }
    }

    private void enableRoulette(bool shouldTurnOn) => roulette.gameObject.SetActive(shouldTurnOn);

    public void EnableCanMove() => MovingState.canMove = true;











    private void initializePlayerStates()
    {
        HoveringState = new HoveringState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.HOVERING);
        WaitState = new WaitState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.WAIT);
        MovingState = new MovingState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVING);
        LookBackState = new LookBackState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.LOOKBACK);
        TacklingState = new TacklingState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.TACKLING);
        BumpedState = new BumpedState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.TACKLING);
        DraggedState = new DraggedState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.DRAGGED);
        DieState = new DieState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.DIE);
    }
}
