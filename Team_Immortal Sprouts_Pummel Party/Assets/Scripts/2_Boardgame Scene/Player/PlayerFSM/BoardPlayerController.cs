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
    [SerializeField] private Material[] playerMaterials;
    private Animator animator;
    private Rigidbody rigidbody;

    private PlayerInput playerInput;
    private InputAction rouletteTouchAction;

    public StateMachine stateMachine { get; private set; }
    private Dictionary<int, PlayerState> stateDictionary = new Dictionary<int, PlayerState>();

    #region 플레이어 상태
    private HoveringState Hovering;
    private MoveStartState MoveStart;
    private MoveInProgressState MoveInProgress;
    private MoveEndState MoveEnd;
    private TacklingState Tackling;
    private BumpedState Bumped;
    private DraggedState Dragged;
    private DieState Die;

    #endregion

    public UnityEvent<int> OnRouletteStopped = new UnityEvent<int>();
    public UnityEvent<Material[]> OnSetPlayerMaterial = new UnityEvent<Material[]>();

    private bool isEggable = false;

    

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


    #region Public 함수들

    /// <summary>
    /// 플레이어의 황금알 획득 가능여부를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetIsPlayerEggable() => isEggable;

    /// <summary>
    /// 플레이어의 황금알 획득 가능여부를 설정하는 함수
    /// </summary>
    /// <param name="isGettable"></param>
    public void SetPlayerEggable(bool isGettable) => isEggable = isGettable;

    /// <summary>
    /// 플레이어가 공격 당했을때 파티클 재생 함수
    /// </summary>
    public void PlayerDamagedParticle() => damagedParticle.Play();

    /// <summary>
    /// 플레이어의 이동이 가능하게끔 만들어주는 함수
    /// </summary>
    /// <param name="isMovable"></param>
    public void ControlCanMove(bool isMovable) => MoveInProgress.ControlCanMove(isMovable);

    /// <summary>
    /// 플레이어의 이동이 끝나 다시 Hovering 상태로 다시 전이시킬 수 있는 함수
    /// </summary>
    /// <param name="isFinished"></param>
    public void ControlMoveFinished(bool isFinished) => MoveEnd.isMoveFinished = isFinished;

    /// <summary>
    /// PlayerAnimID를 이용해 원하는 상태를 받아올 수 있는 함수
    /// </summary>
    /// <param name="stateHashNumber"></param>
    /// <returns></returns>
    public T GetDesiredState<T>(int stateHashNumber) where T : PlayerState => stateDictionary[stateHashNumber] as T;

    /// <summary>
    /// PlayerAnimID 를 이용해 원하는 상태로 전이시킬 수 있는 함수
    /// </summary>
    /// <param name="stateHashNumber"></param>
    public void ChangeToDesiredState(int stateHashNumber) => stateMachine.ChangeState(stateDictionary[stateHashNumber]);

    #endregion





    #region Private 함수들
    private void stopRoulette(InputAction.CallbackContext context) => roulette.ShowDiceResult().Forget();

    private void conveyDiceResult(int diceResult)
    {
        enableRoulette(false); // 룰렛을 꺼주고
        OnRouletteStopped?.Invoke(diceResult); // 값을 전달한다
    }

    private void enableRoulette(bool shouldTurnOn) => roulette.gameObject.SetActive(shouldTurnOn);

    private void initializePlayerStates()
    {
        Hovering = new HoveringState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.HOVERING);
        stateDictionary.Add(BoardgamePlayerAnimID.HOVERING, Hovering);

        MoveStart = new MoveStartState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVESTART);
        stateDictionary.Add(BoardgamePlayerAnimID.MOVESTART, MoveStart);

        MoveInProgress = new MoveInProgressState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVEINPROGRESS);
        stateDictionary.Add(BoardgamePlayerAnimID.MOVEINPROGRESS, MoveInProgress);

        MoveEnd = new MoveEndState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.MOVEEND);
        stateDictionary.Add(BoardgamePlayerAnimID.MOVEEND, MoveEnd);

        Tackling = new TacklingState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.TACKLING);
        stateDictionary.Add(BoardgamePlayerAnimID.TACKLING, Tackling);

        Bumped = new BumpedState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.TACKLING);
        stateDictionary.Add(BoardgamePlayerAnimID.BUMPED, Bumped);

        Dragged = new DraggedState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.DRAGGED);
        stateDictionary.Add(BoardgamePlayerAnimID.DRAGGED, Dragged);

        Die = new DieState(this, stateMachine, animator, rigidbody, BoardgamePlayerAnimID.DIE);
        stateDictionary.Add(BoardgamePlayerAnimID.DIE, Die);


        OnSetPlayerMaterial?.Invoke(playerMaterials); // Die 상태의 PlayerMaterials 초기세팅
    }

    #endregion

}
