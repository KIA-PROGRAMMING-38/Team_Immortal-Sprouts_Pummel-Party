using Cinemachine;
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
    [SerializeField] private ParticleSystem damagedParticle;
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

    public CinemachineVirtualCamera virtualCam; // 프레임워크가 해줘야하나 그냥 지금 임시로

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rouletteTouchAction = playerInput.actions["RouletteTouch"];

        stateMachine = new StateMachine(); // 스테이트 머신 초기화

        initializePlayerStates(); // 플레이어 상태 초기화

        CameraTrace.InitializeCamera(virtualCam, transform); // 임시로 하는거임
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

    
    /// <summary>
    /// 플레이어가 공격 당했을때 파티클 재생 함수
    /// </summary>
    public void PlayerDamagedParticle()
    {
        damagedParticle.Play();
    }
    /// <summary>
    /// 플레이어의 이동이 가능하게끔 만들어주는 함수
    /// </summary>
    /// <param name="isMovable"></param>
    public void ControlCanMove(bool isMovable)
    {
        MoveInProgress.canMove = isMovable;
    }

    /// <summary>
    /// 플레이어의 이동이 끝나 다시 Hovering 상태로 다시 전이시킬 수 있는 함수
    /// </summary>
    /// <param name="isFinished"></param>
    public void ControlMoveFinished(bool isFinished)
    {
        MoveEnd.isMoveFinished = isFinished;
    }


    /// <summary>
    /// 상어에게 끌려가는 상태로 전이 시키는 함수
    /// </summary>
    public void ChangeToDraggedState()
    {
        stateMachine.ChangeState(Dragged);
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
