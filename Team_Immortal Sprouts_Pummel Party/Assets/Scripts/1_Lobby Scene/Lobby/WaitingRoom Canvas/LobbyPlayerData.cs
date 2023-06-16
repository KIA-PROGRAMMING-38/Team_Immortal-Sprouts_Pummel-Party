using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerData : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;
    [SerializeField] private CustomData customData;
    [SerializeField] private bool[] colorIndexing;
    private PhotonView dataPV;
    private void Start()
    {
        colorIndexing = new bool[customData.bodyColors.Length];
    }

    /// <summary>
    /// �־��� �ε����� ���� UI���� �� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="colorIndex"></param>
    /// <returns></returns>
    public Color GetBackgroundColorData(int colorIndex)
    {
        return customData.colors[colorIndex];
    }

    /// <summary>
    /// �־��� �ε����� ���� UI���� ���� �̸��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="hatIndex"></param>
    /// <returns></returns>
    public string GetBackgroundHatTextData(int hatIndex)
    {
        return customData.hatTexts[hatIndex];
    }


    /// <summary>
    /// ���� �迭�� ���ð��� ���θ� ������Ʈ ���ִ� �Լ�
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="isTakeOver"></param>
    public void UpdateColorIndexing(int enterOrder, bool isTakeOver)
    {
        // true�� ��������
        colorIndexing[enterOrder] = isTakeOver;
    }


    public int GetHatTypeCount() => customData.bodyColors.Length;
    public int GetBodyColorCount() => customData.bodyColors.Length;


    /// <summary>
    /// ���� �迭�� ���� ������ �ε����� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="lastIndex"></param>
    /// <param name="requestedIndex"></param>
    /// <param name="isRightButton"></param>
    /// <param name="isFirstEntry"></param>
    /// <returns></returns>
    public int GetCapableBodyIndex(int lastIndex, int requestedIndex, bool isRightButton, bool isFirstEntry)
    {
        if (!isFirstEntry)
        {
            colorIndexing[lastIndex] = false; // ���� ���� �ִ� ���� �����Ѵ�
        }

        int targetIndex = 0;

        int addValue = 0;

        if (isRightButton)
        {
            addValue = 1;
        }
        else
        {
            addValue = -1;
        }

        if (colorIndexing[requestedIndex] == false)
        {
            targetIndex = requestedIndex;
        }
        else
        {
            while (true)
            {
                lastIndex += addValue;
                if (lastIndex < 0)
                {
                    lastIndex = customData.bodyColors.Length - 1;
                }
                else if (customData.bodyColors.Length <= lastIndex)
                {
                    lastIndex = 0;
                }


                if (colorIndexing[lastIndex] == false)
                {
                    targetIndex = lastIndex;
                    break;
                }
            }
        }


        colorIndexing[targetIndex] = true;
        return targetIndex;
    }


    /// <summary>
    /// �������� ����並 ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public PhotonView GetDataPV()
    {
        if (dataPV == null)
        {
            dataPV = GetComponent<PhotonView>();
        }

        return dataPV;
    }

    /// <summary>
    /// �����Ϳ� ���� �÷��̾� ���� ������ ��� �Լ�
    /// </summary>
    /// <param name="newPlayer"></param>
    /// <param name="enterOrder"></param>
    /// <param name="nickName"></param>
    /// <param name="bodyColorIndex"></param>
    /// <param name="hatIndex"></param>
    public void AddPlayerData(Player newPlayer, int enterOrder, string nickName, int bodyColorIndex, int hatIndex)
    {
        playerOrderDictionary.Add(newPlayer, enterOrder);
        playerNameDictionary.Add(newPlayer, nickName);
        playerBodyColorDictionary.Add(newPlayer, bodyColorIndex);
        playerHatDictionary.Add(newPlayer, hatIndex);
    }

    /// <summary>
    /// �����Ϳ��� ������ �÷��̾� ���� ������ ���ִ� �Լ�
    /// </summary>
    /// <param name="newPlayer"></param>
    public void RemovePlayerData(Player newPlayer)
    {
        playerOrderDictionary.Remove(newPlayer);
        playerNameDictionary.Remove(newPlayer);
        playerBodyColorDictionary.Remove(newPlayer);
        playerHatDictionary.Remove(newPlayer);
    }


    // �÷��̾��� ��������� ���� Dictionary
    private Dictionary<Player, int> playerOrderDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// �־��� �÷��̾��� ��������� ����� �����Ϳ��� �̾� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerEnterOrder(Player player)
    {
        return playerOrderDictionary[player];
    }

    // �÷��̾��� �г����� ���� Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    /// <summary>
    /// �־��� �÷��̾��� �г����� ����� �����Ϳ��� �̾� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public string GetPlayerNickName(Player player)
    {
        return playerNameDictionary[player];
    }

    /// <summary>
    /// �־��� �г����� �����Ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newNickName"></param>
    public void UpdateNickName(Player updatePlayer, String newNickName)
    {
        playerNameDictionary[updatePlayer] = newNickName;
    }


    // �÷��̾��� �� ������ ���� Dictionary
    private Dictionary<Player, int> playerBodyColorDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// �־��� �÷��̾��� ���� �ε����� ����� �����Ϳ��� �̾� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerBodyColorIndex(Player player)
    {
        return playerBodyColorDictionary[player];
    }

    /// <summary>
    /// �־��� ���� �ε����� �����Ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newBodyIndex"></param>
    public void UpdateBodyIndex(Player updatePlayer, int newBodyIndex)
    {
        playerBodyColorDictionary[updatePlayer] = newBodyIndex;
    }


    // �÷��̾ �����ϰ� �ִ� ���ڸ� ���� Dictionary
    private Dictionary<Player, int> playerHatDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// �־��� �÷��̾��� ���� �ε����� ����� �����Ϳ��� �̾� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerHatIndex(Player player)
    {
        return playerHatDictionary[player];
    }

    /// <summary>
    /// �־��� ���� �ε����� �����Ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newHatIndex"></param>
    public void UpdateHatIndex(Player updatePlayer, int newHatIndex)
    {
        playerHatDictionary[updatePlayer] = newHatIndex;
    }

}
