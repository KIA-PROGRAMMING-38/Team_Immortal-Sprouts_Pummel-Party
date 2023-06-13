using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerData : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;
    private PhotonView dataPV;
    
    
    public PhotonView GetDataPV()
    {
        if (dataPV == null)
        {
            dataPV = GetComponent<PhotonView>();
        }

        return dataPV;
    }

    public void AddPlayerData(Player newPlayer, int enterOrder, string nickName, Texture2D bodyColor, GameObject newHat)
    {
        playerOrderDictionary.Add(newPlayer, enterOrder);
        playerNameDictionary.Add(newPlayer, nickName);
        playerBodyColorDictionary.Add(newPlayer, bodyColor);
        playerHatDictionary.Add(newPlayer, newHat);
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

    // 플레이어의 닉네임을 담을 Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    // 플레이어의 몸 색깔을 담을 Dictionary
    private Dictionary<Player, Texture2D> playerBodyColorDictionary = new Dictionary<Player, Texture2D>();

    // 플레이어가 착용하고 있는 모자를 담을 Dictionary
    private Dictionary<Player, GameObject> playerHatDictionary = new Dictionary<Player, GameObject>();


}
