using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerData : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;
    [SerializeField] private CustomData customData;
    [SerializeField] private bool[] colorIndexing;
    private PhotonView dataPV;
    private void Start()
    {
        colorIndexing = new bool[customData.bodyColors.Length];
    }

    public void UpdateColorIndexing(int enterOrder, bool isTakeOver)
    {
        // true면 못가져감
        colorIndexing[enterOrder] = isTakeOver;
    }

    public int GetHatTypeCount() => customData.bodyColors.Length;
    public int GetBodyColorCount() => customData.bodyColors.Length;


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


    public PhotonView GetDataPV()
    {
        if (dataPV == null)
        {
            dataPV = GetComponent<PhotonView>();
        }

        return dataPV;
    }

    public void AddPlayerData(Player newPlayer, int enterOrder, string nickName, int bodyColorIndex, int hatIndex)
    {
        playerOrderDictionary.Add(newPlayer, enterOrder);
        playerNameDictionary.Add(newPlayer, nickName);
        playerBodyColorDictionary.Add(newPlayer, bodyColorIndex);
        playerHatDictionary.Add(newPlayer, hatIndex);
    }

    public void RemovePlayerData(Player newPlayer)
    {
        playerOrderDictionary.Remove(newPlayer);
        playerNameDictionary.Remove(newPlayer);
        playerBodyColorDictionary.Remove(newPlayer);
        playerHatDictionary.Remove(newPlayer);
    }

    public void ResetPlayerData()
    {
        playerOrderDictionary.Clear();
        playerNameDictionary.Clear();
        playerBodyColorDictionary.Clear();
        playerHatDictionary.Clear();
    }

    // 플레이어의 입장순서를 담을 Dictionary
    private Dictionary<Player, int> playerOrderDictionary = new Dictionary<Player, int>();

    public int GetPlayerEnterOrder(Player player)
    {
        return playerOrderDictionary[player];
    }

    // 플레이어의 닉네임을 담을 Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    public string GetPlayerNickName(Player player)
    {
        return playerNameDictionary[player];
    }

    public void UpdateNickName(Player updatePlayer, String newNickName)
    {
        playerNameDictionary[updatePlayer] = newNickName;
    }


    // 플레이어의 몸 색깔을 담을 Dictionary
    private Dictionary<Player, int> playerBodyColorDictionary = new Dictionary<Player, int>();

    public int GetPlayerBodyColorIndex(Player player)
    {
        return playerBodyColorDictionary[player];
    }
    public void UpdateBodyIndex(Player updatePlayer, int newBodyIndex)
    {
        playerBodyColorDictionary[updatePlayer] = newBodyIndex;
    }





    // 플레이어가 착용하고 있는 모자를 담을 Dictionary
    private Dictionary<Player, int> playerHatDictionary = new Dictionary<Player, int>();

    public int GetPlayerHatIndex(Player player)
    {
        return playerHatDictionary[player];
    }

    public void UpdateHatIndex(Player updatePlayer, int newHatIndex)
    {
        playerHatDictionary[updatePlayer] = newHatIndex;
    }

}
