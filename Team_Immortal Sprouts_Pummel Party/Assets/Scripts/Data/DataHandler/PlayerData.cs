using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AwardSortType
{
    MVP,
    LOSER,
    FIGHTER
}

public class PlayerData
{
    public List<Dictionary<string, object>> HatDialog; 
    public List<Dictionary<string, object>> BodyDialog; 
    public List<Dictionary<string, object>> AwardDialog;


    #region Init 함수들
    /// <summary>
    /// CSV를 읽어오는 초기화 함수
    /// </summary>
    public void ReadCSV()
    {
        HatDialog = CSVReader.Read("CSVs/HatTable");
        BodyDialog = CSVReader.Read("CSVs/BodyTable");
        AwardDialog = CSVReader.Read("CSVs/AwardTable");
    }

    /// <summary>
    /// 로비씬에서 대기실씬으로 넘어갈때 데이터 컨테이너를 초기화해주는 함수
    /// </summary>
    /// <param name="maxPlayerCount"></param>
    public void InitPhotonPlayerContainer(int maxPlayerCount)
    {
        photonPlayers = new Player[maxPlayerCount];
    }

    /// <summary>
    /// 대기실씬에서 보드게임씬으로 넘어갈때 데이터 컨테이너를 초기화해주는 함수
    /// </summary>
    public void InitPlayerDataContainer()
    {
        initItemArray();
        initEggPlusCount();
    }

    private void initItemArray()
    {
        for (int enterOrder = 1; enterOrder < PhotonNetwork.CurrentRoom.PlayerCount ; ++enterOrder)
        {
            Player player = GetPhotonPlayer(enterOrder);
            itemCountDict.Add(player, new int[Enum.GetValues(typeof(ItemType)).Length]);
        }
    }

    private void initEggPlusCount()
    {
        for (int i = 0; i < eggPlus.Length; ++i)
        {
            eggPlus[i] = (int)AwardDialog[i]["EggPlus"];
        }
    }

    #endregion


    #region 포톤 플레이어 - 포톤 플레이어 캐싱

    private Player[] photonPlayers;

    /// <summary>
    /// PhotonNetwork.Player와 입장순서를 바인딩해주는 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    /// <param name="enterOrder"></param>
    public void UpdatePhotonPlayers(Player newPlayer, int enterOrder) => photonPlayers[enterOrder] = newPlayer; 
    
    /// <summary>
    /// 입장순서에 맞는 플레이어를 반환하는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <returns></returns>
    public Player GetPhotonPlayer(int enterOrder) => photonPlayers[enterOrder];

    /// <summary>
    /// 플레이어 아웃게임시, 초기화해주는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    public void RemovePhotonPlayer(int enterOrder) => photonPlayers[enterOrder] = null;
    #endregion


    #region 커스터마이즈 - 모자, 몸색, 닉네임
    private Dictionary<Player, int> hatDict = new Dictionary<Player, int>();
    private Dictionary<Player, int> bodyDict = new Dictionary<Player, int>();
    private Dictionary<Player, string> nickNameDict = new Dictionary<Player, string>();

    /// <summary>
    /// 플레이의 모자 ID를 설정하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ID"></param>
    public void SetHatID(Player player, int ID) => hatDict[player] = ID;

    /// <summary>
    /// 플레이어의 몸색 ID를 설정하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ID"></param>
    public void SetBodyID(Player player, int ID) => bodyDict[player] = ID;

    /// <summary>
    /// 플레이어의 닉네임을 설정하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="nickName"></param>
    public void SetNickName(Player player, string nickName) => nickNameDict[player] = nickName;

    /// <summary>
    /// 플레이어의 모자 ID를 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetHatID(Player player) => hatDict[player];

    /// <summary>
    /// 플레이어의 몸색 ID를 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetBodyID(Player player) => bodyDict[player]; 

    /// <summary>
    /// 플레이어의 닉네임을 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public string GetNickName(Player player) => nickNameDict[player];

    #endregion


    #region 보드게임 용 - 이전좌표, HP, 아이템 개수, 황금알 개수

    private Dictionary<Player, Vector3> prevPosDict = new Dictionary<Player, Vector3>();
    private Dictionary<Player, int> hpDict = new Dictionary<Player, int>();
    private Dictionary<Player, int[]> itemCountDict = new Dictionary<Player, int[]>();
    private Dictionary<Player, int> eggCountDict = new Dictionary<Player, int>();


    /// <summary>
    /// 이전 좌표를 저장하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="pos"></param>
    public void SavePrevPos(Player player, Vector3 pos) => prevPosDict[player] = pos;

    /// <summary>
    /// 이전 좌표를 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Vector3 GetPrevPos(Player player) => prevPosDict[player];

    /// <summary>
    /// HP를 저장하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="hp"></param>
    public void SaveLastHP(Player player, int hp) => hpDict[player] = hp;

    /// <summary>
    /// HP를 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetHP(Player player) => hpDict[player];

    /// <summary>
    /// 아이템 개수를 +1 더해주는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemType"></param>
    public void AddItem(Player player, ItemType itemType) => ++itemCountDict[player][(int)itemType];

    /// <summary>
    /// 아이템 개수를 -1 빼주는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemType"></param>
    public void RemoveItem(Player player, ItemType itemType)
    {
        itemCountDict[player][(int)itemType] = Mathf.Max(0, --itemCountDict[player][(int)itemType]);
    }

    public void AddEggCount(Player player) => ++eggCountDict[player];
    public int GetEggCount(Player player) => eggCountDict[player];

    #endregion


    #region Award 용 - MVP, Loser, Fighter, 최종 우승자

    private Dictionary<Player, (int victoryCount, int loseCount, int dealtDamage)> statisticsDict = new Dictionary<Player, (int victoryCount, int loseCount, int dealtDamage)>();
    private List<Player>[] winnerContainer = new List<Player>[Enum.GetValues(typeof(AwardSortType)).Length];
    private int[] eggPlus = new int[Enum.GetValues(typeof(AwardSortType)).Length];

    /// <summary>
    /// 해당 플레이어들의 미니게임 관련 통계데이터를 업데이트 하는 함수
    /// </summary>
    /// <param name="winner"></param>
    /// <param name="loser"></param>
    public void UpdateMinigameStatus(Player winner, Player loser)
    {
        int newVictoryCount = statisticsDict[winner].victoryCount + 1;
        int newLoseCount = statisticsDict[loser].loseCount + 1;

        (int, int, int) newWinnerData = (newVictoryCount, statisticsDict[winner].loseCount, statisticsDict[winner].dealtDamage);
        (int, int, int) newLoserData = (newLoseCount, statisticsDict[loser].loseCount, statisticsDict[loser].dealtDamage);

        statisticsDict[winner] = newWinnerData;
        statisticsDict[loser] = newLoserData;
    }

    /// <summary>
    /// 보드게임에서 데미지를 줄때, 해당 플레이어의 데미지 관련 통계 데이터를 업데이트 하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <param name="dealtDamage"></param>
    public void UpdateDamageStatus(Player player, int dealtDamage)
    {
        int newDealtDamage = statisticsDict[player].dealtDamage + dealtDamage;

        (int, int, int) newData = (statisticsDict[player].victoryCount, statisticsDict[player].loseCount, newDealtDamage);

        statisticsDict[player] = newData;
    }

    /// <summary>
    /// 상 타입에 따른 플레이어 리스트를 반환하는 함수
    /// </summary>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public List<Player> getAwardPlayerList(AwardSortType sortType)
    {
        return getWinnerListBySortName(winnerContainer[(int)sortType], sortType);
    }

    /// <summary>
    /// 최종 우승자 리스트를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public List<Player> GetFinalWinner()
    {
        List<Player> finalWinners = new List<Player>();

        for (int awardType = 0; awardType < winnerContainer.Length ;++awardType)
        {
            calculateReward(winnerContainer[awardType], eggPlus[awardType]);
        }

        int maxEggCount = -9999;

        for (int enterOrder = 1; enterOrder < statisticsDict.Count; ++enterOrder)
        {
            Player player = GetPhotonPlayer(enterOrder);

            int playerEggCount = eggCountDict[player];

            if (maxEggCount < playerEggCount) // 최대값이 갱신이 되었다면
            {
                maxEggCount = playerEggCount;
                finalWinners.Clear(); // 지난 인덱스를 비우고
                finalWinners.Add(player); // 새로운 인덱스를 넣어준다
            }
            else if (maxEggCount == playerEggCount) // 동일한 최대값을 가지고 있다면
            {
                finalWinners.Add(player); // 새로운 인덱스를 추가로 넣어준다
            }
        }

        return finalWinners;
    }

    private void calculateReward(List<Player> playerList, int rewardEggCount) // 상 종류에 따른 황금알 더해주는 함수
    {
        foreach (Player player in playerList)
        {
            eggCountDict[player] += rewardEggCount;
        }
    }

    private List<Player> getWinnerListBySortName(List<Player> playerList, AwardSortType sortName) // 코드 중복을 예방하기 위한 함수화
    {
        int maxValue = -9999; // 결국 모두 높은 점수를 기준으로 산정함

        for (int enterOrder = 1; enterOrder <= statisticsDict.Count; ++enterOrder)
        {
            int value = 0;

            Player player = GetPhotonPlayer(enterOrder);

            switch (sortName) // 매개변수로 입력해준 수상분야에 따라 item 1,2,3 중 하나를 골라준다
            {
                case AwardSortType.MVP:
                    value = statisticsDict[player].victoryCount;
                    break;
                case AwardSortType.LOSER:
                    value = statisticsDict[player].loseCount;
                    break;
                case AwardSortType.FIGHTER:
                    value = statisticsDict[player].dealtDamage;
                    break;
                default:
                    break;
            }

            if (maxValue < value) // 최대값이 갱신이 되었다면
            {
                maxValue = value;
                playerList.Clear(); // 지난 인덱스를 비우고
                playerList.Add(player); // 새로운 인덱스를 넣어준다
            }
            else if (maxValue == value) // 동일한 최대값을 가지고 있다면
            {
                playerList.Add(player); // 새로운 인덱스를 추가로 넣어준다
            }
        }

        return playerList;
    }
    #endregion
}
