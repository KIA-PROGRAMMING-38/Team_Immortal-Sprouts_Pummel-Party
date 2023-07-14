using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Testui : UIBase
{
    
    enum Buttons
    {
        Touch,
        TestButton
    }

    enum Texts
    {
        Title,
        TouchGuide,
        TestText
    }

    private Test2 secondUI;
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        BindButtonEvent(Get<Button>(Buttons.Touch), SayHey);
        BindButtonEvent(Get<Button>(Buttons.TestButton), SayWhat);
        BindButtonEvent(Get<Button>(Buttons.TestButton), SayBoom);

        Get<TMP_Text>(Texts.Title).text = "Game Title";
        Get<TMP_Text>(Texts.TouchGuide).text = "Touch to Start!";
        Get<TMP_Text>(Texts.TestText).text = "TestText";

        secondUI = Managers.UI.PopUI<Test2>();
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Managers.UI.CloseUI();
        }
    }

    private void SayHey() => Debug.Log("Hey");
    private void SayWhat() => Debug.Log("What");
    private void SayBoom() => Debug.Log("Boom");
}
