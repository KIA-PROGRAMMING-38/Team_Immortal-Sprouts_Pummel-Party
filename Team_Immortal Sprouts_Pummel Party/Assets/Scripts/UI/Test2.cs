using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : UIBase
{
    enum Buttons
    {
        Button1,
        Button2
    }

    enum Texts
    {
        Text1,
        Text2   
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        BindButtonEvent(Get<Button>(Buttons.Button1), SayTEST1);
        BindButtonEvent(Get<Button>(Buttons.Button2), SayTEST2);

        Get<TMP_Text>(Texts.Text1).text = "Test1";
        Get<TMP_Text>(Texts.Text2).text = "Test2";
    }

    private void SayTEST1() => Debug.Log("TEST1");
    private void SayTEST2() => Debug.Log("TEST2");
}
