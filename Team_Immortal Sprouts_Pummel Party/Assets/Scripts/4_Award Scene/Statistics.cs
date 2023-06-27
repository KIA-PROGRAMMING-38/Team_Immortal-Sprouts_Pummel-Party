using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Statistics
{
    private static readonly int playerMaxIndex = 5;
    // item1 = 승리횟수 , item2 = 꼴지횟수, item3 = 보드게임에서 가한 데미지
    private static (int victoryCount, int loseCount, int dealtDamage)[] statisticsData = new (int, int, int)[playerMaxIndex]; // 0번째 인덱스는 제외한다

    /// <summary>
    /// 보드게임에서 황금알섬을 지날때마다 플레이어의 황금알 개수를 업데이트하는 함수(혹시 몰라서 만듦)
    /// </summary>
    /// <param name="playerEnterOrder"></param>
    public static void UpdateEggStatus(int playerEnterOrder)
    {
        ++playerRank[playerEnterOrder].eggCount; // 한바퀴당 1개씩 준다는 가정하에
    }


    /// <summary>
    /// 미니게임 승리자와 꼴지를 매개변수로 넣어, 해당 플레이어들의 미니게임 관련 통계데이터를 업데이트 하는 함수
    /// </summary>
    /// <param name="winnerEnterOrder"></param>
    /// <param name="loserEnterOrder"></param>
    public static void UpdateMinigameStatus(int winnerEnterOrder, int loserEnterOrder)
    {
        statisticsData[winnerEnterOrder].Item1 += 1; // 승리자의 승리횟수를 +1
        statisticsData[loserEnterOrder].Item2 += 1; // 꼴지의 꼴지횟수를 +1
    }

    /// <summary>
    /// 보드게임에서 데미지를 줄때, 해당 플레이어의 데미지 관련 통계 데이터를 업데이트 하는 함수
    /// </summary>
    /// <param name="damagingPlayerEnterOrder"></param>
    /// <param name="dealtDamage"></param>
    public static void UpdateDamageStatus(int damagingPlayerEnterOrder, int dealtDamage)
    {
        statisticsData[damagingPlayerEnterOrder].Item3 += dealtDamage; // 데미지를 가한 플레이어의 피해량을 데미지만큼 ++해준다
    }

    private static readonly string victoryCountKey = "vicvictoryCount";
    private static List<int> mvpIndexList = new List<int>();
    /// <summary>
    /// 미니게임 승리 최다 횟수를 기록한 플레이어(들)의 입장순서가 담긴 리스트를 반환하는 함수 (여러명 일수 있음)
    /// </summary>
    /// <returns></returns>
    public static List<int> GetMVPIndex() => getIndexByItemName(mvpIndexList, victoryCountKey);


    private static readonly string loseCountKey = "loseCount";
    private static List<int> loserIndexList = new List<int>();
    /// <summary>
    /// 미니게임 꼴지 최다 횟수를 기록한 플레이어(들)의 입장순서가 담긴 리스트를 반환하는 함수 (여러명 일수 있음)
    /// </summary>
    /// <returns></returns>
    public static List<int> GetLoserIndex() => getIndexByItemName(loserIndexList, loseCountKey);


    private static readonly string damageDealtKey = "dealtDamage";
    private static List<int> fighterIndexList = new List<int>();
    /// <summary>
    /// 보드게임에서 최대 데미지를 기록한 플레이어(들)의 입장순서가 담긴 리스트를 반환하는 함수 (여러명 일수 있음)
    /// </summary>
    /// <returns></returns>
    public static List<int> GetFighterIndex() => getIndexByItemName(fighterIndexList, damageDealtKey);


    private static string enterOrderKey = "EnterOrder";
    private static readonly string eggCountKey = "EggCount";
    private static (int eggCount, Player player)[] playerRank = new (int, Player)[playerMaxIndex];
    private static readonly int mvpEggPlus = 3;
    private static readonly int loserEggPlus = 1;
    private static readonly int fighterEggPlus = 2;



    /// <summary>
    /// 최종 우승자(들)의 입장순서가 담긴 리스트를 반환하는 함수 (여러명 일수 있음)
    /// </summary>
    /// <returns></returns>
    public static List<int> GetFinalWinner()
    {
        List<int> finalWinners = new List<int>();


        // ------------------------- 만약 UpdateEggStatus로 알의 개수를 업데이트 한다면 이 부분은 삭제되어도 됌 ----------------------------

        //Dictionary<int, Player> playersInRoom = PhotonNetwork.CurrentRoom.Players; // actorNumber가 키값임

        //for (int i = 1; i <= playersInRoom.Count ;++i) 
        //{
        //    Player player = playersInRoom[i]; // 어떤 플레이어일진 모르겠으나 커스텀프로퍼티에서 enterNumber와 eggCount를 가져올 수 있음
        //    int enterOrder = (int)player.CustomProperties[enterOrderKey];
        //    int eggCount = (int)player.CustomProperties[eggCountKey];
        //    playerRank[enterOrder] = (eggCount, player); // 커스텀 프로퍼티로 등록된 입장순서와 황금알 개수를 받아온다
        //}

        // ----------------------------------------------------------------------------------------------------------------------------


        // 이제 mvp, loser, fighter 에 맞게끔 황금알 개수를 추가해줘야함
        calculateReward(GetMVPIndex(), mvpEggPlus);

        calculateReward(GetLoserIndex(), loserEggPlus);

        calculateReward(GetFighterIndex(), fighterEggPlus);

        // 이제 모든 황금알 개수를 다 비교해서 최종우승자를 가려야 함 (코드 중복이 있긴 하나, 컨테이너 타입이 달라서 쩝... ㅠ)
        int maxEggCount = -9999;

        for (int playerEnterOrder = 1; playerEnterOrder < playerRank.Length; ++playerEnterOrder)
        {
            int playerEggCount = playerRank[playerEnterOrder].eggCount;

            if (maxEggCount < playerEggCount) // 최대값이 갱신이 되었다면
            {
                maxEggCount = playerEggCount;
                finalWinners.Clear(); // 지난 인덱스를 비우고
                finalWinners.Add(playerEnterOrder); // 새로운 인덱스를 넣어준다
            }
            else if (maxEggCount == playerEggCount) // 동일한 최대값을 가지고 있다면
            {
                finalWinners.Add(playerEnterOrder); // 새로운 인덱스를 추가로 넣어준다
            }
        }

        return finalWinners;
    }

    private static void calculateReward(List<int> indexList, int rewardEggCount) // 상 종류에 따른 황금알 더해주는 함수
    {
        foreach (int playerIndex in indexList)
        {
            playerRank[playerIndex].eggCount += rewardEggCount;
        }
    }


    private static List<int> getIndexByItemName(List<int> indexList, string itemName) // 코드 중복을 예방하기 위한 함수화
    {
        int maxValue = -9999; // 결국 모두 높은 점수를 기준으로 산정함

        for (int playerEnterOrder = 1; playerEnterOrder < statisticsData.Length; ++playerEnterOrder)
        {
            int value = 0;

            switch (itemName) // 매개변수로 입력해준 수상분야에 따라 item 1,2,3 중 하나를 골라준다
            {
                case "vicvictoryCount":
                    value = statisticsData[playerEnterOrder].victoryCount;
                    break;
                case "loseCount":
                    value = statisticsData[playerEnterOrder].loseCount;
                    break;
                case "dealtDamage":
                    value = statisticsData[playerEnterOrder].dealtDamage;
                    break;
                default:
                    Debug.Log("잘못된 아이템 이름입니다");
                    break;
            }

            if (maxValue < value) // 최대값이 갱신이 되었다면
            {
                maxValue = value;
                indexList.Clear(); // 지난 인덱스를 비우고
                indexList.Add(playerEnterOrder); // 새로운 인덱스를 넣어준다
            }
            else if (maxValue == value) // 동일한 최대값을 가지고 있다면
            {
                indexList.Add(playerEnterOrder); // 새로운 인덱스를 추가로 넣어준다
            }
        }

        return indexList;
    }
}
