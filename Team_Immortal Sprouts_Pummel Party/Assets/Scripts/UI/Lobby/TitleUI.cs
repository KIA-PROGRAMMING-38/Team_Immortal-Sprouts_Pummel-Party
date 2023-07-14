using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : UIBase
{
    enum Buttons
    {
        Init
    }

    enum Texts
    {
        Title,
        TouchGuide
    }

    private Button initButton = null;
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        initButton = Get<Button>(Buttons.Init);
        Get<TMP_Text>(Texts.Title).text = Managers.Data.GetText(Define.GameTitleText, Language.Eng);

        BindButtonEvent(Get<Button>(Buttons.Init), ClickInitButton);
    }

    private void OnEnable()
    {
        Managers.Photon.OnConnectedToMasterServer -= InitGame;
        Managers.Photon.OnConnectedToMasterServer += InitGame;
    }

    private void OnDisable()
    {
        Managers.Photon.OnConnectedToMasterServer -= InitGame;
    }

    private void InitGame()
    {
        initButton.interactable = true;
        Get<TMP_Text>(Texts.TouchGuide).text = Managers.Data.GetText(Define.TouchGuideText, Language.Eng);
    }

    private void ClickInitButton()
    {
        Managers.UI.CloseUI();
        Managers.UI.PopUI<MultiUI>(parent : Managers.UI.RootTransform);
    }

}
