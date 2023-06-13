using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerData
{
    // 플레이어를 담을 리스트
    private List<Player> currentPlayers = new List<Player>();   

    // 플레이어의 위치를 담을 Dictionary
    private Dictionary<Player, int> playerPositionDictionary = new Dictionary<Player, int>();   

    // 플레이어의 닉네임을 담을 Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    


}
