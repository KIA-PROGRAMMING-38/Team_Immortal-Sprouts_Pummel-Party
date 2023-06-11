using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerSlot playerSlot;
    [SerializeField] Button customizeButton;
    [SerializeField] Button readyButton;
    
    public void EnableButtons()
    {
        customizeButton.interactable = true;
        readyButton.interactable = true;
    }

    

    public void OnClick_ReadyButton()
    {

    }

}
