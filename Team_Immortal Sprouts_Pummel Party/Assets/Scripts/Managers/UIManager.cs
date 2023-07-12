using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    
    public void Init()
    {

    }

    public Dictionary<Type, UnityEngine.Object[]> ObjectDict = new Dictionary<Type, UnityEngine.Object[]>();
    public Stack<UIBase> UIStack { get; private set; } = new Stack<UIBase>();

    public T PopUI<T>(Enum key) where T : UIBase
    {
        int index = Convert.ToInt32(key);
        UnityEngine.Object chosenUI = ObjectDict[key.GetType()][index];

        return chosenUI as T;
    }
    

    public void CloseUI(Enum key)
    {
        int index = Convert.ToInt32(key);
        UnityEngine.Object chosenUI = ObjectDict[key.GetType()][index];

        Managers.Resource.Destroy(chosenUI);
    }
}
