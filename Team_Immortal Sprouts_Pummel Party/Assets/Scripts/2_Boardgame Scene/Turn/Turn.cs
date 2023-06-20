using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GAME_TYPE
{
    FIRST_ENTER_PRIZE,
    LAST_ENTER_PRTZE,
    POINT_ENTER_PRIZE
}

public static class Turn
{
    public static Queue<int> TurnOrder;
    public static Stack<int> LastEnterOrder;
    public static SortedSet<(int, int)> PointOrder;
    
    /// <summary>
    /// 미니게임에서 플레이어가 결승섬에 들어오거나 탈락할때 호출하는 메서드
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerEnterOrder"></param>
    public static void EnterPlayer(GAME_TYPE type, int playerEnterOrder)
    {
        switch (type)
        {
            case GAME_TYPE.FIRST_ENTER_PRIZE:
                TurnOrder.Enqueue(playerEnterOrder);
                break;

            case GAME_TYPE.LAST_ENTER_PRTZE:
                LastEnterOrder.Push(playerEnterOrder);
                break;
        }
    }

    /// <summary>
    /// 포인트로 순위를 환산하는 미니게임에서 결과가 정해진 후 호출할 메서드
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerEnterOrder"></param>
    /// <param name="point"></param>
    public static void EnterPlayer(GAME_TYPE type, int point, int playerEnterOrder)
    {
        if (type == GAME_TYPE.POINT_ENTER_PRIZE)
        {
            PointOrder.Add((point, playerEnterOrder));
        }
    }

    /// <summary>
    /// 플레이어의 순위를 임시로 저장한 컨테이너에서 사용할 컨테이너로 이동
    /// </summary>
    public static void SetContainer()
    {
        if (TurnOrder.Count != 0)
        {
            return;
        }

        if (LastEnterOrder.Count == 0)
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                TurnOrder.Enqueue(PointOrder.Last().Item2);
                PointOrder.Remove(PointOrder.Last());
            }
        }
        else if (PointOrder.Count == 0)
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                TurnOrder.Enqueue(LastEnterOrder.Pop());
            }
        }
    }
    /// <summary>
    /// 다음턴의 플레이어 넘버를 반환
    /// </summary>
    /// <returns></returns>
    public static int OnPlayer()
    {
        return TurnOrder.Dequeue();
    }

    
}
