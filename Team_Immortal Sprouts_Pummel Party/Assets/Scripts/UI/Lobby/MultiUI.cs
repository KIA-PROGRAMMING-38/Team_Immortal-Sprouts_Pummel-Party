using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiUI : UIBase
{
    enum Buttons
    {
        JoinRandom,
        CreateRoom,
        FindRoom,
        LeaveGame
    }

    enum Texts
    {
        Join,
        Create,
        Find,
        Quit
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        Get<TMP_Text>(Texts.Join).text = Managers.Data.GetText(Define.JoinRandomText, Language.Eng);
        Get<TMP_Text>(Texts.Create).text = Managers.Data.GetText(Define.CreateRoomText, Language.Eng);
        Get<TMP_Text>(Texts.Find).text = Managers.Data.GetText(Define.FindRoomText, Language.Eng);
        Get<TMP_Text>(Texts.Quit).text = Managers.Data.GetText(Define.QuitGameText, Language.Eng);

        BindButtonEvent(Get<Button>(Buttons.JoinRandom), ShowCodeInputUI);
        BindButtonEvent(Get<Button>(Buttons.CreateRoom), ShowCodeInputUI);
    }


    private void ShowCodeInputUI()
    {
        Managers.UI.PopUI<RoomCodeUI>(parent: Managers.UI.RootTransform);
    }



}
