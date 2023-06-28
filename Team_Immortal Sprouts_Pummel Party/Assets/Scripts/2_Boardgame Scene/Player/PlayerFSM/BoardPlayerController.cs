using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardPlayerController : MonoBehaviour
{
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
        
    }

    private void Start()
    {
        stateMachine.InitializeState(HoveringState);
    }

    private void Update()
    {
        
    }

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
