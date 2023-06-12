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

    [SerializeField] private int playerIndex; // �׽�Ʈ ���� SerializeField �߰���

    private PlayerModelChanger playerModelChanger;
    // WaitingRoomCanvas�� �ִ� GetPlayerModelChanger()�Լ��� ���ؼ� ��ũ��Ʈ�� �޾ƿ;���
    // �� �ڰ� �Ͼ�� ����

    
    public void SetPlayerModelChanger(PlayerModelChanger modelChanger)
    {
        playerModelChanger = modelChanger;
    }

    /// <summary>
    /// Customize Canvas�� �÷��̾� Index�� �������ش�
    /// </summary>
    /// <param name="playerPositionIndex"></param>
    public void SetCustomizeCanvasPlayerIndex(int playerPositionIndex)
    {
        playerIndex = playerPositionIndex;
    }

    #region OnClick �̺�Ʈ �Լ�

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
