using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Island : MonoBehaviour, IActiveIsland
{
    [SerializeField] Island nextPlayerSpot;
    [SerializeField] Island prevPlayerSpot;
    [SerializeField] Transform currentPlayerSpot;

    [SerializeField] private bool isPlayerPresent; // 테스트 위해 SerializeField 입력
    [SerializeField] private Vector3 nextPosition; // 테스트 위해 SerializeField 입력
    [SerializeField] private Vector3 prevPosition; // 테스트 위해 SerializeField 입력
    [SerializeField] private Vector3 currentPosition; // 테스트 위해 SerializeField 입력

    /// <summary>
    /// 현재 섬의 플레이어 존재여부를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerPresence() => isPlayerPresent;

    /// <summary>
    /// 현재 섬의 플레이어 존재여부를 설정할 수 있는 함수
    /// </summary>
    /// <param name="_isPlayerOut"></param>
    public void SetPlayerPresence(bool _isPlayerOut) => isPlayerPresent = _isPlayerOut;

    /// <summary>
    /// 현재 섬의 플레이어 위치를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentPosition() => currentPosition;

    /// <summary>
    /// 플레이어가 향할 다음 섬의 위치를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextPosition() => nextPosition;

    /// <summary>
    /// 플레이어가 향할 이전 섬의 위치를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPrevPosition() => prevPosition;

    /// <summary>
    /// 현재 섬의 플레이어 위치를 설정하는 함수
    /// </summary>
    /// <param name="_position"></param>
    public void SetCurrentPosition(Vector3 _position) => currentPosition = _position;

    /// <summary>
    /// 플레이어가 향할 다음 섬의 위치를 설정하는 함수
    /// </summary>
    /// <param name="_nextPosition"></param>
    public void SetNextPosition(Vector3 _nextPosition) => nextPosition = _nextPosition;

    /// <summary>
    /// 플레이어가 향할 이전 섬의 위치를 설정하는 함수
    /// </summary>
    /// <param name="_prevPosition"></param>
    public void SetPrevPosition(Vector3 _prevPosition) => prevPosition = _prevPosition;

    /// <summary>
    /// 현재섬의 다음, 이전섬의 위치를 초기화하는 함수
    /// </summary>
    /// <returns></returns>
    protected async UniTaskVoid InitPositionSettings()
    {
        if (currentPlayerSpot != null)
            currentPosition = currentPlayerSpot.position;

        await UniTask.Delay(TimeSpan.FromSeconds(2)); // 모든 섬들의 currentPosition이 정해지기까지 기다리기 위하여 delay 함

        if (nextPlayerSpot != null)
            nextPosition = nextPlayerSpot.GetCurrentPosition();

        if (prevPlayerSpot != null)
            prevPosition = prevPlayerSpot.GetCurrentPosition();
    }

    //public abstract void ActivateIsland(Transform playerTransform = null);

    public virtual void ActivateOnMoveStart(Transform playerTransform = null) { }

    public virtual void ActivateOnMoveInProgress(Transform playerTransform = null) { }

    public virtual void ActivateOnMoveEnd(Transform playerTransform = null) { }
}
