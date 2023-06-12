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

    [SerializeField] private int playerIndex; // 테스트 위해 SerializeField 추가함

    private PlayerModelChanger playerModelChanger;
    // WaitingRoomCanvas에 있는 GetPlayerModelChanger()함수를 통해서 스크립트를 받아와야함
    // 좀 자고 일어나서 하자

    
    public void SetPlayerModelChanger(PlayerModelChanger modelChanger)
    {
        playerModelChanger = modelChanger;
    }

    /// <summary>
    /// Customize Canvas의 플레이어 Index를 설정해준다
    /// </summary>
    /// <param name="playerPositionIndex"></param>
    public void SetCustomizeCanvasPlayerIndex(int playerPositionIndex)
    {
        playerIndex = playerPositionIndex;
    }

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
