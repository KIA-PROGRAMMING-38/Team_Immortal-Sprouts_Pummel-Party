using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatedRoomData : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;
    [SerializeField] private CustomData customData;
    private PhotonView dataPV;
    [field: SerializeField] public bool[] colorIndexing { get; set; } = new bool[Managers.DataManager.Player.BodyDialog.Count];
    [field: SerializeField] public bool[] IsPlayerPresent { get; set; }
    [field: SerializeField] public bool[] IsReady { get; set; }

    private void OnEnable()
    {
        Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsPlayerPresent = new bool[PhotonNetwork.CurrentRoom.MaxPlayers]);
        Managers.PhotonManager.OnJoinedNewRoom.AddListener(() => IsPlayerPresent = new bool[PhotonNetwork.CurrentRoom.MaxPlayers]);

        Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsReady = new bool[PhotonNetwork.CurrentRoom.MaxPlayers]);
        Managers.PhotonManager.OnJoinedNewRoom.AddListener(() => IsReady = new bool[PhotonNetwork.CurrentRoom.MaxPlayers]);
    }

    private void OnDisable()
    {
        Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsPlayerPresent = new bool[PhotonNetwork.CurrentRoom.MaxPlayers]);
        Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsReady = new bool[PhotonNetwork.CurrentRoom.MaxPlayers]);
    }

    /// <summary>
    /// 주어진 인덱스에 따라 UI상의 몸 배경색을 반환하는 함수
    /// </summary>
    /// <param name="colorIndex"></param>
    /// <returns></returns>
    public Color GetBackgroundColorData(int colorIndex)
    {
        return customData.colors[colorIndex];
    }

    /// <summary>
    /// 주어진 인덱스에 따라 UI상의 모자 이름을 반환하는 함수
    /// </summary>
    /// <param name="hatIndex"></param>
    /// <returns></returns>
    public string GetBackgroundHatTextData(int hatIndex)
    {
        return customData.hatTexts[hatIndex];
    }


    /// <summary>
    /// 색깔 배열의 선택가능 여부를 업데이트 해주는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="isTakeOver"></param>
    public void UpdateColorIndexing(int enterOrder, bool isTakeOver)
    {
        // true면 못가져감
        colorIndexing[enterOrder] = isTakeOver;
    }


    public int GetHatTypeCount() => customData.bodyColors.Length;
    public int GetBodyColorCount() => customData.bodyColors.Length;


    /// <summary>
    /// 색깔 배열의 선택 가능한 인덱스를 반환하는 함수
    /// </summary>
    /// <param name="lastIndex"></param>
    /// <param name="requestedIndex"></param>
    /// <param name="isRightButton"></param>
    /// <param name="isFirstEntry"></param>
    /// <returns></returns>
    public int GetCapableBodyIndex(int lastIndex, int requestedIndex, bool isRightButton, bool isFirstEntry)
    {
        if (!isFirstEntry)
        {
            colorIndexing[lastIndex] = false; // 현재 갖고 있는 색을 포기한다
        }

        int targetIndex = 0;

        int addValue = 0;

        if (isRightButton)
        {
            addValue = 1;
        }
        else
        {
            addValue = -1;
        }

        if (colorIndexing[requestedIndex] == false)
        {
            targetIndex = requestedIndex;
        }
        else
        {
            while (true)
            {
                lastIndex += addValue;
                if (lastIndex < 0)
                {
                    lastIndex = customData.bodyColors.Length - 1;
                }
                else if (customData.bodyColors.Length <= lastIndex)
                {
                    lastIndex = 0;
                }


                if (colorIndexing[lastIndex] == false)
                {
                    targetIndex = lastIndex;
                    break;
                }
            }
        }


        colorIndexing[targetIndex] = true;
        return targetIndex;
    }


    /// <summary>
    /// 데이터의 포톤뷰를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public PhotonView GetDataPV()
    {
        if (dataPV == null)
        {
            dataPV = GetComponent<PhotonView>();
        }

        return dataPV;
    }

    /// <summary>
    /// 데이터에 들어온 플레이어 관련 정보를 담는 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    /// <param name="enterOrder"></param>
    /// <param name="nickName"></param>
    /// <param name="bodyColorIndex"></param>
    /// <param name="hatIndex"></param>
    public void AddPlayerData(Player newPlayer, int enterOrder, string nickName, int bodyColorIndex, int hatIndex)
    {
        Managers.DataManager.Player.SetEnterOrder(newPlayer, enterOrder);
        Managers.DataManager.Player.SetNickName(newPlayer, nickName);
        Managers.DataManager.Player.SetBodyID(newPlayer, bodyColorIndex);
        Managers.DataManager.Player.SetHatID(newPlayer, hatIndex);

        //playerOrderDictionary.Add(newPlayer, enterOrder);
        //playerNameDictionary.Add(newPlayer, nickName);
        //playerBodyColorDictionary.Add(newPlayer, bodyColorIndex);
        //playerHatDictionary.Add(newPlayer, hatIndex);
    }

    /// <summary>
    /// 데이터에서 나가는 플레이어 관련 정보를 없애는 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    public void RemovePlayerData(Player newPlayer)
    {
        //playerOrderDictionary.Remove(newPlayer);
        //playerNameDictionary.Remove(newPlayer);
        //playerBodyColorDictionary.Remove(newPlayer);
        //playerHatDictionary.Remove(newPlayer);
    }


    // 플레이어의 입장순서를 담을 Dictionary
    //private Dictionary<Player, int> playerOrderDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// 주어진 플레이어의 입장순서를 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    //public int GetPlayerEnterOrder(Player player)
    //{
    //    return playerOrderDictionary[player];
    //}

    // 플레이어의 닉네임을 담을 Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    /// <summary>
    /// 주어진 플레이어의 닉네임을 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public string GetPlayerNickName(Player player)
    {
        return playerNameDictionary[player];
    }

    /// <summary>
    /// 주어진 닉네임을 데이터에 저장하는 함수
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newNickName"></param>
    public void UpdateNickName(Player updatePlayer, String newNickName)
    {
        playerNameDictionary[updatePlayer] = newNickName;
    }


    // 플레이어의 몸 색깔을 담을 Dictionary
    private Dictionary<Player, int> playerBodyColorDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// 주어진 플레이어의 몸색 인덱스를 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerBodyColorIndex(Player player)
    {
        return playerBodyColorDictionary[player];
    }

    /// <summary>
    /// 주어진 몸색 인덱스를 데이터에 저장하는 함수
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newBodyIndex"></param>
    public void UpdateBodyIndex(Player updatePlayer, int newBodyIndex)
    {
        playerBodyColorDictionary[updatePlayer] = newBodyIndex;
    }


    // 플레이어가 착용하고 있는 모자를 담을 Dictionary
    private Dictionary<Player, int> playerHatDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// 주어진 플레이어의 모자 인덱스를 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerHatIndex(Player player)
    {
        return playerHatDictionary[player];
    }

    /// <summary>
    /// 주어진 모자 인덱스를 데이터에 저장하는 함수
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newHatIndex"></param>
    public void UpdateHatIndex(Player updatePlayer, int newHatIndex)
    {
        playerHatDictionary[updatePlayer] = newHatIndex;
    }

}
