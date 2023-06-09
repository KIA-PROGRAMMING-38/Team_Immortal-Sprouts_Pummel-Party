using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class FakeBoardGameManager : MonoBehaviour
{
    [SerializeField] BoardGameFrameWork gameFrameWork;
    [SerializeField] CustomData customData;
    [SerializeField] PositionData spawnPoint;

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

    private PhotonView playerView;
    private GameObject hat;
    private Transform hatPosition;
    private Transform bodyPosition;
    public void FirstSpawnPlayer()
    {

        for (int playerEnterNumber = 1; playerEnterNumber <= PhotonNetwork.CurrentRoom.PlayerCount; playerEnterNumber++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject playerModel = PhotonNetwork.Instantiate("Player",
                    spawnPoint._BoardPositions[playerEnterNumber].position, Quaternion.identity);
                playerView = playerModel.GetPhotonView();
                playerView.TransferOwnership(PhotonNetwork.CurrentRoom.Players[playerEnterNumber]);
                hatPosition = playerModel.transform.GetChild(0);
                bodyPosition = playerModel.transform.GetChild(1);
                Player player = PhotonNetwork.CurrentRoom.Players[playerEnterNumber];
                if ((int)player.CustomProperties[PropertiseKey.hatKey] != 0)
                {
                    hat = PhotonNetwork.Instantiate(customData.hats[(int)player.CustomProperties[PropertiseKey.hatKey]].name,
                        hatPosition.position, Quaternion.identity);
                    hat.transform.SetParent(bodyPosition, true);
                }
            }
        }
    }
}
