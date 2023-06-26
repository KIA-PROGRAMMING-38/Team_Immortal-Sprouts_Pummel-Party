using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cysharp.Threading.Tasks;

public class BoardGameManager : MonoBehaviour
{
    [SerializeField] BoardGameFrameWork gameFrameWork;
    [SerializeField] CustomData customData;
    [SerializeField] PositionData spawnPoint;
    private PhotonView playerView;

    private void Start()
    {
        if (Turn.isFirst)
        {
            gameFrameWork.OnFirtstStartBoardGame.Invoke();

            Turn.isFirst = false; 
        }
        else
        {
            gameFrameWork.OnStartBoardGame.Invoke();
        }
    }

    public void FirstSpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int playerEnterNumber = 1; playerEnterNumber <= PhotonNetwork.CurrentRoom.PlayerCount; playerEnterNumber++)
            {
                GameObject player = PhotonNetwork.Instantiate("Player",
                    spawnPoint._BoardPositions[playerEnterNumber].position, Quaternion.identity);
                playerView = player.GetPhotonView();
                playerView.TransferOwnership(PhotonNetwork.CurrentRoom.Players[playerEnterNumber]);
            }
        }
    }
}
