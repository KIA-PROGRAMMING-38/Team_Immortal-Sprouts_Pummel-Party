using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    public List<Dictionary<string, object>> HatDialog; // PrefabPool
    public List<Dictionary<string, object>> BodyDialog; // PrefabPool

    public void ReadCSV()
    {
        HatDialog = CSVReader.Read("CSVs/HatTable");
        BodyDialog = CSVReader.Read("CSVs/BodyTable");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerCount"></param>
    public void Init(int playerCount)
    {
        photonPlayers = new Player[playerCount];
    }

    /// <summary>
    /// 대기실에서 보드게임씬으로 넘어갈때 호출해줘야하는 함수
    /// </summary>
    /// <param name="player"></param>
    public void InitItemArray(Player player)
    {
        itemCountDict.Add(player, new int[Enum.GetValues(typeof(ItemType)).Length]);
    }

    #region 포톤 플레이어

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



    #region 커스터마이즈
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


    #region 보드게임 용

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
}
