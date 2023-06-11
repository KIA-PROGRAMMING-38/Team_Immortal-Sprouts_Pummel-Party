using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerSlot playerSlot;
    [SerializeField] private TMP_Text nicknameInputField;
    [SerializeField] private Image bodyColor;
    [SerializeField] private Button bodyLeftButton;
    [SerializeField] private Button bodyRightButton;
    [SerializeField] private TMP_Text hatText;
    [SerializeField] private Button hatLeftButton;
    [SerializeField] private Button hatRightButton;
    [SerializeField] private Button confirmButton;

    #region OnClick 이벤트 함수

    public void OnClick_ConfirmButton()
    {
        playerSlot.ActivateSelectCanvas(true);
        SetPlayerNickname(nicknameInputField.text);
    }

    #endregion



    private void SetPlayerNickname(string inputPlayerName)
    {
        playerSlot.GetSelectCanvas().SetPlayerNickName(inputPlayerName);
    }
}
