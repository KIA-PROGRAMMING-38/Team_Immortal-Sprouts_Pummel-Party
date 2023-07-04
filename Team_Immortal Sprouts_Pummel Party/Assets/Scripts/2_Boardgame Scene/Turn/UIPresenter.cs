using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPresenter : MonoBehaviour
{
    public UnityEvent OnSetupMinigameInfo;

    [SerializeField] private TMP_Text miniGameName;
    [SerializeField] private TMP_Text miniGameInfo;
    [SerializeField] private Image miniGameImage;
    [SerializeField] private GameImageData imageData;
    public void SetInfo()
    {
        GameManager.Instance.SellectMiniGameNumber();
        int miniGameNumber = (int)PhotonNetwork.CurrentRoom.CustomProperties[Propertise.miniGameKey];

        miniGameName.text = GameManager.Instance.miniGameDialogs[miniGameNumber]["Name"].ToString();
        miniGameInfo.text = GameManager.Instance.miniGameDialogs[miniGameNumber]["Text"].ToString();

        miniGameImage.sprite = imageData.miniGameImages[miniGameNumber];
    }

}
