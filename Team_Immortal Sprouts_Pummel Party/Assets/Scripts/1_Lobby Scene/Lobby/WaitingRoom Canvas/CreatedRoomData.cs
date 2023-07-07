using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CreatedRoomData : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomPresenter presenter;
    [SerializeField] private TMP_Text roomNameText;
    private PhotonView dataPV;

    [field : SerializeField] public Transform[] PositionTransforms { get; set; }
    [field: SerializeField] public bool[] colorIndexing { get; set; }
    [field: SerializeField] public PlayerModelChanger[] ModelChangers { get; set; }
    [field: SerializeField] public PhotonView[] ModelPVs { get; set; }  
    [field: SerializeField] public bool[] IsPlayerPresent { get; set; }
    [field: SerializeField] public bool[] IsReady { get; set; }
    [field: SerializeField] public bool IsOriginalMaster { get; set; } = false;
    [field : SerializeField] public string roomName { get; set; }   
    [field : SerializeField] public string[] DefaultNames { get; set; }


    public int PlayerCount { get; set; }    
    public int MaxPlayerCount { get; set; } 
    public bool IsStartable { get; set; }
    public int MaxReadyCount { get; set; }
    public int ReadyCount { get; set; }
    public Room CurrentRoom { get; set; }

    public int BodyTypeCount { get; set; }
    public int HatTypeCount { get; set; }

    [field : SerializeField] public Color readyColor { get; set; } = Color.green;
    [field : SerializeField] public Color notReadyColor { get; set; } = Color.red;

    private Color getReadyColor(bool isReady)
    {
        if (isReady)
        {
            return readyColor;
        }
        else
        {
            return notReadyColor;
        }
    }

    private void OnEnable()
    {
        Managers.PhotonManager.OnJoinedTheRoom.RemoveListener(InitRoomData);
        Managers.PhotonManager.OnJoinedTheRoom.AddListener(InitRoomData);

        for (int enterOrder = 1; enterOrder < presenter.WaitingViews.Length ;++enterOrder)
        {
            presenter.WaitingViews[enterOrder].OnClickReadyButton -= getReadyColor;
            presenter.WaitingViews[enterOrder].OnClickReadyButton += getReadyColor;
        }



        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(UpdateRoomName);
        //Managers.PhotonManager.OnJoinedNewRoom.AddListener(UpdateRoomName);

        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsPlayerPresent = new bool[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.AddListener(() => IsPlayerPresent = new bool[MaxPlayerCount + 1]);

        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsReady = new bool[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.AddListener(() => IsReady = new bool[MaxPlayerCount + 1]);

        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => ModelChangers = new PlayerModelChanger[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.AddListener(() => ModelChangers = new PlayerModelChanger[MaxPlayerCount + 1]);

        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => ModelPVs = new PhotonView[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.AddListener(() => ModelPVs = new PhotonView[MaxPlayerCount + 1]);
        
        //BodyTypeCount = Managers.DataManager.Player.GetBodyTypeCount();
        //HatTypeCount = Managers.DataManager.Player.GetHatTypeCount();

        Managers.PhotonManager.OnJoinedTheRoom.RemoveListener(presenter.UpdateNewPlayerData);
        Managers.PhotonManager.OnJoinedTheRoom.AddListener(presenter.UpdateNewPlayerData);

        Managers.PhotonManager.OnOtherPlayerJoinedTheRoom.RemoveListener(presenter.UpdateNewOtherPlayerData);
        Managers.PhotonManager.OnOtherPlayerJoinedTheRoom.AddListener(presenter.UpdateNewOtherPlayerData);
    }

    private void InitRoomData()
    {
        SetRoomData();
        InitDataContainer();
        SetDefaultNames();
        UpdateRoomName();
    }

    private void SetRoomData()
    {
        CurrentRoom = PhotonNetwork.CurrentRoom;
        MaxPlayerCount = CurrentRoom.MaxPlayers;
        MaxReadyCount = MaxPlayerCount;
        roomName = CurrentRoom.Name;
    }

    private void InitDataContainer()
    {
        IsPlayerPresent = new bool[MaxPlayerCount + 1];
        IsReady = new bool[MaxPlayerCount + 1];
        ModelChangers = new PlayerModelChanger[MaxPlayerCount + 1];
        ModelPVs = new PhotonView[MaxPlayerCount + 1];
        DefaultNames = new string[MaxPlayerCount + 1];
        BodyTypeCount = Managers.DataManager.Player.GetBodyTypeCount();
        HatTypeCount = Managers.DataManager.Player.GetHatTypeCount();
        colorIndexing = new bool[BodyTypeCount];
    }

    private void OnDisable()
    {
        Managers.PhotonManager.OnJoinedTheRoom.RemoveListener(InitRoomData);
        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(UpdateRoomName);
        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsPlayerPresent = new bool[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => IsReady = new bool[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => ModelChangers = new PlayerModelChanger[MaxPlayerCount + 1]);
        //Managers.PhotonManager.OnJoinedNewRoom.RemoveListener(() => ModelPVs = new PhotonView[MaxPlayerCount + 1]);
        

        for (int enterOrder = 1; enterOrder < presenter.WaitingViews.Length; ++enterOrder)
        {
            presenter.WaitingViews[enterOrder].OnClickReadyButton -= getReadyColor;
        }

        Managers.PhotonManager.OnJoinedTheRoom.RemoveListener(presenter.UpdateNewPlayerData);

        Managers.PhotonManager.OnOtherPlayerJoinedTheRoom.RemoveListener(presenter.UpdateNewOtherPlayerData);
    }


    private void SetDefaultNames()
    {
        for (int i = 1; i < DefaultNames.Length; ++i)
        {
            DefaultNames[i] = $"Player {i}";
        }
    }

    public void UpdateRoomName() => roomNameText.text = roomName;

    public string GetDefaultName(int enterOrder) => DefaultNames[enterOrder];

    /// <summary>
    /// 주어진 인덱스에 따라 UI상의 몸 배경색을 반환하는 함수
    /// </summary>
    /// <param name="colorIndex"></param>
    /// <returns></returns>
    public Color GetBackgroundColorData(int colorIndex)
    {
        string bodyMaterialPath = Managers.DataManager.Player.BodyDialog[colorIndex]["Name"].ToString();
        Material bodyMaterial = Resources.Load<Material>(bodyMaterialPath);
        //Texture2D bodyTexture = Resources.Load<Texture2D>(bodyTexturePath);
        Color bodyColor = bodyMaterial.GetColor("_BaseColor");
        //return customData.colors[colorIndex];
        return bodyColor;
    }

    public void UpdateStartable()
    {
        if (MaxReadyCount <= ReadyCount)
        {
            IsStartable = true;
        }
        else
        {
            IsStartable = false;
        }
    }

    /// <summary>
    /// 주어진 인덱스에 따라 UI상의 모자 이름을 반환하는 함수
    /// </summary>
    /// <param name="hatIndex"></param>
    /// <returns></returns>
    public string GetBackgroundHatTextData(int hatIndex)
    {
        //return customData.hatTexts[hatIndex];
        string hatName = Managers.DataManager.Player.HatDialog[hatIndex]["Name"].ToString();
        return hatName;
    }

    public void CheckIfRoomStillOpen()
    {
        if (MaxPlayerCount <= PlayerCount)
        {
            CurrentRoom.IsOpen = false;
        }
        else
        {
            CurrentRoom.IsOpen = true;
        }
    }


    public int GetEmptySlot()
    {
        int emptyIndex = 9999;
        for (int slotIndex = 1; slotIndex < IsPlayerPresent.Length; ++slotIndex)
        {
            if (IsPlayerPresent[slotIndex] == false)
            {
                emptyIndex = slotIndex;
                return emptyIndex;
            }
        }

        return emptyIndex;
    }

    /// <summary>
    /// 색깔 배열의 선택가능 여부를 업데이트 해주는 함수
    /// </summary>
    /// <param name="enterOrder"></param>
    /// <param name="isTakeOver"></param>
    public void UpdateColorIndexing(int enterOrder, bool isTakeOver)
    {
        // true면 못가져감
        colorIndexing[enterOrder] = isTakeOver;
    }


    public int GetCapableHatIndex(int lastIndex, bool isRightButton)
    {
        if (isRightButton)
        {
            lastIndex += 1;
        }
        else
        {
            lastIndex -= 1;
        }

        if (lastIndex < 0)
        {
            lastIndex = HatTypeCount - 1;
        }
        else if (HatTypeCount <= lastIndex)
        {
            lastIndex = 0;
        }

        return lastIndex;
    }

    /// <summary>
    /// 색깔 배열의 선택 가능한 인덱스를 반환하는 함수
    /// </summary>
    /// <param name="lastIndex"></param>
    /// <param name="requestedIndex"></param>
    /// <param name="isRightButton"></param>
    /// <param name="isFirstEntry"></param>
    /// <returns></returns>
    public int GetCapableBodyIndex(int lastIndex, bool isRightButton, bool isFirstEntry)
    {
        if (!isFirstEntry)
        {
            colorIndexing[lastIndex] = false; // 현재 갖고 있는 색을 포기한다
        }


        int addValue = 0;

        if (isRightButton)
        {
            addValue = 1;
        }
        else
        {
            addValue = -1;
        }

        int targetIndex = lastIndex + addValue;
        if (targetIndex < 0)
        {
            targetIndex = BodyTypeCount - 1;
        }
        else if (BodyTypeCount <= targetIndex)
        {
            targetIndex = 0;
        }


        if (colorIndexing[targetIndex] == false)
        {
            colorIndexing[targetIndex] = true;
            return targetIndex;
        }
        else
        {
            while (true)
            {
                targetIndex += addValue;
                if (targetIndex < 0)
                {
                    targetIndex = BodyTypeCount - 1;
                }
                else if (BodyTypeCount <= targetIndex)
                {
                    targetIndex = 0;
                }


                if (colorIndexing[targetIndex] == false)
                {
                    colorIndexing[targetIndex] = true;
                    return targetIndex;
                }
            }
        }
    }


    /// <summary>
    /// 데이터의 포톤뷰를 반환하는 함수
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
    /// 데이터에 들어온 플레이어 관련 정보를 담는 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    /// <param name="enterOrder"></param>
    /// <param name="nickName"></param>
    /// <param name="bodyColorIndex"></param>
    /// <param name="hatIndex"></param>
    public void AddPlayerData(Player newPlayer, int enterOrder, string nickName, int bodyColorIndex, int hatIndex)
    {
        Managers.DataManager.Player.UpdatePlayerData(newPlayer, enterOrder);
        Managers.DataManager.Player.SetNickName(newPlayer, nickName);
        Managers.DataManager.Player.SetBodyID(newPlayer, bodyColorIndex);
        Managers.DataManager.Player.SetHatID(newPlayer, hatIndex);

        //playerOrderDictionary.Add(newPlayer, enterOrder);
        //playerNameDictionary.Add(newPlayer, nickName);
        //playerBodyColorDictionary.Add(newPlayer, bodyColorIndex);
        //playerHatDictionary.Add(newPlayer, hatIndex);
    }

    /// <summary>
    /// 데이터에서 나가는 플레이어 관련 정보를 없애는 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    public void RemovePlayerData(Player newPlayer)
    {
        //playerOrderDictionary.Remove(newPlayer);
        //playerNameDictionary.Remove(newPlayer);
        //playerBodyColorDictionary.Remove(newPlayer);
        //playerHatDictionary.Remove(newPlayer);
    }


    // 플레이어의 입장순서를 담을 Dictionary
    //private Dictionary<Player, int> playerOrderDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// 주어진 플레이어의 입장순서를 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    //public int GetPlayerEnterOrder(Player player)
    //{
    //    return playerOrderDictionary[player];
    //}

    // 플레이어의 닉네임을 담을 Dictionary
    private Dictionary<Player, string> playerNameDictionary = new Dictionary<Player, string>();

    /// <summary>
    /// 주어진 플레이어의 닉네임을 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public string GetPlayerNickName(Player player)
    {
        return playerNameDictionary[player];
    }

    /// <summary>
    /// 주어진 닉네임을 데이터에 저장하는 함수
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newNickName"></param>
    public void UpdateNickName(Player updatePlayer, String newNickName)
    {
        playerNameDictionary[updatePlayer] = newNickName;
    }


    // 플레이어의 몸 색깔을 담을 Dictionary
    private Dictionary<Player, int> playerBodyColorDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// 주어진 플레이어의 몸색 인덱스를 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerBodyColorIndex(Player player)
    {
        return playerBodyColorDictionary[player];
    }

    /// <summary>
    /// 주어진 몸색 인덱스를 데이터에 저장하는 함수
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newBodyIndex"></param>
    public void UpdateBodyIndex(Player updatePlayer, int newBodyIndex)
    {
        playerBodyColorDictionary[updatePlayer] = newBodyIndex;
    }


    // 플레이어가 착용하고 있는 모자를 담을 Dictionary
    private Dictionary<Player, int> playerHatDictionary = new Dictionary<Player, int>();

    /// <summary>
    /// 주어진 플레이어의 모자 인덱스를 저장된 데이터에서 뽑아 반환하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerHatIndex(Player player)
    {
        return playerHatDictionary[player];
    }

    /// <summary>
    /// 주어진 모자 인덱스를 데이터에 저장하는 함수
    /// </summary>
    /// <param name="updatePlayer"></param>
    /// <param name="newHatIndex"></param>
    public void UpdateHatIndex(Player updatePlayer, int newHatIndex)
    {
        playerHatDictionary[updatePlayer] = newHatIndex;
    }

}
