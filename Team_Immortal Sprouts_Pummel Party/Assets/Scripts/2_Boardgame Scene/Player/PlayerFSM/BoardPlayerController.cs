using Cysharp.Threading.Tasks;
using System;
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
    public HoveringState Hovering { get; private set; }
    public MoveStartState MoveStart { get; private set; }
    public MoveInProgressState MoveInProgress { get; private set; }
    public MoveEndState MoveEnd { get; private set; }
    public TacklingState Tackling { get; private set; }
    public BumpedState Bumped { get; private set; }
    public DraggedState Dragged { get; private set; }
    public DieState Die { get; private set; }

    #endregion

    public UnityEvent<int> OnRouletteStopped = new UnityEvent<int>();

    public bool isEggGettable { get; set; }

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
        stateMachine.InitializeState(Hovering);
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
        enableRoulette(false); // 룰렛을 꺼주고
        OnRouletteStopped?.Invoke(diceResult); // 값을 전달한다
    }

    private void enableRoulette(bool shouldTurnOn) => roulette.gameObject.SetActive(shouldTurnOn);

    

    public void ControlCanMove(bool isMovable)
    {
        MoveInProgress.canMove = isMovable;
    }

    public void ControlMoveFinished(bool isFinished)
    {
        MoveEnd.isMoveFinished = isFinished;
    }









    private void initializePlayerStates()
    {
        Hovering = new HoveringState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.HOVERING);
        MoveStart = new MoveStartState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVESTART);
        MoveInProgress = new MoveInProgressState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVING);
        MoveEnd = new MoveEndState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVEEND);
        Tackling = new TacklingState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.TACKLING);
        Bumped = new BumpedState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.TACKLING);
        Dragged = new DraggedState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.DRAGGED);
        Die = new DieState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.DIE);
    }
}
