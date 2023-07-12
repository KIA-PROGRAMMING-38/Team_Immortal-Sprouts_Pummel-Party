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
        Touch
    }

    enum Texts
    {
        Title,
        TouchGuide
    }

    public override void Init()
    {
        Debug.Log(Managers.Data);
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        List<UnityAction> functionList = CreateFunctionList(SayHey, SayWhat, SayBoom);
        BindEventsWithButtons<Buttons>(functionList);
    }

    private void SayHey() => Debug.Log("Hey");
    private void SayWhat() => Debug.Log("What");
    private void SayBoom() => Debug.Log("Boom");
}
