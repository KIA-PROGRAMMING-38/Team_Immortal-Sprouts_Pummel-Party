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

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        Get<TMP_Text>(Texts.Code).text = Managers.Data.GetText(Define.CodeText, Language.Eng);
        Get<TMP_Text>(Texts.CodeGuide).text = Managers.Data.GetText(Define.CodeGuideText, Language.Eng);
        Get<TMP_Text>(Texts.OK).text = Managers.Data.GetText(Define.OKText, Language.Eng);
        Get<TMP_Text>(Texts.Cancel).text = Managers.Data.GetText(Define.CancelText, Language.Eng);
    }




}
