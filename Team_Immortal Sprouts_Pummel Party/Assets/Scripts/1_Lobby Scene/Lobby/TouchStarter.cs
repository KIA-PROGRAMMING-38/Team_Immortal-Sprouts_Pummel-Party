using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchStarter : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject touchGuide;
    private void Start()
    {
        Managers.PhotonManager.OnConnectedToMasterServer.RemoveListener(turnOnTouchGuide);
        Managers.PhotonManager.OnConnectedToMasterServer.AddListener(turnOnTouchGuide);

    }

    private void OnDisable()
    {
        Managers.PhotonManager.OnConnectedToMasterServer.RemoveListener(turnOnTouchGuide);
    }

    private void turnOnTouchGuide() => touchGuide.SetActive(true);
}
