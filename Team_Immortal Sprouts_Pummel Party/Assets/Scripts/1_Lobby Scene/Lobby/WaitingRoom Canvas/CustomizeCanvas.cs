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
    [SerializeField] private CustomData customData;
    private PlayerModelChanger playerModelChanger;

    [SerializeField] private int bodyColorIndex = 0;// �׽�Ʈ ���� SerializeField �߰���
    [SerializeField] private int hatIndex = 0;// �׽�Ʈ ���� SerializeField �߰���

    private const string hatPrefabPath = "Prefabs/Hats/";

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

    private void SetBodyColor(int index)
    {
        Texture2D bodyColor = customData.GetBodyColorFromData(index);
        playerModelChanger.SetBodyColor(bodyColor);
    }

    private void SetHat(int index)
    {
        if (playerModelChanger.GetCurrentHat() != null)
        {
            playerSlot.defaultPrefabPool.Destroy(playerModelChanger.GetCurrentHat());
        }

        GameObject newHat = null;
        GameObject hat = customData.GetHatFromData(index);
        if (customData.GetHatFromData(index) != null)
        {
            newHat = playerSlot.defaultPrefabPool.Instantiate(hat.name, playerModelChanger.GetHatPosition(), Quaternion.identity);
        }
        
        playerModelChanger.SetHatOnPlayer(newHat);
        
    }

    private int SetButtonIndex(bool isRightButton, int index, int length) // ���� 7��(6), ���� 8��(7)
    {
        if (isRightButton) // ������ ��ư�� �����ٸ�
        {
            // �ε����� �����ؾ��Ѵ�
            ++index;
            if (length <= index)
            {
                index = 0;
            }
        }
        else // ���� ��ư�� �����ٸ�
        {
            // �ε����� �����ؾ��Ѵ�
            --index;
            if (index < 0)
            {
                index = length- 1;
            }
        }

        return index;

    }

    #region OnClick �̺�Ʈ �Լ�

    public void OnClick_Body_LeftButton()
    {
        bodyColorIndex = SetButtonIndex(false, bodyColorIndex, customData.bodyColors.Length);
        SetBodyColor(bodyColorIndex);
    }

    public void OnClick_Body_RightButton()
    {
        bodyColorIndex = SetButtonIndex(true, bodyColorIndex, customData.bodyColors.Length);
        SetBodyColor(bodyColorIndex);
    }

    public void OnClick_Hat_LeftButton()
    {
        hatIndex = SetButtonIndex(false, hatIndex, customData.hats.Length);
        SetHat(hatIndex);
    }

    public void OnClick_Hat_RightButton()
    {
        hatIndex = SetButtonIndex(true, hatIndex, customData.hats.Length);
        SetHat(hatIndex);
    }

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
