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

    private void Start()
    {
        gameFrameWork.OnFirtstStartBoardGame.Invoke();
    }
}
