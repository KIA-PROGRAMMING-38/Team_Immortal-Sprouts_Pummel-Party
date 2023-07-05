using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FrameWorkType
{
    OnBoardFirst,
    OnBoardBegin,
    OnBoardTurnStart,
    OnBoardTurnEnd,
    OnBoardAllTurnEnd,
    OnMiniStart,
    OnMiniEnd
}
public class FrameWorkManager
{
    public UnityEvent[] FrameWorkEvents = new UnityEvent[Enum.GetValues(typeof(FrameWorkType)).Length];

    /// <summary>
    /// 프레임워크 이벤트에 함수를 구독하는 함수 => 보통 OnEnable에서 쓰일듯?
    /// </summary>
    /// <param name="frameWorkType"></param>
    /// <param name="action"></param>
    public void BindEvents(FrameWorkType frameWorkType, UnityAction action)
    {
        FrameWorkEvents[(int)frameWorkType].RemoveListener(action);
        FrameWorkEvents[(int)frameWorkType].AddListener(action);
    }

    /// <summary>
    /// 프레임워크 이벤트에 함수를 제거하는 함수 => 보통 OnDisable에서 쓰일듯?
    /// </summary>
    /// <param name="frameWorkType"></param>
    /// <param name="action"></param>
    public void DeleteEvents(FrameWorkType frameWorkType, UnityAction action)
    {
        FrameWorkEvents[(int)frameWorkType].RemoveListener(action);
    }

    /// <summary>
    /// 프레임워크 이벤트를 발동시키는 함수
    /// </summary>
    /// <param name="frameWorkType"></param>
    public void InvokeFrameWorkEvent(FrameWorkType frameWorkType)
    {
        FrameWorkEvents[(int)frameWorkType]?.Invoke();
    }

}
