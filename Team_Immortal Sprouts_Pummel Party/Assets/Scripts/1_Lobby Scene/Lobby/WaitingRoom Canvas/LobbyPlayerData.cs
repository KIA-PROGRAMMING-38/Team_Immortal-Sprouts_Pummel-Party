using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerData
{
    // �÷��̾ ���� ����Ʈ
    private List<Player> currentPlayers = new List<Player>();   

    // �÷��̾��� ��ġ�� ���� Dictionary
    private Dictionary<Player, int> playerPositionDictionary = new Dictionary<Player, int>();   

    // �÷��̾��� �г����� ���� Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    


}
