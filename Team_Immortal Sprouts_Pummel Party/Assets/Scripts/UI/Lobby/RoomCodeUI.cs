using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCodeUI : UIBase
{
    enum Buttons
    {
        OK,
        Cancel
    }

    enum Texts
    {
        Code,
        CodeGuide,
        CodeInput,
        OK,
        Cancel
    }

    public bool isCreatingRoom { get; set; }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        Get<TMP_Text>(Texts.Code).text = Managers.Data.GetText(Define.CodeText, Language.Eng);
        Get<TMP_Text>(Texts.CodeGuide).text = Managers.Data.GetText(Define.CodeGuideText, Language.Eng);
        Get<TMP_Text>(Texts.OK).text = Managers.Data.GetText(Define.OKText, Language.Eng);
        Get<TMP_Text>(Texts.Cancel).text = Managers.Data.GetText(Define.CancelText, Language.Eng);

        BindButtonEvent(Get<Button>(Buttons.Cancel), ClickCancel);
    }


    private void ClickCancel()
    {
        Managers.UI.CloseUI(this);
    }

    private void CreateOrFindRoom()
    {
        string roomCode = Get<TMP_Text>(Texts.CodeInput).text;

        if (isCreatingRoom == true)
        {
            RoomOptions option = new RoomOptions();
            option.MaxPlayers = 4;
            
            if (string.IsNullOrEmpty(roomCode))
            {
                roomCode = Random.Range(0, 10000).ToString();
            }

            bool isExist = Managers.Photon.Rooms.ContainsKey(roomCode);
            
            if (isExist == false)
            {
                PhotonNetwork.CreateRoom(roomCode, option);
                PhotonNetwork.LoadLevel(1);
            }
            else
            {
                
            }
        }
        else
        {
            bool isExist = Managers.Photon.Rooms.ContainsKey(roomCode);
            
            if (isExist)
            {
                PhotonNetwork.JoinRoom(roomCode);
                PhotonNetwork.LoadLevel(1);
            }
            else
            {

            }
        }
    }
}
