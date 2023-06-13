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

    // �÷��̾��� ��������� ���� Dictionary
    private Dictionary<Player, int> playerOrderDictionary = new Dictionary<Player, int>();   

    // �÷��̾��� �г����� ���� Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    // �÷��̾��� �� ������ ���� Dictionary
    private Dictionary<Player, Texture2D> playerBodyColorDictionary = new Dictionary<Player, Texture2D>();

    // �÷��̾ �����ϰ� �ִ� ���ڸ� ���� Dictionary
    private Dictionary<Player, GameObject> playerHatDictionary = new Dictionary<Player, GameObject>();


}
