using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;

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
                GameObject playerModel = PhotonNetwork.Instantiate("Player",
                    spawnPoint._BoardPositions[playerEnterNumber].position, Quaternion.identity);
                playerView = playerModel.GetPhotonView();
                playerView.TransferOwnership(PhotonNetwork.CurrentRoom.Players[playerEnterNumber]);
                Transform hatPosition = playerModel.transform.GetChild(0);
                Player player = PhotonNetwork.CurrentRoom.Players[playerEnterNumber];
                if ((int)player.CustomProperties[PropertiesKey.hatKey] != 0)
                {
                    GameObject hat = PhotonNetwork.Instantiate(customData.hats[(int)player.CustomProperties[PropertiesKey.hatKey]].name,
                        hatPosition.position, Quaternion.identity);
                    hat.transform.SetParent(hatPosition,true);
                }
            }
        }
    }
}
