using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    
    

    public void Init(int playerCount)
    {
        photonPlayers = new Player[playerCount];
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




}
